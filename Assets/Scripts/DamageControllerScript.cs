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
        //Debug.Log("DamControl WOColls" + healthWOColls.Length);
        // временный список, потом копируется в массив для скорости использования
        List<Damagable> tmp = new List<Damagable>();
        foreach(HealthControllerScript hScript in healthWOColls) {
            Collider2D collider = hScript.gameObject.GetComponent(typeof (Collider2D)) as Collider2D;
            if (collider) tmp.Add(new Damagable(collider, hScript));
        }
        //Debug.Log("DamControl TMP" + tmp.Count);

        damagables = new Damagable[tmp.Count];
        //Debug.Log("DamControl Dam" + damagables.Length);

        tmp.CopyTo(damagables);
        //Debug.Log("DamControl With Colls" + tmp.Count);
    }

    public Damagable[] GetDamagables() {
        return damagables;
    }
}
