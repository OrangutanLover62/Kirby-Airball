using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] public Slider chargeSlider;
    [SerializeField] public Slider healthSlider;
    public PlayerMovement player;


    private void Awake()
    {
        chargeSlider.minValue = 1;
        healthSlider.minValue = 0;
        player = null;
    }

    public void getAssigned(PlayerMovement p)
    {
        player = p;
    }

    // 
    void Update()
    {
        // Prüft ob Spieler vorhanden ist (fängt Nullpointer bei initialisierung ab). 
        // Überprüft welches UI angezeigt werden muss

        if (player)

        {
            if (player.HasCar())
            {
                ShowCarUI();
            }
            else
            {
                ShowMarbleUI();
            }
            
        }  
    }



    void ShowCarUI()
    {

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = player.car.GetComponent<HealthHandler>().maxHealth;
        healthSlider.value = player.car.GetComponent<HealthHandler>().currentHealth;

        if (player.isBreaking() && player.grounded)
        {
            chargeSlider.gameObject.SetActive(true);
            chargeSlider.maxValue = player.stats.maxBoost + player.car.getCarStats().maxBoost;
            chargeSlider.value = player.chargedBoost;
        }
        else if (!player.grounded && player.glideTime < player.stats.maxBoost + player.car.getCarStats().maxBoost)
        {
            chargeSlider.gameObject.SetActive(true);
            chargeSlider.maxValue = player.stats.maxBoost + player.car.getCarStats().maxBoost;
            chargeSlider.value = chargeSlider.maxValue - player.glideTime;
        }
        else
        {
            chargeSlider.gameObject.SetActive(false);
        }
    }

    void ShowMarbleUI()
    {

        // Deaktiviert den Lebensbalken, falls der Spieler das Fahrzeug verloren hat
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = player.GetComponent<HealthHandler>().maxHealth;
        healthSlider.value = player.GetComponent<HealthHandler>().currentHealth;

        if (player.isBreaking())
        {
            chargeSlider.gameObject.SetActive(true);
            chargeSlider.maxValue = player.stats.maxBoost;
            chargeSlider.value = player.chargedBoost;
        }
        else
        {
            chargeSlider.gameObject.SetActive(false);
        }
    }
}
