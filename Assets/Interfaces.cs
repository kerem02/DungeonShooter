using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

public interface IDamageble
{
    public float health { get; set; }
    public bool isKnocking { get; set; }
    public float knockbackDuration { get; set; }
    public SimpleFlash flashEffect { get; set; }

    public void takeDamage(float damage,Vector2 direction, float knockbackForce);

    public IEnumerator Knockback(Vector2 direction, float knockbackForce);



}