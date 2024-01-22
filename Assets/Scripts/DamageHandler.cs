using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Rigidbody rb;
    HealthHandler healthHandler;
    [SerializeField] TypeEnum type;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthHandler = GetComponent<HealthHandler>();
        Debug.Log("init: " + type);
    }

    public void ChangeTypeAndHealth(TypeEnum type, HealthHandler healthHandler)
    {
        this.type = type;
        this.healthHandler = healthHandler;
    }

    TypeEnum getTypeFromTag(string tag)
    {
        if (tag == "Marble") {
            return TypeEnum.PLAYER;
        }
        if (tag == "Car") {
            return TypeEnum.CAR;
        }
        return TypeEnum.BOX;
    }

    // Todo: Unsaubere Lösung für Ragdoll. Viel kopierter Code durch überladungen

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Marble" && !collision.gameObject.GetComponent<PlayerMovement>().isBreaking()) {
            Vector3 aggressorVector = collision.gameObject.GetComponent<Rigidbody>().velocity;
            Debug.Log(aggressorVector);
            calculateDamage(aggressorVector, aggressorVector.magnitude, collision.gameObject.GetComponent<PlayerMovement>().GetAttackStat());
        }
    }

    public void calculateDamage(float aggressorSpeed, float attackPower)
    {
        // ToDo: Damage logic not perfect
        // ToDo: Use maxSpeed instead of 100 for current calculation
        float damage = aggressorSpeed / 100 * attackPower;
        Debug.Log("damage: " + damage);
        if (healthHandler.decreaseHealth(damage))
        {
            deathActionByType();
        } 
    }

    // Überschriebene Methode für Ragdolls

    public void calculateDamage(Vector3 impactVel, float aggressorSpeed, float attackPower)
    {
        // ToDo: Damage logic not perfect
        // ToDo: Use maxSpeed instead of 100 for current calculation
        float damage = aggressorSpeed / 100 * attackPower;
        Debug.Log("damage: " + damage);
        if (healthHandler.decreaseHealth(damage))
        {
            deathActionByType(impactVel);
        }
    }

    void deathActionByType()
    {
        Debug.Log("death: " + type);
        if (type == TypeEnum.PLAYER)
        {
            GetComponent<PlayerStatHandler>().DropUpgrades(10);
            GetComponent<PlayerMovement>().playerMerio.RagDollOn(rb.velocity);
            healthHandler.currentHealth = healthHandler.maxHealth;
        }
        else if (type == TypeEnum.CAR)
        {
            if (GetComponent<PlayerMovement>())
            {
                GetComponent<PlayerStatHandler>().DropUpgrades(5);
                GetComponent<PlayerMovement>().playerMerio.RagDollOn(rb.velocity);
                GetComponent<PlayerMovement>().DestroyCar();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (type == TypeEnum.BOX)
        {
            GetComponent<LootboxHandler>().onDestroy();
        }
        else if (type == TypeEnum.EVENT)
        {
            GetComponentInParent<EventDestructor>().EventOver(transform.position);
        }
    }



    void deathActionByType(Vector3 impactVel)
    {
        Debug.Log("death: " + type);
        if (type == TypeEnum.PLAYER)
        {
            GetComponent<PlayerStatHandler>().DropUpgrades(1);
            GetComponent<PlayerMovement>().playerMerio.RagDollOn(impactVel);
            healthHandler.currentHealth = healthHandler.maxHealth;
        }
        else if (type == TypeEnum.CAR)
        {
            if (GetComponent<PlayerMovement>())
            {
                GetComponent<PlayerStatHandler>().DropUpgrades(5);
                GetComponent<PlayerMovement>().playerMerio.RagDollOn(impactVel);
                GetComponent<PlayerMovement>().DestroyCar();

            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (type == TypeEnum.BOX)
        {
            GetComponent<LootboxHandler>().onDestroy();
        }
        else if (type == TypeEnum.EVENT)
        {
            GetComponentInParent<EventDestructor>().EventOver(transform.position);
        }
    }
}
