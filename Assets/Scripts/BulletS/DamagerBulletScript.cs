using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerBulletScript : MonoBehaviour
{

    Damagable[] damagables;
    public int damage;
    Collider2D collider;
    DamageControllerScript damageControllerScript;
    TerrainScript terrain;
	public float explDiam;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();        
        damageControllerScript = FindObjectOfType<DamageControllerScript>();
        damagables = damageControllerScript.GetDamagables();
        terrain = FindObjectOfType<TerrainScript>();
    }

    public void MakeDamage() {
        // дырка в земле
        terrain.TerrainHole(gameObject, explDiam);

        // урон объектам
        foreach(Damagable dmg in damagables) {
            if (collider.IsTouching(dmg.GetCollider())) {
                HealthControllerScript healthScript = dmg.GetHealth();
                healthScript.HealthDecrease(damage);
                healthScript.Shooted();
            }
        }
    }
}
