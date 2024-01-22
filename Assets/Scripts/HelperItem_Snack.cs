using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperItem_Snack : MonoBehaviour
{


    HealthHandler health;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Marble")
        {
            if (other.gameObject.GetComponent<PlayerMovement>().HasCar())
            {
                health = other.gameObject.GetComponent<PlayerMovement>().car.GetComponent<HealthHandler>();
            }
            else
            {
                health = other.gameObject.GetComponent<HealthHandler>();
            }
            

            health.currentHealth += 100;
            if(health.currentHealth > health.maxHealth)
            {
                health.currentHealth = health.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
