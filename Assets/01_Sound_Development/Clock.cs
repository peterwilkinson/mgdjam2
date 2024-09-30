using UnityEngine;

public class Clock
{
    private float bpm;
    private double nextTick;
    private bool isRunning;
    private int currentBeat = 0;

    public Clock(float bpm)
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
        if (isRunning && AudioSettings.dspTime >= nextTick)
        {
            Tick();
            nextTick = AudioSettings.dspTime + GetInterval();
        }
    }

    private void Tick()
    {
        Debug.Log($"Tick: {currentBeat}");
        currentBeat++;
        currentBeat %= 4;

        // if (audioSource != null)
        // {
        //     audioSource.PlayScheduled(AudioSettings.dspTime);
        // }
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
}