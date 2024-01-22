using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHandler : MonoBehaviour
{
    [SerializeField] private bool highScoreGood;
    [SerializeField] private List<Transform> startingPositions;

    public GameController gc;

    public float timer = 5;
    private bool minigameStarted = false;
    public List<string> results;


    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        results = new List<string>();
        SetUpMinigame();
    }

    public void SetUpMinigame()
    {
        int playerCounter = 0;
        foreach (PlayerMovement player in gc.players)
        {
            player.transform.position = startingPositions[playerCounter].position;
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            player.SetupMiniGame();
            player.enabled = false;
            playerCounter++;
        }
    }

    public void StartMinigame()
    {
        foreach (PlayerMovement player in gc.players)
        {
            player.enabled = true;
        }
        minigameStarted = true;
    }

    public void PlayerScores(string score)
    {
        results.Add(score);
        Debug.Log(score);
        if(results.Count == gc.players.Count)
        {
            if (!highScoreGood)
            {
                gc.GameOver(results);
            }else
            {
                results.Reverse();
                gc.GameOver(results);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (minigameStarted)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                StartMinigame();
            }
        }
    }

}
