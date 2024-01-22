using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Knee : MonoBehaviour
{

    [SerializeField] Attack_KneeEffect attackEffect;
    [SerializeField] Merio playerMerio;

    public void SpawnKneeHitbox()
    {
        Attack_KneeEffect knee = Instantiate(attackEffect);
        knee.setParameters(transform, playerMerio.player);
    }

}
