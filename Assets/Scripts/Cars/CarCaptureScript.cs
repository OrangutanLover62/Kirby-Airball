using UnityEngine;

public class CarCaptureScript : MonoBehaviour
{
    [SerializeField] CarScript car;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Marble")
        {
            collider.GetComponent<PlayerMovement>().TakeCar(car);
        }
    }
}
