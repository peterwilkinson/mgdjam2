using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{

    public class PlayerControl : MonoBehaviour
    {

        CharacterController characterController;

        [SerializeField]
        float movementSpeed = 4f;
        [SerializeField]
        float rotationSpeed = 300;
        [SerializeField]
        float rotationSensitivity = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
            // print(move.magnitude);
            characterController.Move(move * Time.deltaTime * movementSpeed);
            if (move.magnitude > rotationSensitivity)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

        }
    }

}