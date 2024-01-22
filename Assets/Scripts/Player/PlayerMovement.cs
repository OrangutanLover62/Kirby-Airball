using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public CinemachineFreeLook cam;
    [SerializeField] public AttackHandler attack;
    [SerializeField] public DamageHandler damage;
    [SerializeField] GroundChecker checker;

    public Abstract_SpecialAttack specialAttack = null;
    public int specialAttackUses = 0;

    public PlayerInputHandler inputHandler;
    public Merio playerMerio;
    public CarScript car;
    public BaseStats stats;
    public Camera playerCam;
    public Vector2 direction;
    public Vector2 directionCar;
    public bool breaking;
    public bool grounded;
    public bool attacking;

    public int life = 3;

    // Tempor�re Variablen
    public float chargedBoost = 0f;
    public float glideTime = 0f;

    private float attackTimer;
    public Rigidbody rb;

    //Cache Variablen
    public float c_currentSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        breaking = false;
        car = null;
        rb = GetComponent<Rigidbody>();
    }

    public void StopAttack()
    {
        attacking = false;
    }

    public void LoseLife()
    {
        life--;
        if(life <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetupMiniGame()
    {
        cam.m_XAxis.Value = 0;
        checker.ResetList();
        chargedBoost = 0;
        GetComponent<HealthHandler>().currentHealth = GetComponent<HealthHandler>().maxHealth;
        if (HasCar())
        {
            car.GetComponent<HealthHandler>().currentHealth = car.GetComponent<HealthHandler>().maxHealth;
        }
    }

    //Bremsen
    public bool isBreaking()
    {
        return breaking;
    }

    public void BreakingTrue()
    {
        breaking = true;
        stats.currentRotationSpeed = stats.baseRotationSpeed + 1;
    }

    public bool HasCar()
    {
        if (car)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SlowDown()
    {
        // Holt die Richtung und Geschwindigkeit der Kugel
        Vector3 currentVelocity = rb.velocity;
        // Hindert Kugel vom Steigen
        if (currentVelocity.y > 0)
        {
            currentVelocity.y = currentVelocity.y * 0.9f;
        }
        Vector3 targetVelocity = new Vector3(currentVelocity.x * 0.95f, currentVelocity.y, currentVelocity.z * 0.95f);
        rb.velocity = targetVelocity;
        if (car == null)
        {
            Charge();
        }
    }

    //Charge
    public void Charge()
    {
        // Checks if Player has a Car and adds Charge accordingly

        if (HasCar())
        {
            if (chargedBoost < stats.maxBoost + car.getCarStats().maxBoost)
            {
                chargedBoost += (stats.chargeRate + car.getCarStats().chargeRate) * Time.deltaTime;
            }
        }
        else
        {
            if (chargedBoost < stats.maxBoost)
            {
                chargedBoost += stats.chargeRate * Time.deltaTime;
            }
        }

    }

    //Boost
    public void Boost()
    {
        breaking = false;
        stats.currentRotationSpeed = stats.baseRotationSpeed;
    }

    //Rotieren
    public void RotatePlayer(Vector2 inputVec)
    {
        direction = inputVec;
    }

    public void RotateCar(Vector2 inputVec)
    {
        directionCar = inputVec;
    }

    public void Attack()
    {
        if(attackTimer > 1.5f)
        {
            attacking = true;

            if (specialAttack)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + Mathf.RoundToInt(specialAttack.GetYOffset()), transform.position.z);
                Abstract_SpecialAttack sAttack = Instantiate(specialAttack, spawnPos, Quaternion.Euler(0, playerCam.transform.rotation.eulerAngles.y, 0));
                sAttack.SetPlayer(this);
                // ToDo: Same Nullpointer problem?
                attackTimer = 0;
                specialAttackUses--;
                if(specialAttackUses == 0)
                {
                    specialAttack = null;
                }
            }
            else
            {
                if (HasCar() && car.carAttack)
                {
                    Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + Mathf.RoundToInt(car.carAttack.GetYOffset()), transform.position.z);
                    Abstract_SpecialAttack sAttack = Instantiate(car.carAttack, spawnPos, Quaternion.Euler(0, playerCam.transform.rotation.eulerAngles.y, 0));
                    sAttack.SetPlayer(this);
                    attackTimer = 0;
                }
                else
                {
                    
                    AttackHandler a = Instantiate(attack);
                    a.SetupHitboxes(this);
                    // ToDo: Same Nullpointer problem?
                    attackTimer = 0;
                    
                }

            }
        }

    }

    public float GetAttackStat()
    {
        float sum = stats.attackPower;;
        if (car)
        {
            sum += car.getCarStats().attackPower;
        }
        return sum;
    }

    public void TiltCar(Vector2 inputVector)
    {
        if(car != null)
        {
            car.desiredXrotation = inputVector[1] * 45;
        }
    }

    public void TakeCar(CarScript car)
    {
        if (this.car == null) {
            damage.ChangeTypeAndHealth(TypeEnum.CAR, car.GetComponent<HealthHandler>());
            this.car = car;
            car.GetTaken(this);
        }
    }

    public void DestroyCar()
    {
        car.Dismount();
        Destroy(car.gameObject);
        car = null;
        damage.ChangeTypeAndHealth(TypeEnum.PLAYER, GetComponent<HealthHandler>());
    }

    public void DismountCar() 
    {
        car.Dismount();
        car = null;
        damage.ChangeTypeAndHealth(TypeEnum.PLAYER, GetComponent<HealthHandler>());
    }

    // �berpr�ft ob Spieler ein Fahrzeug hat und f�hrt entsprechende Funktionen aus.
    // Falls 
    // (Beschleunigen mit und ohne Fahrzeug wird durch verschiedene Logik durchgef�hrt)
    // Dreht Kamera
    // Bremst
    // �berpr�ft ob Kugel am Boden ist
    void FixedUpdate()
    {
        // Timer werden Hochgez�hlt
        attackTimer += Time.deltaTime;

        // Spieler hat Fahrzeug
        if (car != null)
        {
            HandleCarInput();
        }
        // Kein Fahrzeug
        else
        {
            //Beschleunigt
            if (!breaking)
            {
                // Max Speed erreicht?
                if (rb.velocity.magnitude > stats.maxSpeed)
                {
                    rb.velocity = new Vector3(
                        rb.velocity.x * 0.99f,
                        rb.velocity.y,
                        rb.velocity.z * 0.99f
                    );
                }
                else
                {
                    Vector3 pushDirection = (this.transform.position - cam.transform.position).normalized;

                    rb.AddForce(
                        pushDirection.x * stats.speed * Time.deltaTime * (chargedBoost + 1),
                        0,
                        pushDirection.z * stats.speed * Time.deltaTime * (chargedBoost + 1)
                    );
                    chargedBoost = 0;
                }
            }
            // Bremsen
            if (breaking)
            {
                SlowDown();
            }
        }
        // Kamera drehen
        if(direction.magnitude >= 0.001)
        {
            // Rotate wenn daf�r Signal entdeckt wird
            transform.Rotate(0, direction[0] * stats.currentRotationSpeed, 0);
            cam.m_XAxis.Value = cam.m_XAxis.Value + direction[0] * stats.currentRotationSpeed;
            cam.m_YAxis.Value = cam.m_YAxis.Value + direction[1] * stats.currentRotationSpeed / 4 * Time.deltaTime;
        }
    }

    void HandleCarInput()
    {
        // Max Speed erreicht?
        
        if (rb.velocity.magnitude > stats.maxSpeed + car.getCarStats().maxSpeed)
        {
            rb.velocity = new Vector3(
                rb.velocity.x * 0.99f, 
                rb.velocity.y, 
                rb.velocity.z * 0.99f
            );
        }
        //Bremst nicht
        if (!breaking)
        {
            CarAccelerate();
        }
        // Spieler Bremst, Charge wird durchgef�hrt wenn Spieler nicht in der Luft ist
        else
        {
            CarSlowDown();
        }
    }

    // Pr�ft ob Kugel am Boden ist und beschleunigt entsprechend
    void CarAccelerate()
    {
        //Falls Kugel am Boden > Beschleunigt mit den CarStats
        if (grounded)
        {
            Vector3 pushDirection = (this.transform.position - cam.transform.position).normalized;
            c_currentSpeed = rb.velocity.magnitude;
            rb.AddForce(
                pushDirection.x * (stats.speed + car.getCarStats().speed) * Time.deltaTime * (chargedBoost + 1), 
                0, 
                pushDirection.z * (stats.speed + car.getCarStats().speed) * Time.deltaTime * (chargedBoost + 1)
            );
            chargedBoost = 0;
        }
        //Kugel Fliegt
        else
        {
            // Kugel steigt und GlideTime wird erh�ht, solange nicht gebremst wird
            if (glideTime < stats.maxBoost + car.getCarStats().maxBoost)
            {
                rb.AddForce(
                    0, 
                    -car.desiredXrotation * Time.deltaTime * (stats.glide + car.getCarStats().glide), 
                    0
                );
                glideTime += Time.deltaTime * 100;
            }
            // Rotiert Kamera und "Force"
            // Beschleunigt Wagen solange unter Topspeed
            Vector3 pushDirection = (this.transform.position - cam.transform.position).normalized;
            Vector3 oldVelocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            Vector3 newVelocity = oldVelocity.magnitude * pushDirection;
            c_currentSpeed = rb.velocity.magnitude;
            if (c_currentSpeed < stats.maxSpeed + car.getCarStats().maxSpeed)
            {
                rb.AddForce(pushDirection.x * Time.deltaTime, 0, pushDirection.z * Time.deltaTime);
            }
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;

        }
    }

    //Momentan selbe BremsLogik wie ohne Fahrzeug. Wenn Spieler am Boden ist ladet der Spieler Boost auf. In der wird der Spieler nach unten gedr�ckt.
    void CarSlowDown()
    {
        SlowDown();
        if (grounded)
        {
            Charge();
        }
        else
        {
            rb.AddForce(
                0, 
                -(Time.deltaTime * (stats.fallSpeed + car.getCarStats().fallSpeed)), 
                0
            );
        }

    }
}
