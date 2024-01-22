using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float fallSpeed;
    public float glide;
    public float currentRotationSpeed;
    public float baseRotationSpeed;
    // Charge
    public float maxBoost;
    public float chargeRate;
    // AttackPower
    public float attackPower;

    public void setStats(
        float speed,
        float maxSpeed,
        float fallSpeed,
        float glide,
        float currentRotationSpeed,
        float baseRotationSpeed,
        float maxBoost,
        float chargeRate,
        float attackPower
    ) {
        this.speed = speed;
        this.maxSpeed = maxSpeed;
        this.fallSpeed = fallSpeed;
        this.glide = glide;
        this.currentRotationSpeed = currentRotationSpeed;
        this.baseRotationSpeed = baseRotationSpeed;
        this.maxBoost = maxBoost;
        this.chargeRate = chargeRate;
        this.attackPower = attackPower;
    }
}
