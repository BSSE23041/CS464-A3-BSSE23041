using UnityEngine;
public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.SetCheckpoint(transform);
    }
}
