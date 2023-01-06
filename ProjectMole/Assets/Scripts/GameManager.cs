using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool DebugMode;
    [SerializeField]
    private List<DaySO> daySOs;
    [SerializeField]
    private Dictionary <DayNumber, Day> daysDictionary;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private RoomsManager[] rooms;
    [SerializeField]
    private Beam[] beams;

    private GameState oldState;
    private GameState actualState;
    private int level;

    public Day currentDay;

    public enum GameState {Intro, SetCurrentLevel, SetTimerEvents, LevelSetUp, Play, EndLevel, JobDone, GameOver }
    public enum DayNumber {DayOne, DayTwo, DayThree, DayFour, DayFive, DaySix, DaySeven, DayEight, DayNine, DayTen}
    public enum TypeOfJobs {FixingHole, FixingBeam}

    public static GameManager instance;


    private void Awake()
    {
        CreateDaysDictionary();
        DeactivateMarkers();
        instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Intro);
    }

    public void UpdateGameState(GameState newState)
    {
        oldState = actualState;
        actualState = newState;

        if (DebugMode)
        {
            Debug.Log($"{this.GetType().Name} - {MethodBase.GetCurrentMethod().Name}: GameStatus da {oldState} a {actualState} ");
        }

        switch (newState)
        {
            case GameState.Intro:
                Intro();
                break;
            case GameState.SetCurrentLevel:
                SetCurrentLevel();
                break;
            case GameState.SetTimerEvents:
                SetTimerEvents();
                break;
            case GameState.Play:
                Play();
                break;
            case GameState.EndLevel:
                EndLevel();
                break;
            case GameState.JobDone:
                JobDone();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }
    void Intro()
    {
        level = 1;
        UpdateGameState(GameState.SetCurrentLevel);
    }
    void SetCurrentLevel()
    {
        if (DebugMode)
        {
            Debug.Log("CurrentDay " + level);
        }
        switch (level)
        {
            case 1:
                if (daysDictionary.ContainsKey(DayNumber.DayOne))
                {
                    currentDay = daysDictionary[DayNumber.DayOne];
                }
                break;
            case 2:
                if (daysDictionary.ContainsKey(DayNumber.DayTwo))
                {
                    currentDay = daysDictionary[DayNumber.DayTwo];
                }
                break;
            case 3:
                if (daysDictionary.ContainsKey(DayNumber.DayThree))
                {
                    currentDay = daysDictionary[DayNumber.DayThree];
                }
                break;
            case 4:
                if (daysDictionary.ContainsKey(DayNumber.DayFour))
                {
                    currentDay = daysDictionary[DayNumber.DayFour];
                }
                break;
        }

        UpdateGameState(GameState.SetTimerEvents);
    }

    void SetTimerEvents()
    {
        Timer.instance.SetTimer(currentDay.dayDuration, currentDay.jobs);
        UpdateGameState(GameState.Play);
    }

    void Play()
    {
        Timer.instance.StartCountDown();
        Timer.instance.countDownFinish += UpdateGameState;

    }
    void EndLevel()
    {
        Timer.instance.countDownFinish -= UpdateGameState;

        if (IsJobDone(rooms, beams))
        {
            Reset();
            UpdateGameState(GameState.JobDone);
        }
        else
        {
            Reset();
            UpdateGameState(GameState.GameOver);
        }
    }
    void JobDone()
    {
        level++;
        CanvasManager.instance.JobDone();
    }
    void GameOver()
    {
        CanvasManager.instance.GameOver();
    }
    public void NewJob(Job job)
    {
        switch (job.typeOfJobs)
        {
            case TypeOfJobs.FixingBeam:
                DestroyBeam();
                break;
            case TypeOfJobs.FixingHole:
                GenerateHole();
                break;
        }
    }
    public void DestroyBeam()
    {
        Randomizer.Shuffle(beams);
        for (int i = 0; i < beams.Length; i++)
        {
            if (!beams[i].IsAnActiveHole())
            {
                beams[i].Break();
                ActivateMarker(beams[i].gameObject);
                return;
            }
        }
    }
    public void GenerateHole()
    {
        Randomizer.Shuffle(rooms);

        if (!rooms[0].IsThereAnHoleInThisRoom())
        {
            if (DebugMode)
            {
                Debug.Log("First try: There is no hole in this room " + rooms[0].transform.name);
            }
            rooms[0].GenerateHole();
            ActivateMarker(rooms[0].gameObject);
        }
        else if (!rooms[1].IsThereAnHoleInThisRoom())
        {
            if (DebugMode)
            {
                Debug.Log("Second try: There is no hole in this room " + rooms[1].transform.name);
            }
            rooms[1].GenerateHole();
            ActivateMarker(rooms[0].gameObject);
        }
        else if (!rooms[2].IsThereAnHoleInThisRoom())
        {
            if (DebugMode)
            {
                Debug.Log("Third try: There is no hole in this room " + rooms[2].transform.name);
            }
            rooms[2].GenerateHole();
            ActivateMarker(rooms[0].gameObject);
        }
        else
        {
            if (DebugMode)
            {
                Debug.Log("Forth try: all rooms have an holes ");
            }
        }
    }
    void CreateDaysDictionary()
    {
        daysDictionary = new Dictionary<DayNumber, Day>();

        foreach (DaySO daySO in daySOs)
        {
            Day day = new Day();

            day.dayName = daySO.dayName;
            day.jobs = daySO.jobs;
            day.dayDuration = daySO.dayDuration;
            daysDictionary.Add(day.dayName, day);

            if (DebugMode)
            {
                Debug.Log($"{MethodBase.GetCurrentMethod().Name}: add {day.dayName} a {daysDictionary} ");
            }
        }
    }
    void DeactivateMarkers()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].GetComponent<Marker>())
            {
                IMarkable c;
                rooms[i].TryGetComponent<IMarkable>(out c);
                c.DeactivateMarker();
            }
        }
        for (int i = 0; i < beams.Length; i++)
        {
            if (beams[i].GetComponent<Marker>())
            {
                IMarkable c;
                beams[i].TryGetComponent<IMarkable>(out c);
                c.DeactivateMarker();
            }
        }
    }
    bool IsJobDone(RoomsManager[] rooms, Beam[] beams)
    {
        for (int i = 0; i < beams.Length; i++)
        {
            if (beams[i].IsAnActiveHole() == true)
            {
                return false;
            }
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsThereAnHoleInThisRoom() == true)
            {
                return false;
            }
        }
        return true;
    }

    void ActivateMarker(GameObject g)
    {
        if (g.GetComponent<Marker>())
        {
            IMarkable c;
            g.TryGetComponent<IMarkable>(out c);
            c.ActivateMarker();
        }
    }
    public void DeAtivateMarker(GameObject g)
    {
        if (g.GetComponent<Marker>())
        {
            IMarkable c;
            g.TryGetComponent<IMarkable>(out c);
            c.DeactivateMarker();
        }
    }
    void Reset()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Reset();
        }
        for (int i = 0; i < beams.Length; i++)
        {
            beams[i].Reset();
        }
    }
}

[System.Serializable]
public struct Day
{
    [SerializeField]
    public GameManager.DayNumber dayName { get; set; }

    [SerializeField]
    public Job[] jobs;

    [SerializeField]
    public float delta { get; set; }

    [SerializeField]
    public float dayDuration { get; set; }

}
public class Randomizer
{
    public static void Shuffle<T>(T[] items)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < items.Length - 1; i++)
        {
            int j = rand.Next(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}

[Serializable]
public class Job
{
    public GameManager.TypeOfJobs typeOfJobs;

    public float timeInSeconds;

    [HideInInspector]
    public Transform eventPosition { get; set; }
}

