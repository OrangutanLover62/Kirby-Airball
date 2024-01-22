using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merio : MonoBehaviour
{

    [SerializeField] Animator ani;
    [SerializeField] Transform merio;
    [SerializeField] Attack_Knee kneeHandler;
    public PlayerMovement player;
    private bool flying;
    private bool breaking;
    private bool attacking;
    private Component[] bodyPartColliders;
    private Component[] bodyPartRigidbodys;
    private float ragdollTimer = 0f;
    private bool playerHit;
    GameController gc;



    private void Awake()
    {
        bodyPartColliders = GetComponentsInChildren<Collider>();
        bodyPartRigidbodys = GetComponentsInChildren<Rigidbody>();
        gc = FindObjectOfType<GameController>();
    }

    public void GetAssigned(PlayerMovement player)
    {
        this.player = player;
        this.player.playerMerio = this;
    }

    public void SpawnKneeEffect()
    {
        kneeHandler.SpawnKneeHitbox();
    }


    private void Update()
    {
        if (player)
        {
            GetParams();
            SetParams();
            gameObject.transform.position = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y + 1.5f, player.gameObject.transform.position.z);
            if (!playerHit)
            {
                this.transform.rotation = Quaternion.Euler(0, player.playerCam.transform.rotation.eulerAngles.y, 0);
            }  
        }

        if (playerHit)
        {
            ragdollTimer += Time.deltaTime;
            if(ragdollTimer > 5)
            {
                RagDollOff();
            }
        }
    }


    public void RagDollOn(Vector3 speed)
    {
        playerHit = true;
        ani.enabled = false;

        player.GetComponent<MeshRenderer>().enabled = false;
        player.cam.LookAt = merio;
        player.cam.Follow = merio;
        merio.GetComponent<Rigidbody>().velocity = speed;

        foreach (Rigidbody part in bodyPartRigidbodys)
        {
            part.isKinematic = false;
            part.useGravity = true;
            part.velocity = speed;
        }

        foreach (Collider part in bodyPartColliders)
        {
            part.enabled = true;
        }

        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().enabled = false;
        player.breaking = false;
        player.chargedBoost = 0;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void RagDollOff()
    {
        if (gc.minigame)
        {
            if (gc.minigame.gameObject.tag == "DeathMatch")
            {
                gc.minigame.PlayerScores(player.name);
                return;
            }
        }

        player.GetComponent<PlayerMovement>().enabled = true;
        ragdollTimer = 0f;
        playerHit = false;
        ani.enabled = true;
        player.GetComponent<MeshRenderer>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Collider>().enabled = true;
        player.cam.LookAt = player.transform;
        player.cam.Follow = player.transform;
        foreach (Collider part in bodyPartColliders)
        {
            part.enabled = false;
        }
        foreach (Rigidbody part in bodyPartRigidbodys)
        {
            part.isKinematic = true;
            part.useGravity = false;
        }
        player.transform.position = merio.position;
    }

    void GetParams()
    {
        flying = !player.grounded;
        breaking = player.breaking;
        attacking = player.attacking;

    }

    void SetParams()
    {
        ani.SetBool("isFlying", flying);
        ani.SetBool("isBreaking", breaking);
        ani.SetBool("isAttacking", attacking);
    }
}
