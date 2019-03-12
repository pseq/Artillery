using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonTankScripts : MonoBehaviour
{

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ImWeak(bool weakness) {
        enemy.GetComponent<TankAIScript>().EnemyIsWeak(weakness);
    }

// TODO заменить везде
    public Direction DirToEnemy() {
        if (transform.position.x > enemy.transform.position.x) return Direction.Left;
        else return Direction.Right; 
    }

    public Direction DirAwayEnemy() {
        if (transform.position.x > enemy.transform.position.x) return Direction.Right;
        else return Direction.Left; 
    }
}
