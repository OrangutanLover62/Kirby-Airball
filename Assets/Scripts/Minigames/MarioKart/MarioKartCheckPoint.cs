using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioKartCheckPoint : MonoBehaviour
{

    [SerializeField] MarioKartCheckPoint previousCheckpoint;
    public List<PlayerMovement> passedPlayers;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Marble")
        {

            //Überprüft ob Spieler schon in der Liste ist und beim letzten Checkpoint war.
            if (!passedPlayers.Contains(other.GetComponent<PlayerMovement>()))
            {
                if (previousCheckpoint)
                {
                    if (previousCheckpoint.passedPlayers.Contains(other.GetComponent<PlayerMovement>()))
                    {
                        passedPlayers.Add(other.GetComponent<PlayerMovement>());
                    }
                }
                else
                {
                    passedPlayers.Add(other.GetComponent<PlayerMovement>());
                }
            }
        }
    }

    public void CompletedLap(PlayerMovement p)
    {
        passedPlayers.Remove(p);
        if (previousCheckpoint)
        {
            previousCheckpoint.CompletedLap(p);
        }
    }

}
