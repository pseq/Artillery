using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletTurnEnder : MonoBehaviour
{
    //todo set private
    //public TankAIScript tankAIScript;

    void OnDisable() {
//        Debug.Log("bullet OnDisable");
            
//get tank
    if (transform.parent)
        if (transform.parent.parent)
            if (transform.parent.parent.parent.GetComponent<TankAIScript>())
                transform.parent.parent.parent.GetComponent<TankAIScript>().ShootEnded();
    }
}
