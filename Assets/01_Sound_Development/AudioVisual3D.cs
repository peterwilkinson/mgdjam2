using UnityEngine;
using UnityEngine.Audio;

public class AudioVisual3D : MonoBehaviour
{
    public Color baseColor = Color.white;
    public float sensitivity = 1.0f;
    public float lightIntensityMultiplier = 10.0f;
    public double startTime; // sync purposes

    private AudioSource audioSource;
    private float[] clipSampleData;
    private float currentAmplitude = 0f;
    private Renderer objectRenderer;
    private Rigidbody rb;
    private Vector3 direction;
    // private float changeDirectionTime = 1f;
    // private float changeDirectionTimer = 0f;
    // private float moveSpeed = 0f;
    private Collider objectCollider;
    private AudioHighPassFilter highPassFilter;
    private bool isPickedUp = false; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        highPassFilter = GetComponent<AudioHighPassFilter>();

        clipSampleData = new float[1024];
        
        
        //** play start will be handled by the audiomanager **//
        if (!AudioManager.Instance.loops.ContainsKey(gameObject.name))
        {
            AudioManager.Instance.AddLoop(gameObject.name, audioSource, isPickedUp);
        }

    }

    void Update()
    {
        audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.BlackmanHarris);
        currentAmplitude = 0f;
        foreach (var sample in clipSampleData)
        {
            currentAmplitude += sample;
        }
        currentAmplitude /= clipSampleData.Length;

        Color amplitudeColor = baseColor * (currentAmplitude * sensitivity * 500);
        objectRenderer.material.color = amplitudeColor;

        float scale = 0.1f + currentAmplitude * sensitivity;

        bool isPickedUp = AudioManager.Instance.loops.ContainsKey(gameObject.name) && AudioManager.Instance.loops[gameObject.name].isPickedUp;
        Debug.Log("isPickedUp: " + isPickedUp);

        if (isPickedUp)
        {
            scale = 0.1f;
            transform.localScale = new Vector3(scale, scale, scale);
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float speed = 5f;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else if (audioSource.isPlaying)
        {
            transform.localScale = new Vector3(scale * 2, Mathf.Exp(scale) * 2, scale * 2);

            if (objectCollider is BoxCollider boxCollider)
            {
                boxCollider.size = new Vector3(scale * 2, Mathf.Exp(scale) * 2, scale * 2);
            }
            else if (objectCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = scale / 2f;
            }
            else if (objectCollider is CapsuleCollider capsuleCollider)
            {
                capsuleCollider.height = scale;
                capsuleCollider.radius = scale / 2f;
            }
        }
    }
}