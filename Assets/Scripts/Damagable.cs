//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Damagable //: MonoBehaviour
{
 
Collider2D collider;
HealthControllerScript healthScript;

    public Damagable(Collider2D collider, HealthControllerScript healthScript) {
        this.collider = collider;
        this.healthScript = healthScript;
    }

    public Collider2D GetCollider() {
        return collider;
    }

    public HealthControllerScript GetHealth() {
        return healthScript;
    }
}
