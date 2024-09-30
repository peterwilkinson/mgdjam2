using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundContainer : MonoBehaviour
{
    [SerializeField]
    GameObject owner;
    bool isOwned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOwned) return;
        if (Vector3.Distance(transform.position, owner.transform.position) <= .03f)
        {
            transform.parent = owner.transform;
            isOwned = true;
        }
        ToOwner();
    }

    void ToOwner()
    {
        transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, .03f);
        //transform.position = Vector3.Lerp(transform.position, owner.transform.position, 1 * Time.deltaTime);
    }
}
