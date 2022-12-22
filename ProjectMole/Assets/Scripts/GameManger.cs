using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static GameManger;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    private bool DebugMode;
    //private Transform[] beams;
    [SerializeField]
    private Transform[] holes;
    [SerializeField]
    private List <DaySO> daySOs;
    [SerializeField]
    private Dictionary<DayNumber, Day> daysDictionary;
    [SerializeField]
    private Timer timer;
    private List<Transform> currentBeams; //One by one 
    [SerializeField]
    private List<Transform> currentHoles; //One by one

    [SerializeField]
    private RoomsManager[] rooms;
    [SerializeField]
    private Beam[] beams;


    private GameState oldState;
    private GameState actualState;
    private int level;
    private float lightAmount; //you can loose before the end of the countdown if the light amout is biggern than.... 

    public enum GameState { Intro, SetDifficulty, SetBeams, SetHoles, LevelSetUp, Play, EndLevel, GameOver }
    public enum DayNumber { DayOne, DayTwo, DayThree, DayFour, DayFive, DaySix, DaySeven, DayEight, DayNine, DayTen }
    public enum TypeOfJobs { FixingHole, FixingBeam }

    public static GameManger instance;

    [SerializeField] public Day currentDay;

    private void Awake()
    {
        CreateDaysDictionary();
        DeactivateMarkers();
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Intro);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGameState(GameState newState)
    {
        oldState = actualState;
        actualState = newState;

        if (DebugMode)
        {
            Debug.Log($"{this.GetType().Name} - {MethodBase.GetCurrentMethod().Name}: GameStatus da {oldState} a {actualState} ");
        }

        switch(newState)
        {
            case GameState.Intro:
                Intro();
                break;
            case GameState.SetDifficulty:
                SetDifficulty();
                break;
            case GameState.LevelSetUp:
                LevelSetUp();
                break;
            case GameState.Play:
                Play();
                break;
            case GameState.EndLevel:
                EndLevel();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }
    void Intro()
    {
        level = 1;
        UpdateGameState(GameState.SetDifficulty);
    }
    void SetDifficulty()
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

        UpdateGameState(GameState.LevelSetUp);
    }

    //void SetBeams()
    //{
    //    Randomizer.Shuffle(beams);
    //    //currentBeams = new List<Transform>();
    //    for (int i = 0; i < currentDay.beams.Length; i++)
    //    {
    //        currentDay.beams[i].beamPosition = beams[i];
    //    }

    //    UpdateGameState(GameState.SetHoles);
    //}
    //void SetHoles()
    //{
    //    Randomizer.Shuffle(holes);
    //    //currentHoles = new List<Transform>();
    //    for (int i = 0; i < currentDay.holes.Length; i++)
    //    {
    //        currentDay.holes[i].holePosition = holes[i];
    //    }
    //    UpdateGameState(GameState.LevelSetUp);
    //}

    void LevelSetUp()
    {
        //Randomizer.Shuffle(beams);
        //Randomizer.Shuffle(holes);
        //int beamIndex = 0;
        //int beamholes = 0;

        //for (int i = 0; i < currentDay.jobs.Length; i++)
        //{
        //    if(currentDay.jobs[i].typeOfJobs == TypeOfJobs.FixingBeam)
        //    {
        //        currentDay.jobs[i].eventPosition = beams[i];
        //        beamIndex++;
        //    }
        //    else if (currentDay.jobs[i].typeOfJobs == TypeOfJobs.FixingHole)
        //    {
        //        currentDay.jobs[i].eventPosition = holes[i];
        //        beamholes++;
        //    }
        //}

        Timer.instance.SetTimer(currentDay.dayDuration, currentDay.jobs);

        UpdateGameState(GameState.Play);
    }

    void Play()
    {
        //Keep track of the timer, the holes 
        Timer.instance.StartCountDown();
        Timer.instance.countDownFinish += UpdateGameState;
        
    }
    void EndLevel()
    {
        Timer.instance.countDownFinish -= UpdateGameState;

        for(int i = 0; i < beams.Length; i ++)
        {
            if(beams[i].IsAnActiveHole() == true)
            {
                Debug.Log("GameOver");
            }
        }
        for(int i = 0; i < rooms.Length; i ++ )
        {
            if (rooms[i].IsThereAnHoleInThisRoom() == true)
            {
                Debug.Log("GameOver");
            }
        }
        Debug.Log("Next Level");

        Reset();
        level++;
        UpdateGameState(GameState.SetDifficulty);
    }
    void GameOver()
    {
        Reset();
        //Back To first Scene
    }

    public void NewJob(Job job)
    {
        switch(job.typeOfJobs)
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
                beams[i].isAnActiveHole = true;
                return;
            }
        }
    }

    public void GenerateHole()
    {
        Randomizer.Shuffle(rooms);

        if (!rooms[0].IsThereAnHoleInThisRoom())
        {
            if(DebugMode)
            {
                Debug.Log("First try: There is no hole in this room " + rooms[0].transform.name);
            }
            rooms[0].GenerateHole();
        }
        else if (!rooms[1].IsThereAnHoleInThisRoom())
        {
            if (DebugMode)
            {
                Debug.Log("Second try: There is no hole in this room " + rooms[1].transform.name);
            }
            rooms[1].GenerateHole();
        }
        else if (!rooms[2].IsThereAnHoleInThisRoom())
        {
            if (DebugMode)
            {
                Debug.Log("Third try: There is no hole in this room " + rooms[2].transform.name);
            }
            rooms[2].GenerateHole();
        }
        else
        {
            if (DebugMode)
            {
                Debug.Log("Forth try: all rooms have an holes ");
            }
            //rooms[0].GenerateHole();
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
        for(int i = 0; i < holes.Length; i++)
        {
            if (holes[i].GetComponent<Marker>())
            {
                IMarkable c;
                holes[i].TryGetComponent<IMarkable>(out c);
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
    void Reset()
    {
        for(int i = 0; i < rooms.Length; i ++)
        {
            rooms[i].Reset();
        }
        for(int i = 0; i < beams.Length; i ++)
        {
            beams[i].Reset();
        }
    }
}

[System.Serializable]
public struct Day
{
    [SerializeField]
    public GameManger.DayNumber dayName { get; set; }

    [SerializeField]
    public Job [] jobs;
    //[SerializeField]
    //public Holes [] holes { get; set; }

    //[SerializeField]
    //public Beams [] beams { get; set; }

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
    public GameManger.TypeOfJobs typeOfJobs;

    public float timeInSeconds;

    [HideInInspector]
    public Transform eventPosition { get; set; }
}

//[Serializable]
//public class Beams
//{
//    public float timeInSeconds;

//    [HideInInspector]
//    public Transform beamPosition { get; set; }
//}