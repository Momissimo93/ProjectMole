using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    private bool DebugMode;
    [SerializeField]
    private Transform[] beams;
    [SerializeField]
    private Transform[] holes;
    [SerializeField]
    private List <DaySO> daySOs;
    [SerializeField]
    private Dictionary<Level, Day> daysDictionary;
    [SerializeField]
    private Timer timer;

    private GameState oldState;
    private GameState actualState;
    private int level;
    private float lightAmount; //you can loose before the end of the countdown if the light amout is biggern than.... 
    private List<Transform> currentBeams; //One by one 
    private List<Transform> currentHoles; //One by one

    public enum GameState { Intro, SetDifficulty, SetBeams, SetHoles, LevelSetUp, Play, EndLevel, GameOver }
    public enum Level { DayOne, DayTwo, DayThree, DayFour, DayFive, DaySix, DaySeven, DayEight, DayNine, DayTen }

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
            case GameState.SetBeams:
                SetBeams();
                break;
            case GameState.SetHoles:
                SetHoles();
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
        switch(level)
        {
            case 1:
                if(daysDictionary.ContainsKey(Level.DayOne))
                {
                    currentDay = daysDictionary[Level.DayOne];
                    if (DebugMode) { Debug.Log("CurrentDay " + currentDay.dayName + " " +  currentDay.numberOfHoles + " " + currentDay.numberOfBeams + " " + currentDay.delta + " " + currentDay.dayDuration); }
                }
                break;
            case 2:
                if (daysDictionary.ContainsKey(Level.DayOne))
                {
                    currentDay = daysDictionary[Level.DayOne];
                }
                break;
        } 

        UpdateGameState(GameState.SetBeams);
    }
    void SetBeams()
    {
        Randomizer.Shuffle(beams);
        currentBeams = new List<Transform>();
        //for (int i = 0; i < numberOfBeams; i++)
        //{
        //    currentBeams.Add(beams[i]);
        //}

        UpdateGameState(GameState.SetHoles);
    }
    void SetHoles()
    {
        Randomizer.Shuffle(holes);
        currentHoles = new List<Transform>();
        //for (int i = 0; i < numberOfHoles; i++)
        //{
        //    currentHoles.Add(holes[i]);
        //}
        UpdateGameState(GameState.LevelSetUp);
    }

    void LevelSetUp()
    {

        timer.SetTime(currentDay.dayDuration);
        timer.StartCountDown();

        //UpdateGameState(GameState.Play);
    }

    void Play()
    {
        //Keep track of the timer, the holes 
        UpdateGameState(GameState.EndLevel);
    }
    void EndLevel()
    {
        //Or SetDifficulty or GameOver()
        //timer = 5;
        level++;
    }
    void GameOver()
    {
        //Back To first Scene
    }

    void CreateDaysDictionary()
    {
        daysDictionary = new Dictionary<Level, Day>();

        foreach (DaySO daySO in daySOs)
        {
            Day day = new Day();
            day.dayName = daySO.dayName;
            day.numberOfHoles = daySO.numberOfHoles;
            day.numberOfBeams = daySO.numberOfBeams;
            day.delta = daySO.delta;
            day.dayDuration = daySO.dayDuration;

            if (DebugMode)
            {
                Debug.Log($"{MethodBase.GetCurrentMethod().Name}: add {day.dayName} a {daysDictionary} ");
            }

            daysDictionary.Add(day.dayName, day);
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
}

[System.Serializable]
public struct Day
{
    [SerializeField]
    public GameManger.Level dayName { get; set; }
    [SerializeField]
    public int numberOfHoles { get; set; }
    [SerializeField]
    public int numberOfBeams { get; set; }
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
