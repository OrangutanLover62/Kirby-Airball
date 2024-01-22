using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    [SerializeField] PlayerMovement player;

    private float groundedTimer;
    public List<GameObject> collidingObjects;
    private Vector3 c_position;



    public void ResetList()
    {

        collidingObjects = new List<GameObject>();
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
        {
            if (!collidingObjects.Contains(other.gameObject))
            {
                collidingObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Ground")
        {
                collidingObjects.Remove(collision.gameObject);
        }
    }

    private void Update()
    {

        c_position = player.transform.position;
        c_position.y -= 1.7f;
        this.transform.position = c_position;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if(collidingObjects.Count == 0)
        {
            if (player.grounded == true)
            {
                player.glideTime = 0;
            }
            player.grounded = false;
        }
        else
        {
            player.grounded = true;
        }
    }

}
