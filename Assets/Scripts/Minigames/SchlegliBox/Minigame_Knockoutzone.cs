using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Knockoutzone : MonoBehaviour
{
    [SerializeField] MinigameHandler minigameMaster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Marble")
        {
            minigameMaster.PlayerScores(other.name + " " + minigameMaster.timer);
            Destroy(other.gameObject);
            if(minigameMaster.results.Count == minigameMaster.gc.players.Count - 1)
            {
                minigameMaster.PlayerScores("Wintime: " + minigameMaster.timer);
            }
        }
    }

}
