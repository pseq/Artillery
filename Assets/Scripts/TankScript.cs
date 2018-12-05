using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

    float gunAngle;
    float maxGunAngle = 90f;
    Transform gun;
    Transform body;

    // Use this for initialization
    void Start () {
		gun = gameObject.transform.GetChild(0).GetChild(0);
        body = gameObject.transform.GetChild(0);
        Debug.Log(gun.name + " " + gun);
        //gun.eulerAngles = new Vector3(0f,0f,0f);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void GunAngleChange (Vector2 scroll)
    {
        //Debug.Log(scroll);
        gunAngle = (1f - scroll.y) * maxGunAngle + body.eulerAngles.z;
        gun.eulerAngles = new Vector3(0f,0f,gunAngle);
    }
}
