using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

// Interface to define damageable entities
public interface IDamageble
{
    public float health { get; set; } // Health of the entity
    public bool isKnocking { get; set; } // Whether the entity is being knocked back
    public float knockbackDuration { get; set; } // Duration of the knockback
    public SimpleFlash flashEffect { get; set; } // Flash effect when taking damage

    // Method to take damage
    public void takeDamage(float damage, Vector2 direction, float knockbackForce);

    // Coroutine to handle knockback
    public IEnumerator Knockback(Vector2 direction, float knockbackForce);
}
