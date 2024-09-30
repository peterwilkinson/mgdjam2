using UnityEngine;
using UnityEngine.Audio;

//** quick prototype code cm -- parts to be handled by an audiomanager singleton in future... **//

public class CollisionAudioHandler : MonoBehaviour
{
    public string mainCharacterTag = "Player";
    private AudioSource audioSource;
    private AudioHighPassFilter highPassFilter;
    private AudioEchoFilter audioEchoFilter;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        highPassFilter = GetComponent<AudioHighPassFilter>();
        audioEchoFilter = GetComponent<AudioEchoFilter>();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log($"Collision with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag(mainCharacterTag))
        {
            Debug.Log("Collision with player.");

          

            // Get the isPickedUp state from AudioManager
            if (AudioManager.Instance.loops.ContainsKey(gameObject.name))
            {
                var loopInfo = AudioManager.Instance.loops[gameObject.name];
                bool isPickedUp = loopInfo.isPickedUp;

                if (isPickedUp)
                {
                    // Set isPickedUp to false
                    AudioManager.Instance.SetPickedUp(gameObject.name, false);

                    // Set the object's transform to a random X location within 3 units from the center
                    Vector3 newPosition = transform.position;
                    newPosition.x = Random.Range(-3f, 3f);
                    transform.position = newPosition;
                    if (highPassFilter != null)
                    {
                        highPassFilter.enabled = true;
                        highPassFilter.cutoffFrequency = 500.0f; // Example value
                    }

                    if (audioSource != null)
                    {
                        audioSource.spatialBlend = 1.0f; 
                        audioSource.reverbZoneMix = 1.0f;
                    }
                    
                    if (audioEchoFilter != null)
                    {
                        audioEchoFilter.wetMix = 1.0f; // Example value
                        audioEchoFilter.enabled = true;
                    }
                        
                }
                else
                {
                    Debug.Log($"BeatsSinceDropped:{loopInfo.beatsSinceDropped}"); 
                    // Only allow state change if beatsSinceDropped is greater than 2
                    if (loopInfo.beatsSinceDropped > 2)
                    {
                        // Set isPickedUp to true
                        AudioManager.Instance.SetPickedUp(gameObject.name, true);
                        if(highPassFilter != null)
                        
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
                    }
                }
            }
        }
    }
}