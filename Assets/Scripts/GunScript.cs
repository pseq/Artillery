using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    public float firePower;
    public GameObject fireSpot;
    public float maxGunAngle = 90;

    // Use this for initialization
    void Start () {
    }
	
    public void Fire ()
    {
        bullet.transform.SetParent(transform);
        bullet.GetComponent<Rigidbody2D>().angularVelocity = 0f; 
        bullet.GetComponent<Rigidbody2D>().rotation = transform.eulerAngles.z;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * firePower, ForceMode2D.Impulse);
        bullet.transform.position = fireSpot.transform.position;
    }

    public void GunAngleChange (Vector2 scroll)
    {
        //Debug.Log(scroll);
        float gunAngle = (1f - scroll.y) * maxGunAngle + transform.parent.eulerAngles.z;
        transform.eulerAngles = new Vector3(0f,0f,gunAngle);
    }

    public void TEststst() {

    }


    // Update is called once per frame
    void Update () {


    }
}
