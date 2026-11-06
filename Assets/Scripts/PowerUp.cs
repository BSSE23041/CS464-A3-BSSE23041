using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float multiplier = 1.6f;
    [SerializeField] float duration = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out PlayerController player))
            player.BoostSpeed(multiplier, duration);
        Destroy(gameObject);
    }
}
