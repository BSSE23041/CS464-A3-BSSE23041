using UnityEngine;

// Lecture 07, part 3. The pickup is a trigger that reports and disappears.
// The boost AND its timer belong to the player (PlayerController.BoostSpeed):
// a coroutine started here would die with Destroy(gameObject), and the boost would never end.
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
