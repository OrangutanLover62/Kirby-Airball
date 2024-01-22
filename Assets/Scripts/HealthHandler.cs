using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool decreaseHealth(float damage)
    {
        currentHealth -= damage;
        Debug.Log("health: " + currentHealth);
        return currentHealth <= 0;
    }

    public void increaseHealth(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
