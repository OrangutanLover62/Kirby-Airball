using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;

public class GameController : MonoBehaviour
{
    //All Active Players. Set in Runtime
    public List<PlayerMovement> players;

    // Cameras
    [SerializeField] Camera lobbyCamera;
    [SerializeField] public List<Camera> playerCameras;
    [SerializeField] public GameObject startingMap;
    [SerializeField] public List<GameObject> possibleMinigames;

    // UI
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] public List<PlayerUIHandler> sliders;
    [SerializeField] public TextMeshProUGUI timerUI;
    [SerializeField] private float gameLenghtInSeconds;

    // Other Helperfields
    [SerializeField] PlayerInputManager manager;
    [SerializeField] public List<Transform> playerLobbyPositions;
    [SerializeField] List<Transform> playerStartPositions;

    public int playerIndex;
    public bool started = false;
    public MinigameHandler minigame;
    private float counter;


    // Start is called before the first frame update
    void Awake()
    {
        playerIndex = 0;
    }
    
    //Todo: Werte für Kamera und UI Elemente sauber auslesen und implementieren

    public void PlayerJoined()
    {
        switch (playerIndex)
        {
            case 0:
                break;
            case 1:
                // Enable Camera for Player 2. Settings for Player two are preset.
                //playerCameras[1].gameObject.SetActive(true);


                // Set Camera and UI for Player 1
                playerCameras[0].rect = new Rect(0, 0.5f, 1, 1);
                sliders[0].chargeSlider.GetComponent<RectTransform>().localPosition = new Vector3(581, 364, 0);
                sliders[0].healthSlider.GetComponent<RectTransform>().localPosition = new Vector3(939, 411, 0);

                break;
            case 2:
                // Enable Camera for Player 3.
                //playerCameras[2].gameObject.SetActive(true);



                // Set Camera and UI for Player 2. Player One remains the same
                playerCameras[1].rect = new Rect(0, -0.5f, 0.5f, 1);
                sliders[1].chargeSlider.GetComponent<RectTransform>().localPosition = new Vector3(296, 49, 0);
                sliders[1].healthSlider.GetComponent<RectTransform>().localPosition = new Vector3(553, 122, 0);

                break;
            case 3:
                // Enable Camera for Player 4.
                // playerCameras[3].gameObject.SetActive(true);


                // Set Camera and UI for Player 1
                playerCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                sliders[0].chargeSlider.GetComponent<RectTransform>().localPosition = new Vector3(334, 364, 0);
                sliders[0].healthSlider.GetComponent<RectTransform>().localPosition = new Vector3(532, 457, 0);

                // Set Camera and UI for Player 2
                playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                sliders[1].chargeSlider.GetComponent<RectTransform>().localPosition = new Vector3(828, 364, 0);
                sliders[1].healthSlider.GetComponent<RectTransform>().localPosition = new Vector3(1022, 457, 0);

                // Set Camera and UI for Player 3
                playerCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                sliders[2].chargeSlider.GetComponent<RectTransform>().localPosition = new Vector3(334, 50, 0);
                sliders[2].healthSlider.GetComponent<RectTransform>().localPosition = new Vector3(532, 140, 0);
                break;
        }

    }

    public void ShowPlayerStats()
    {
        foreach(PlayerMovement p in players)
        {
            p.GetComponent<PlayerStatHandler>().ShowStats();
        }
    }

    public GameObject SelectMiniGame()
    {
        return possibleMinigames[Random.Range(0, possibleMinigames.Count)];
    }


    //Todo: Alles im GameOver

    public void GameOver(List<string> results)
    {
        gameOverUI.SetActive(true);

        foreach(string s in results)
        {
            Debug.Log(s);
        }
    }
    
    public void StartMinigame(GameObject mg)
    {
        GameObject mgBuffer = Instantiate(mg, transform);
        minigame = mgBuffer.GetComponent<MinigameHandler>();
        startingMap.SetActive(false);

        // Removes Upgrades from Scene

        Upgrade[] upgrades;
        upgrades = FindObjectsOfType<Upgrade>();

        foreach(Upgrade ug in upgrades)
        {
            ug.DespawnUpgrade();
        }

        // Removes Cars from Scene
        CarScript[] cars;
        cars = FindObjectsOfType<CarScript>();

        foreach (CarScript car in cars)
        {
            if(car.currentDriver == null)
            {
                Destroy(car.gameObject);
            }
        }

        // Removes Events from Scene
        EventDestructor[] events;

        events = FindObjectsOfType<EventDestructor>();

        foreach (EventDestructor e in events)
        {
            Destroy(e.gameObject);
        }

        // Removes LootBoxes from Scene
        LootboxHandler[] loots;

        loots = FindObjectsOfType<LootboxHandler>();

        foreach (LootboxHandler loot in loots)
        {
            Destroy(loot.gameObject);
        }

    }

    public void StartGame()
    {
        if(players.Count > 0)
        {
            GetComponent<PlayerInputManager>().enabled = false;
            started = true;
            playerUI.SetActive(true);
            lobbyUI.SetActive(false);
            lobbyCamera.gameObject.SetActive(false);
            int _helper = 0;
            foreach (PlayerMovement player in players)
            {
                player.GetComponent<Rigidbody>().useGravity = true;
                player.enabled = true;
                player.playerMerio.gameObject.SetActive(true);
                player.playerCam.gameObject.SetActive(true);
                sliders[_helper].getAssigned(player);
                player.transform.position = playerStartPositions[_helper].position;
                _helper++;

            }
        }
        else
        {
            Debug.Log("Keine Spieler Gefunden");
        }


    }

    private void FixedUpdate()
    {
        if (!minigame && started)
        {
            counter += Time.deltaTime;

            double mainGameTimerd = (double)counter;
            System.TimeSpan time = System.TimeSpan.FromSeconds(mainGameTimerd);
            string displayTime = time.ToString("mm':'ss");
            timerUI.text = displayTime;


            // Checks if MiniGame Should Start

            if (counter > gameLenghtInSeconds)
            {
                StartMinigame(SelectMiniGame());
            }
        }
        else if(minigame && started)
        {
            double mainGameTimerd = (double)minigame.timer;
            System.TimeSpan time = System.TimeSpan.FromSeconds(mainGameTimerd);
            string displayTime = time.ToString("mm':'ss");
            timerUI.text = displayTime;
        }

    }

}
