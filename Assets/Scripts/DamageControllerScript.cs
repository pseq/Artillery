using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControllerScript : MonoBehaviour
{
    Damagable[] damagables;

    // Start is called before the first frame update
    void Start()
    {
        HealthControllerScript[] healthWOColls = FindObjectsOfType<HealthControllerScript>();
        // временный список, потом копируется в массив для скорости использования
        List<Damagable> tmp = new List<Damagable>();
        foreach(HealthControllerScript hScript in healthWOColls) {
            Collider2D collider = hScript.gameObject.GetComponent(typeof (Collider2D)) as Collider2D;
            if (collider) tmp.Add(new Damagable(collider, hScript));
        }
        damagables = new Damagable[tmp.Count];
        tmp.CopyTo(damagables);
    }

    public Damagable[] GetDamagables() {
        return damagables;
    }
}
