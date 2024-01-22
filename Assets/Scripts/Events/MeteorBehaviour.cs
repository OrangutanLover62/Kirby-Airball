using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{

    Rigidbody meteor;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionPower;
    [SerializeField] float explosionRadius;
    bool exploded = false;
    float destructionTimer = 0f;
    GameObject meteorExplosion;



    private void Awake()
    {
        meteor = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        meteor.AddForce(0, -5000, 0);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Marble")
        {
            Explode();
        }
    }
    
    void Explode()
    {

        // Deaktiviert Collider und löscht entsprechende Gameobjekte
        exploded = true;
        meteorExplosion = Instantiate(explosion, transform);
        meteorExplosion.transform.localScale = new Vector3(explosionRadius / 2, explosionRadius / 2, explosionRadius / 2);
        meteor.detectCollisions = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(transform.GetChild(0).gameObject);


        // Adds Explosionforce and Damage

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(explosionPower, transform.position, explosionRadius);
                DamageHandler damageHandler = nearbyObject.GetComponent<DamageHandler>();
                if (damageHandler)
                {
                    float distance = Vector3.Distance(transform.position, nearbyObject.gameObject.transform.position);
                    if(explosionPower - (distance * distance) > 0)
                    {
                        damageHandler.calculateDamage(explosionPower - (distance * distance), 1);
                    }
                }

            }
        }
    }

    private void FixedUpdate()
    {
        if (exploded)
        {
            destructionTimer += Time.deltaTime;
            if(destructionTimer > 3)
            {
                Destroy(meteorExplosion);
                Destroy(this.gameObject);
            }
        }
    }

}
