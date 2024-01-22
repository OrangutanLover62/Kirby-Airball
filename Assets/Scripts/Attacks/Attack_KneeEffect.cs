using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_KneeEffect : MonoBehaviour
{
    private PlayerMovement player;
    private Transform transformToTrack;
    private float hitboxTimer;

    public void setParameters(Transform t, PlayerMovement p)
    {
        transformToTrack = t;
        player = p;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.Equals(other.gameObject.GetComponent<PlayerMovement>()))
        {
            return;
        }
        if (other.tag == "Marble" || other.tag == "Lootbox" || other.tag == "Car")
        {
            other.gameObject.GetComponent<DamageHandler>().calculateDamage(player.GetComponent<Rigidbody>().velocity.magnitude * 3, player.GetAttackStat());
            Vector3 direction = other.gameObject.transform.position - player.transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity.magnitude * direction.normalized * 3;
        }
    }

    private void Update()
    {
        if (player)
        {
            transform.position = transformToTrack.position;

            hitboxTimer += Time.deltaTime;

            if(hitboxTimer > 0.3)
            {
                Destroy(gameObject);
            }
        }
    }
}
