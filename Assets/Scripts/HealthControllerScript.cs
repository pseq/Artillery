using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerScript : MonoBehaviour
{
    public int startHealth = 100;
    float startIndicatorScale;
    public int health;
    public Animator animator;
    public GameObject healthIndicator;
    public GameObject weakestBullet;
    

    void Start() {
        health = startHealth;
        startIndicatorScale = healthIndicator.transform.localScale.x;
    }
    public void HealthDecrease(int decrease) {
        health -= decrease;
        DecreaseHealthIndicator(decrease);
        
        if(health <= 0) {
            Death();
            return;
        }
    }

    public void Shooted() {
        if(GetComponent<TankAIScript>()) GetComponent<TankAIScript>().Shooted();
    }

    void Death() {
        Debug.Log(gameObject.name + " IS DEAD");
    }

    void DecreaseHealthIndicator(int decrHealth) {
        if (health >= 0) {
        //Debug.Log(gameObject.name + " HEALTH=" + health);
        // если здоровье меньше минимального выстрела - ставим флажок ИИ
        GetComponent<TankAIScript>().CritHealth(health <= weakestBullet.GetComponent<BulletScript>().damage);
        float scaleDecrease = startIndicatorScale * decrHealth / startHealth;
        healthIndicator.transform.localScale -= new Vector3(scaleDecrease, 0, 0);
        }
    }
}
