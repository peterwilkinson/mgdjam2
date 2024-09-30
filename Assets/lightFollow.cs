using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    public float followSpeed = 1.0f;
    public float rotationSpeed = 1.0f;

    private Transform playerTransform;
    private Transform lightTransform;

    void Start()
    {
        
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        
        GameObject lightObject = GameObject.FindWithTag("Light");
        if (lightObject != null)
        {
            lightTransform = lightObject.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null && lightTransform != null)
        {
        
            Vector3 targetPosition = playerTransform.position + Vector3.up * 10.0f; // spotlight loosely fits the listener field
            lightTransform.position = Vector3.Lerp(lightTransform.position, targetPosition, followSpeed * Time.deltaTime);

            Vector3 directionToPlayer = playerTransform.position - lightTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            lightTransform.rotation = Quaternion.Slerp(lightTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}