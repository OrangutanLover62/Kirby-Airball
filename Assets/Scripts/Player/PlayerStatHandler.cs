using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public List<Upgrade> upgrades;
    BaseStats playerStats;
    Dictionary<string, int> collectedUpgrades;

    // Initialize Basic Variables and Basestats
    private void Awake()
    {
        playerStats = this.gameObject.AddComponent<BaseStats>();
        playerStats.setStats(
            700f,
            150f,
            2000f,
            15f,
            2f,
            2f,
            100f,
            150f,
            100
        );
        player.stats = playerStats;

        collectedUpgrades = new Dictionary<string, int>();

        collectedUpgrades.Add("Speed", 0);
        collectedUpgrades.Add("Glide", 0);
        collectedUpgrades.Add("ChargeRate", 0);
        collectedUpgrades.Add("MaxBoost", 0);
        collectedUpgrades.Add("Attack", 0);
        collectedUpgrades.Add("MaxHealth", 0);
    }

    public void ShowStats()
    {
        Debug.Log(player.name);
        Debug.Log("Speed: " + collectedUpgrades["Speed"]);
        Debug.Log("Glide: " + collectedUpgrades["Glide"]);
        Debug.Log("ChargeRate: " + collectedUpgrades["ChargeRate"]);
        Debug.Log("MaxBoost: " + collectedUpgrades["MaxBoost"]);
        Debug.Log("Attack: " + collectedUpgrades["Attack"]);
        Debug.Log("MaxHealth: " + collectedUpgrades["MaxHealth"]);
    }

    public void DropUpgrades(int amount)
    {
        // Are any Upgrades Collected? 
        for (int i = 0; i < amount; i++) {
            if (upgrades.Count < 3) {
                return;
            }
            // Lose Random Upgrade
            upgrades[Random.Range(0, upgrades.Count - 1)].LoseUpgrade(transform.position, this);
        }
    }

    // Upgrade Funktionen
    public void IncreaseSpeed()
    {
        collectedUpgrades["Speed"] += 1;
        playerStats.speed += 25;
        playerStats.maxSpeed += 10;
    }

    public void DecreaseSpeed()
    {
        collectedUpgrades["Speed"] -= 1;
        playerStats.speed -= 25;
        playerStats.maxSpeed -= 10;
    }

    public void IncreaseFallSpeed()
    {
        playerStats.fallSpeed += 250;
    }

    public void DecreaseFallSpeed()
    {
        playerStats.fallSpeed -= 250;
    }

    public void IncreaseGlide()
    {
        collectedUpgrades["Glide"] += 1;
        playerStats.glide += 5;
    }

    public void DecreaseGlide()
    {
        collectedUpgrades["Glide"] -= 1;
        playerStats.glide -= 5;
    }

    public void IncreaseTurn()
    {
        playerStats.baseRotationSpeed += 1;
    }

    public void DecreaseTurn()
    {
        playerStats.baseRotationSpeed -= 1;
    }

    public void IncreaseCharge()
    {
        collectedUpgrades["ChargeRate"] += 1;
        playerStats.chargeRate += 25;
    }

    public void DecreaseCharge()
    {
        collectedUpgrades["ChargeRate"] -= 1;
        playerStats.chargeRate -= 25;
    }

    public void IncreaseMaxBoost()
    {
        collectedUpgrades["MaxBoost"] += 1;
        playerStats.maxBoost += 25;
    }

    public void DecreaseMaxBoost()
    {
        collectedUpgrades["MaxBoost"] -= 1;
        playerStats.maxBoost -= 25;
    }

    public void IncreaseAttackPower()
    {
        collectedUpgrades["Attack"] += 1;
        playerStats.attackPower += 20;
    }

    public void DecreaseAttackPower()
    {
        collectedUpgrades["Attack"] -= 1;
        playerStats.attackPower -= 20;
    }

    public void IncreaseMaxHealth()
    {
        collectedUpgrades["MaxHealth"] += 1;
        GetComponent<HealthHandler>().maxHealth += 50;
        GetComponent<HealthHandler>().currentHealth += 25;
    }

    public void DecreaseMaxHealth()
    {
        collectedUpgrades["MaxHealth"] -= 1;
        GetComponent<HealthHandler>().maxHealth -= 50;
    }

}
