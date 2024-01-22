using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHitbox : MonoBehaviour
{
    [SerializeField] Upgrade upgrade;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Marble")
        {
            upgrade.GetCollected(collision.gameObject.GetComponent<PlayerStatHandler>());
        }

    }
}