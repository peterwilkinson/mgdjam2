using UnityEngine;
using UnityEngine.Audio;

//** quick prototype code cm -- parts to be handled by an audiomanager singleton in future... **//

public class CollisionAudioHandler : MonoBehaviour
{
    public string mainCharacterTag = "Player";
    private AudioSource audioSource;
    private AudioHighPassFilter highPassFilter;
    private AudioEchoFilter audioEchoFilter;
    
    // private AudioReverbZone reverbZone;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        highPassFilter = GetComponent<AudioHighPassFilter>();
        audioEchoFilter = GetComponent<AudioEchoFilter>();
        
        // reverbZone = GetComponent<AudioReverbZone>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log($"Collision with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag(mainCharacterTag))
        {
            Debug.Log("Collision with player.");

            // Disable the highpass filter
            if (highPassFilter != null)
            {
                highPassFilter.enabled = false;
                highPassFilter.cutoffFrequency = 0.0f;
            }

            if (audioSource != null)
            {
                audioSource.spatialBlend = 0.0f; 
                audioSource.reverbZoneMix = 0.0f;
            }
            
            if (audioEchoFilter != null)
            {
                // Debug.Log("Disable echo for this channel.");
                audioEchoFilter.wetMix = 0.0f;
                audioEchoFilter.enabled = false;
            }

            // Set isPickedUp to true in AudioManager
            AudioManager.Instance.SetPickedUp(gameObject.name, true);
        }
    }
}