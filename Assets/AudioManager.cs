using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    _instance = obj.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    private Clock clock;
    public Dictionary<string, LoopInfo> loops = new Dictionary<string, LoopInfo>();
    public Dictionary<AudioSource, float> audioAmplitudes = new Dictionary<AudioSource, float>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        
        Debug.Log("AudioManager Awake");
        clock = Clock.Instance;
        clock.Initialize(120); // Initialize the clock with BPM
        clock.OnTick += HandleTick; 
        clock.Start();
    }

    private void HandleTick(int beat)
    {
        bool allPickedUp = true;
        foreach (var loop in loops.Values)
        {
            if (!loop.isPickedUp)
            {
                allPickedUp = false;
                loop.beatsSinceDropped++;
            }
            else
            {
                loop.beatsSinceDropped++;
            }
        }

        if (allPickedUp)
        {
            if (beat == 0)
            {
                foreach (var loop in loops.Values)
                {
                    loop.audioSource.Stop();
                    loop.audioSource.Play();
                    loop.beatsSinceDropped = 0;
                }
            }
        }
        else
        {
            foreach (var loop in loops.Values)
            {
                if (beat % 2 == 0 && !loop.isPickedUp)
                {
                    loop.audioSource.Stop();
                    loop.audioSource.Play();
                }
                
                if (beat % 4 == 0 && loop.isPickedUp)
                {
                    loop.audioSource.Stop();
                    loop.audioSource.Play();
                    loop.beatsSinceDropped = 0;
                }
            }
        }
    }

    

    public void AddLoop(string key, AudioSource audioSource, bool isPickedUp)
    {
        if (!loops.ContainsKey(key))
        {
            loops.Add(key, new LoopInfo(audioSource, isPickedUp));
        }
    }

    public void SetPickedUp(string key, bool isPickedUp)
    {
        if (loops.ContainsKey(key))
        {
            loops[key].isPickedUp = isPickedUp;
            if (!isPickedUp)
            {
                loops[key].beatsSinceDropped = 0;
            }
        }
    }

    public void SetBPM(float newBPM)
    {
        clock.SetBPM(newBPM);
    }

    public void StartClock()
    {
        clock.Start();
    }

    public void StopClock()
    {
        clock.Stop();
    }

    void OnDestroy()
    {
        if (clock != null)
        {
            clock.OnTick -= HandleTick;
        }
    }
}

// Helper class to store AudioSource and its picked-up state
public class LoopInfo
{
    public AudioSource audioSource;
    public bool isPickedUp;
    public int beatsSinceDropped;

    public LoopInfo(AudioSource audioSource, bool isPickedUp)
    {
        this.audioSource = audioSource;
        this.isPickedUp = isPickedUp;
        this.beatsSinceDropped = 0;
    }
}