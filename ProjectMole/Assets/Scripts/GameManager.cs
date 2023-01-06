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
    private Dictionary <int, Day> daysDictionary;
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Room[] rooms;
    [SerializeField]
    private Beam[] beams;

    private GameState oldState;
    private GameState actualState;
    private int level;

    public Day currentDay;

    public enum GameState {Intro, SetCurrentLevel, SetTimerEvents, LevelSetUp, Play, EndLevel, JobDone, GameOver }

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

        if (daysDictionary.ContainsKey(level))
        {
            Day value;
            daysDictionary.TryGetValue(level, out value);
            currentDay = value;
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
            if (!beams[i].IsBroken())
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

        if (!rooms[0].IsBroken())
        {
            if (DebugMode)
            {
                Debug.Log("First try: There is no hole in this room " + rooms[0].transform.name);
            }
            rooms[0].GenerateHole();
            ActivateMarker(rooms[0].gameObject);
        }
        else if (!rooms[1].IsBroken())
        {
            if (DebugMode)
            {
                Debug.Log("Second try: There is no hole in this room " + rooms[1].transform.name);
            }
            rooms[1].GenerateHole();
            ActivateMarker(rooms[0].gameObject);
        }
        else if (!rooms[2].IsBroken())
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
        daysDictionary = new Dictionary<int, Day>();

        foreach (DaySO daySO in daySOs)
        {
            Day day = new Day();

            day.dayNumber = daySO.dayNumber;
            day.jobs = daySO.jobs;
            day.dayDuration = daySO.dayDuration;

            daysDictionary.Add(day.dayNumber, day);

            if (DebugMode)
            {
                Debug.Log($"{MethodBase.GetCurrentMethod().Name}: add {day.dayNumber} a {daysDictionary} ");
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

    bool IsJobDone(Room[] rooms, Beam[] beams)
    {
        for (int i = 0; i < beams.Length; i++)
        {
            if (beams[i].IsBroken() == true)
            {
                return false;
            }
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsBroken() == true)
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
    public int dayNumber;

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

