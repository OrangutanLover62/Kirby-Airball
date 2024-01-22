using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{

    [SerializeField] Transform stage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Marble")
        {
            other.GetComponent<PlayerMovement>().cam.LookAt = stage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Marble")
        {
            other.GetComponent<PlayerMovement>().cam.LookAt = other.gameObject.transform;
        }
    }
}
