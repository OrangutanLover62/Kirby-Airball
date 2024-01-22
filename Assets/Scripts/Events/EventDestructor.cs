using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDestructor : MonoBehaviour
{

    [SerializeField] float eventDuration;
    [SerializeField] List<Upgrade> upgradesToSpawn;

    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(eventDuration < timer)
        {
            EventOver();
            
        }
    }

    public void EventOver()
    {
        //Spawns Upgrades

        foreach (Upgrade upgrade in upgradesToSpawn)
        {
            GameObject c_upgrade = Instantiate(upgrade.gameObject, transform.position, transform.rotation);
            c_upgrade.GetComponent<Upgrade>().LoseUpgrade(transform.position);
        }

        Destroy(this.gameObject);
    }

    public void EventOver(Vector3 pos)
    {
        //Spawns Upgrades

        foreach (Upgrade upgrade in upgradesToSpawn)
        {
            GameObject c_upgrade = Instantiate(upgrade.gameObject, pos, transform.rotation);
            c_upgrade.GetComponent<Upgrade>().LoseUpgrade(pos);
        }

        Destroy(this.gameObject);
    }
}
