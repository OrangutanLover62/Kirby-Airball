using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackUpgrade : MonoBehaviour
{


    [SerializeField] Abstract_SpecialAttack specialAttack;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Marble")
        {
            other.gameObject.GetComponent<PlayerMovement>().specialAttack =  specialAttack;
            other.gameObject.GetComponent<PlayerMovement>().specialAttackUses = 3;

            Destroy(gameObject);
        }
    }

}
