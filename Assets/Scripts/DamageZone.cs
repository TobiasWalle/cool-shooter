using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageZone : MonoBehaviour {
    public HealthControl healthControl;
    public float multiplier = 1;

    /// <summary>
    /// Trigger a hit on the damage zone
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damager"></param>
    public void Hit(int damage, GameObject damager)
    {
        damage = (int) (damage * multiplier);
        healthControl.TakeDamage(damage, damager);
    }
}
