using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    public override void GetCollected(PlayerStatHandler player)
    {
        player.upgrades.Add(this);
        transform.position = new Vector3(10000, 10000, 10000);
        player.IncreaseSpeed();
    }

    public override void DespawnUpgrade()
    {
        Destroy(gameObject);
    }

    public override void LoseUpgrade(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y + 10, position.z);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().AddForce(Random.Range(-1000, 1000), 1500, Random.Range(-1000, 1000));
    }

    public override void LoseUpgrade(Vector3 position, PlayerStatHandler player)
    {

        // Stops from Losing after Minigame has started
        if (FindObjectOfType<GameController>().minigame)
            return;

        player.upgrades.Remove(this);
        player.DecreaseSpeed();
        transform.position = new Vector3(position.x, position.y + 10, position.z);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().AddForce(Random.Range(-1000, 1000), 1500, Random.Range(-1000, 1000));
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
