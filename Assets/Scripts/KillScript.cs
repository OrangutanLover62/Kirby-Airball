using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class KillScript : MonoBehaviour
{
    GameController gc;
    public Button newGameButton;
    private GameObject c_player;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Marble")
        {
            c_player = collider.gameObject;
            c_player.GetComponent<Transform>().position = new Vector3(0, 15, 0);
            c_player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            // ToDo: Killzone necessary?
            collider.gameObject.GetComponent<PlayerStatHandler>().DropUpgrades(5);

        }
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
