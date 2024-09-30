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
        clock = new Clock(120);
        clock.Start();
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
}