using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public PlayerMovement player;

    private void OnTriggerEnter(Collider other)
    {
        if (player.Equals(other.gameObject.GetComponent<PlayerMovement>()))
        {
            return;
        }
        if (other.tag == "Marble" || other.tag == "Lootbox" || other.tag == "Car")
        {
            other.gameObject.GetComponent<DamageHandler>().calculateDamage(player.GetComponent<Rigidbody>().velocity.magnitude, player.GetAttackStat());
            Vector3 direction = other.gameObject.transform.position - player.transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity.magnitude * direction.normalized;
        }
    }
}
