using UnityEngine;
public class BouncePad : MonoBehaviour
{
    [SerializeField] float launchSpeed = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;

        Vector3 v = body.linearVelocity;
        body.linearVelocity = new Vector3(v.x, 0f, v.z);
        body.AddForce(Vector3.up * launchSpeed, ForceMode.VelocityChange);
    }
}
