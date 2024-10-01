using UnityEngine;
using UnityEngine.Audio;

//** quick prototype code cm -- parts to be handled by an audiomanager singleton in future... **//

public class CollisionAudioHandler : MonoBehaviour
{
    public string mainCharacterTag = "Player";
    private AudioSource audioSource;
    private AudioHighPassFilter highPassFilter;
    private AudioEchoFilter audioEchoFilter;
    public Renderer playerRenderer;

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
            
            Renderer playerRenderer = collision.gameObject.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                Color playerColor = playerRenderer.material.color;

                Renderer collidingRenderer = GetComponent<Renderer>();
                if (collidingRenderer != null)
                {
                    Color collidingBaseColor = collidingRenderer.material.color;
                    Color resultingColor = playerColor + (collidingBaseColor * 0.1f);

                    playerRenderer.material.color = resultingColor;
                    GameObject lightObject = GameObject.FindWithTag("Light");
                    if (lightObject != null)
                    {
                        Light lightComponent = lightObject.GetComponent<Light>();
                        if (lightComponent != null)
                        {
                            lightComponent.color = resultingColor + (playerColor / 2.0f);
                        }
                    }
                }
            }
            // Set isPickedUp to true in AudioManager
            AudioManager.Instance.SetPickedUp(gameObject.name, true);
        }
    }
}