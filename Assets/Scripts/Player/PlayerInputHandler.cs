using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] List<PlayerMovement> playerPrefabs;
    [SerializeField] Merio playerMerio;
    PlayerMovement player;
    GameController gc;

    private void Start()
    {
        // Stellt Variablen her und weist dem BoostBalken den Spieler zu
        gc = FindObjectOfType<GameController>();
        player = Instantiate(playerPrefabs[gc.playerIndex], gc.playerLobbyPositions[gc.playerIndex].position, transform.rotation);
        playerMerio = Instantiate(playerMerio, transform.position, transform.rotation);
        playerMerio.GetAssigned(player);
        player.inputHandler = this;
        gc.sliders[gc.playerIndex].getAssigned(player);
        player.playerCam = gc.playerCameras[gc.playerIndex];
        gc.playerIndex++;
        gc.players.Add(player);

    }

    public void Dismount(InputAction.CallbackContext context)
    {
        if (player)
        {
            if (context.performed)
            {
                if (player.car)
                {
                    player.DismountCar();
                }
            }
        }
    }

    public void Boost(InputAction.CallbackContext context)
    {
        if (player)
        {
            if (context.canceled)
            { 
                player.Boost();
            }
        }
    }

    public void BreakingTrue(InputAction.CallbackContext context)
    {
        if (player)
        {
            if (context.performed)
            {
                player.BreakingTrue();
            }
        }
    }

    public void RotatePlayer(InputAction.CallbackContext context)
    {
        if (player)
        {
            player.RotatePlayer(context.ReadValue<Vector2>().normalized);
            if (context.canceled)
            {
                player.direction = new Vector2(0,0);
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (player)
        {
            if(context.performed)
                player.Attack();

            if (context.canceled)
                player.StopAttack();
        }
    }

    public void TiltCar(InputAction.CallbackContext context)
    {
        if (player)
        {
            player.TiltCar(context.ReadValue<Vector2>());
        }
    }
}
