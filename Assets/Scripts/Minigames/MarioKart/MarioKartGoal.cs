using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioKartGoal : MonoBehaviour
{
    [SerializeField] MinigameHandler minigame;
    [SerializeField] MarioKartCheckPoint lastCheckPoint;
    [SerializeField] int lapCount;

    Dictionary<PlayerMovement, int> playersCompletedLaps;

    private void Awake()
    {
        playersCompletedLaps = new Dictionary<PlayerMovement, int>();
        foreach (PlayerMovement player in minigame.gc.players)
        {

            playersCompletedLaps.Add(player, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lastCheckPoint.passedPlayers.Contains(other.GetComponent<PlayerMovement>()))
        {
            lastCheckPoint.CompletedLap(other.GetComponent<PlayerMovement>());
            playersCompletedLaps[other.GetComponent<PlayerMovement>()] += 1;
            Debug.Log("Lap Completed");
            if (playersCompletedLaps[other.GetComponent<PlayerMovement>()] >= lapCount)
            {
                minigame.PlayerScores(other.gameObject.name + " " + System.TimeSpan.FromSeconds((double)minigame.timer).ToString("mm':'ss"));
            }
        }
    }

}
