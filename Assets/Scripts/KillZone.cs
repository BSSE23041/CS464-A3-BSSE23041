using UnityEngine;


// Falling skips the hearts and cost a life at once.
public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.PlayerDied();
    }
}
