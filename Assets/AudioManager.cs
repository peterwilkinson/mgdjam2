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
    public Dictionary<string, AudioSource> loops = new Dictionary<string, AudioSource>();
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
        if (beat == 0)
        {
            RestartAllLoops();
        }
    }
     private void RestartAllLoops()
    {
        foreach (var loop in loops.Values)
        {
            loop.Stop();
            loop.Play();
        }
    }
     void Update()
    {
        clock.UpdateClock();
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
            clock.OnTick -= HandleTick; // Unsubscribe from the OnTick event
        }
    }
}