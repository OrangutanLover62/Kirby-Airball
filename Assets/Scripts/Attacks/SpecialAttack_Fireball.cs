using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack_Fireball : Abstract_SpecialAttack
{
    [SerializeField] float neededYoffset;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject fireBall;
    public PlayerMovement player;
    bool triggered = false;
    float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void SetPlayer(PlayerMovement player)
    {
        this.player = player;
        this.transform.rotation = Quaternion.Euler(0, player.playerCam.transform.rotation.eulerAngles.y, 0);
        ShootFireBall();

    }

    private void ShootFireBall()
    {
        Vector3 pushDirection = (player.transform.position - player.cam.transform.position).normalized;
        pushDirection.y = 0;
        rb.AddForce(pushDirection * 10000);

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Marble" || other.gameObject.tag == "Lootbox" || other.gameObject.tag == "Event" || other.gameObject.tag == "Car")
        {
            Explode();
        }
    }

    void Explode()
    {

        // Adds Explosionforce and Damage

        Collider[] colliders = Physics.OverlapSphere(transform.position, 30);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(5000, transform.position, 30);
                DamageHandler damageHandler = nearbyObject.GetComponent<DamageHandler>();
                if (damageHandler)
                {
                    float distance = Vector3.Distance(transform.position, nearbyObject.gameObject.transform.position);
                    if (500 - (distance * distance) > 0)
                    {
                        damageHandler.calculateDamage(500 - (distance * distance), player.stats.attackPower);
                    }
                }
                
            }
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        fireBall.SetActive(false);
        Instantiate(explosion, transform);
        triggered = true;
        timer += Time.deltaTime;

    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Destroy(gameObject);
        }
        if (triggered)
        {
            if (timer > 3)
            {
                Destroy(gameObject);
            }
        }
    }

    public override float GetYOffset()
    {
        return neededYoffset;
    }
}
