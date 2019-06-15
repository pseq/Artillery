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

    void Death()
    {
        //        Debug.Log(gameObject.name + " IS DEAD")
        //ищем все танки, и говорим каждому, что бой закончен
        GameObject[] aiControlled = GameObject.FindGameObjectsWithTag("AIControlled");
        foreach (GameObject aiObject in aiControlled)
        {
            TankAIScript tankAIScript = aiObject.GetComponent<TankAIScript>();
            tankAIScript.EndBattle();
        }
    }

    void DecreaseHealthIndicator(int decrHealth) {
        if (health >= 0) {
        //Debug.Log(gameObject.name + " HEALTH=" + health);
        // если здоровье меньше минимального выстрела - ставим флажки ИИ себе и противнику
        bool weakness = health <= weakestBullet.GetComponent<BulletScript>().GetDamage();
        GetComponent<TankAIScript>().CritHealth(weakness);
        GetComponent<CommonTankScripts>().ImWeak(weakness);
        float scaleDecrease = startIndicatorScale * decrHealth / startHealth;
        healthIndicator.transform.localScale -= new Vector3(scaleDecrease, 0, 0);
        }
    }
}
