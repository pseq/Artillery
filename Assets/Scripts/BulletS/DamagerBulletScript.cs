using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerBulletScript : MonoBehaviour
{

    Damagable[] damagables;
    public int damage;
    Collider2D collider;
    DamageControllerScript damageControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();        
        damageControllerScript = FindObjectOfType<DamageControllerScript>();
        damagables = damageControllerScript.GetDamagables();
    }

    public void MakeDamage() {
        Debug.Log("MakeDamage damagables: " + damagables.Length);
        foreach(Damagable dmg in damagables) {
            if (collider.IsTouching(dmg.GetCollider())) {
                HealthControllerScript healthScript = dmg.GetHealth();
                healthScript.HealthDecrease(damage);
                healthScript.Shooted();
            }
        }
    }
}
