using System.Collections.Generic;
using UnityEngine;

public class LootboxHandler : MonoBehaviour
{
    [SerializeField] List<Upgrade> upgrades;
    [SerializeField] int maxDrop;
    List<Upgrade> loot;

    void Awake()
    {
        int counter = Random.Range(1, maxDrop);
        loot = new List<Upgrade>();
        for (int i = 0; i < counter; i++) {
            loot.Add(upgrades[Random.Range(0, upgrades.Count)]);
        }
    }

    public void onDestroy()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Destroy(gameObject);
        foreach(Upgrade upgrade in loot)
        {
            Vector3 spawnPosition = position;
            spawnPosition.x = spawnPosition.x - Random.Range(-5, 5);
            spawnPosition.z = spawnPosition.z - Random.Range(-5, 5);
            Instantiate(upgrade, spawnPosition, rotation);
            upgrade.GetComponent<Rigidbody>().AddForce(
                Random.Range(-3000, 3000), 
                Random.Range(1500, 3000), 
                Random.Range(-3000, 3000)
            );
        }
    }
}
