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
        
        //to del
        animator.SetTrigger("isDamaged");
    }

    void Death() {
        Debug.Log(gameObject.name + " IS DEAD");
    }

    void DecreaseHealthIndicator(int decrHealth) {
        if (health >= 0) {
        //Debug.Log(gameObject.name + " HEALTH=" + health);
        float scaleDecrease = startIndicatorScale * decrHealth / startHealth;
        healthIndicator.transform.localScale -= new Vector3(scaleDecrease, 0, 0);
        }
    }
}
