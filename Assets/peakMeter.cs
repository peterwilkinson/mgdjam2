using UnityEngine;

public class PeakMeter : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject targetObject;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    public int frequencyIndexStart = 0;
    public int frequencyIndexEnd = 64;
    public bool useSmoothing = true;
    public float smoothingSpeed = 5f;

    private float[] spectrumData = new float[1024];
    private float currentAmplitude = 0f;
    private Renderer objectRenderer;
    
    private Collider objectCollider;

    //* Amplitude scaling *//
    private const float MinAmplitude = 1e-7f; // Minimum amplitude to avoid log(0)
    private const float RefValue = 1e-7f; // Reference value for dB conversion

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (targetObject == null)
            targetObject = this.gameObject;

        objectRenderer = targetObject.GetComponent<Renderer>();

        frequencyIndexEnd = Mathf.Clamp(frequencyIndexEnd, 1, spectrumData.Length);
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);

        //* RMS value calculation *//
        
        float sum = 0f;
        for (int i = frequencyIndexStart; i < frequencyIndexEnd; i++)
        {
            float value = spectrumData[i];
            sum += value * value;
        }
        float rmsValue = Mathf.Sqrt(sum / (frequencyIndexEnd - frequencyIndexStart));
        float dbValue = 20 * Mathf.Log10(rmsValue / RefValue);

        if (useSmoothing)
        {
            currentAmplitude = Mathf.Lerp(currentAmplitude, dbValue, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            currentAmplitude = dbValue;
        }

        Debug.Log("Amplitude: " + currentAmplitude);
        
        //* No action for now *//
        
        // Color amplitudeColor = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(-80, 0, currentAmplitude));
        // objectRenderer.material.color = amplitudeColor;
        // float scale = 0.1f + currentAmplitude;
        
        // if (audioSource.isPlaying)
        // {
        //     transform.localScale = new Vector3(scale, scale, scale);

        //     if (objectCollider is BoxCollider boxCollider)
        //     {
        //         boxCollider.size = new Vector3(scale, scale, scale);
        //     }
        //     else if (objectCollider is SphereCollider sphereCollider)
        //     {
        //         sphereCollider.radius = scale / 2f;
        //     }
        //     else if (objectCollider is CapsuleCollider capsuleCollider)
        //     {
        //         capsuleCollider.height = scale;
        //         capsuleCollider.radius = scale / 2f;
        //     }
        // }
    }
}