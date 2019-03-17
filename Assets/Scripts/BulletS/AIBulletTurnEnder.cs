using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletTurnEnder : MonoBehaviour
{
    TankAIScript tankAIScript;

    // Start is called before the first frame update
    void Start()
    {
        //get tank
        if (transform.parent)
            if (transform.parent.parent)
                if (transform.parent.parent.parent.GetComponent<TankAIScript>())
                    tankAIScript = transform.parent.parent.parent.GetComponent<TankAIScript>();
    }

    void OnDisable() {
        Debug.Log(gameObject.name + "BULLET disabled");
        if (tankAIScript) tankAIScript.ShootOK();
    }
}
