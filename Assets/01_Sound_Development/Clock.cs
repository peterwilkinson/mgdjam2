using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    private static Clock _instance;
    public static Clock Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("Clock");
                _instance = obj.AddComponent<Clock>();
            }
            return _instance;
        }
    }

    private float bpm;
    private double nextTick;
    private bool isRunning;
    private int currentBeat = 0;

    // Define an event to be triggered on each tick
    public event Action<int> OnTick;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Initialize(float bpm)
    {
        this.bpm = bpm;
        nextTick = AudioSettings.dspTime + GetInterval();
        isRunning = false;
    }

    private double GetInterval()
    {
        return 60.0 / bpm;
    }

    public void UpdateClock()
    {
        while (isRunning && AudioSettings.dspTime >= nextTick)
        {
            Tick();
             //** test these two approaches **//
            // nextTick = AudioSettings.dspTime + GetInterval();
            nextTick += GetInterval();
        }
    }

    private void Tick()
    {
        // Debug.Log($"Tick: {currentBeat}");
        OnTick?.Invoke(currentBeat); // Trigger the event and pass the current beat
        currentBeat++;
        currentBeat %= 16;
    }

    public void SetBPM(float newBPM)
    {
        bpm = newBPM;
        nextTick = AudioSettings.dspTime + GetInterval();
    }

    public void Start()
    {
        isRunning = true;
        nextTick = AudioSettings.dspTime + GetInterval();
    }

    public void Stop()
    {
        isRunning = false;
    }

    private void Update()
    {
        UpdateClock();
    }
}
           