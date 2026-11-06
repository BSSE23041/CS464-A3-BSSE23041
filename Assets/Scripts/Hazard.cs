using System.Collections;
using UnityEngine;

//   Spikes  
//   Guard   
//   Crusher 
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 6f;     // Impulse: 6 m/s for a 1 kg player
    [SerializeField] float knockTime = 0.25f;  // seconds the player can't steer

    [Header("Timed hazard (crusher). Leave restTime at 0 for an always-on hazard.")]
    [SerializeField] float restTime = 0f;      // collider OFF, safe
    [SerializeField] float warnTime = 0.7f;    // warning ON, still safe (>= 0.5 s or it's unfair)
    [SerializeField] float strikeTime = 0.4f;  // loses life
    [SerializeField] float startOffset = 0f;   // wait this long first, so two crushers can alternate
    [SerializeField] GameObject warning;       // child that lights up during warnTime
    [SerializeField] GameObject slab;          // child shown during the strike

    Collider zone;

    void Awake() => zone = GetComponent<Collider>();

    IEnumerator Start()
    {
        if (restTime <= 0f) yield break;       // static hazard

        if (startOffset > 0f)
        {
            zone.enabled = false;
            yield return new WaitForSeconds(startOffset);
        }

        while (true)
        {
            zone.enabled = false;
            if (slab) slab.SetActive(false);
            if (warning) warning.SetActive(false);
            yield return new WaitForSeconds(restTime);

            if (warning) warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);

            if (warning) warning.SetActive(false);
            zone.enabled = true;
            if (slab) slab.SetActive(true);
            yield return new WaitForSeconds(strikeTime);
        }
    }

    // Every physics step while overlapping. Health's i frames decide how often it lands.
    void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Health health)) return;
        if (!health.TakeDamage(damage)) return;      // no hit, no knockback

        // Away from the nearest point of the hazard, so a long row of spikes throws clear of it.
        Vector3 from = other.transform.position;
        Vector3 away = from - zone.ClosestPoint(from);
        if (away.sqrMagnitude < 0.0001f) away = from - transform.position;   
        away.y = 0f;                                  
        if (away.sqrMagnitude < 0.0001f) away = transform.right;
        Vector3 push = away.normalized * knockback;

        if (other.TryGetComponent(out PlayerController player))
            player.Knockback(push, knockTime);
        else if (other.attachedRigidbody != null)
            other.attachedRigidbody.AddForce(push, ForceMode.Impulse);
    }
}
