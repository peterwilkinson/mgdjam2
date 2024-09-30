//* commented out as I still get an error even though I don't load the script in the scene *//

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerControl : MonoBehaviour
// {

//     CharacterController characterController;

//     [SerializeField]
//     float speed = 5f;
//     // Start is called before the first frame update
//     void Start()
//     {
//         characterController = GetComponent<CharacterController>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//         characterController.Move(move * Time.deltaTime * speed);
//     }
// }
