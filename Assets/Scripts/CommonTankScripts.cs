using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonTankScripts : MonoBehaviour
{

    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<TankScript>().target;
    }

    public void ImWeak(bool weakness) {
        // сообщаем противнику о маленьком остатке здоровья
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

    public void EnemyFSMTurn() {
        // запускаем ход противника
        enemy.GetComponent<TankAIScript>().MyTurn();
    }
}
