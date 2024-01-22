using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public PlayerMovement player;
    [SerializeField] List<AttackHitbox> hitboxes;
    private float timer;

    public void SetupHitboxes(PlayerMovement p)
    {
        player = p;
        foreach (AttackHitbox box in hitboxes)
        {
            box.player = player;
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1f)
        {
            Destroy(this.gameObject);
        }
        if (player)
        {
            transform.position = player.gameObject.transform.position;

            if (player.car)
            {
                transform.rotation = player.car.gameObject.transform.rotation;
            }
        }
    }
}
