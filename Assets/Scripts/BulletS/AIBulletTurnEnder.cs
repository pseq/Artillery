using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletTurnEnder : MonoBehaviour
{
    //todo set private
    //public TankAIScript tankAIScript;

void OnDisable() {
//get tank
    if (transform.parent)
        if (transform.parent.parent)
            if (transform.parent.parent.parent.GetComponent<TankAIScript>()) {
                Transform tank = transform.parent.parent.parent;
                tank.GetComponent<TankScript>().SetLastHitPoint(transform.position.x);
                tank.GetComponent<TankAIScript>().ShootEnded();
            }
    }   
}
