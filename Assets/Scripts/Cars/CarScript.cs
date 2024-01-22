using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] GameObject captureSphere;
    [SerializeField] BaseStats carStats;
    [SerializeField] float carOffsetY;
    [SerializeField] public Abstract_SpecialAttack carAttack;

    public PlayerMovement currentDriver;
    public float desiredXrotation;
    private float tiltTimer;
    private float invisibleTimer = 5;
    private float tiltDelay = 1f;

    private Vector3 c_position;

    public BaseStats getCarStats() {
        return carStats;
    }

    // Start is called before the first frame update
    void Awake()
    {
        currentDriver = null;
        desiredXrotation = 0;
    }

    // Fahrzeug folgt der Murmel
    private void Update()
    {
        if (currentDriver != null)
        {
            c_position = currentDriver.transform.position;
            c_position.y = c_position.y + carOffsetY;
            this.transform.position = c_position;
        }
        // Dreht Fahrzeug langsam gerade
        tiltTimer += Time.deltaTime;
        if (tiltTimer >= tiltDelay)
        {
            desiredXrotation = desiredXrotation * 0.999f;
        }
        if (invisibleTimer >= 1f && invisibleTimer < 3f) {
            invisibleTimer = 3;
            captureSphere.SetActive(true);
        } else if (invisibleTimer < 1f) {
            invisibleTimer += Time.deltaTime;
        }
    }

    // Rotiert Fahrzeug mit Kamera und kippt falls in der Luft
    private void FixedUpdate()
    {
        if(currentDriver != null)
        {
            this.transform.rotation = Quaternion.Euler( 0, currentDriver.playerCam.transform.rotation.eulerAngles.y, 0 );

            if (!currentDriver.grounded)
            {
                transform.rotation = Quaternion.Euler(desiredXrotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }
    }

    public void GetTaken(PlayerMovement newDriver)
    {
        currentDriver = newDriver;
        captureSphere.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        MeshCollider m_Collider = GetComponent<MeshCollider>();
        m_Collider.enabled = !m_Collider.enabled;
    }

    public void Dismount()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.AddForce(
            0,
            5000,
            1000
        );
        rigidBody.velocity = currentDriver.GetComponent<Rigidbody>().velocity;
        currentDriver.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        currentDriver = null;
        MeshCollider m_Collider = GetComponent<MeshCollider>();
        m_Collider.enabled = !m_Collider.enabled;
        invisibleTimer = 0;
    }
}
