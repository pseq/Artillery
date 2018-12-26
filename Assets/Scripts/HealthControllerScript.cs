using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerScript : MonoBehaviour
{
    public int health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void HealthDecrease(int decrease) {
        health -= decrease;
        
        if(health < 0) {
            SetHealthIndicator(0);
            Death();
            return;
        }

        SetHealthIndicator(health);
    }

    void Death() {
        Debug.Log(gameObject.name + " IS DEAD");
    }

    void SetHealthIndicator(int indicHealth) {
        Debug.Log(gameObject.name + " HEALTH=" + indicHealth);

    }
}
