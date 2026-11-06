using UnityEngine;

// Health owns the hearts AND the rule about how which can be lost (i frames), so a hazard may ask every physics step.
public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;
    [Tooltip("Optional: the player's mesh. It blinks while invulnerable.")]
    [SerializeField] Renderer blinkRenderer;

    int current;
    float invulnerableUntil;

    public int Current => current;
    public int Max => max;
    public bool Invulnerable => Time.time < invulnerableUntil;

    void Awake() => current = max;

    void Update()
    {
        if (blinkRenderer == null) return;
        // blink 10x a second while protecte always visible otherwise
        blinkRenderer.enabled = !Invulnerable || ((int)(Time.time * 10f) % 2 == 0);
    }

    // true = the hit landed (the hazard uses that to decide on knockback)
    public bool TakeDamage(int amount)
    {
        if (current <= 0 || Time.time < invulnerableUntil) return false;
        invulnerableUntil = Time.time + iFrames;
        current -= amount;
        GameManager.Instance.UpdateUI();
        if (current <= 0) GameManager.Instance.PlayerDied();   // <= 0, never == 0 (a crusher hits for 2)
        return true;
    }

    // Called by the GameManager on a respawn.
    public void Restore(float protection)
    {
        current = max;
        invulnerableUntil = Time.time + protection;
        GameManager.Instance.UpdateUI();
    }
}
