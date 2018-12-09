using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    float firePower = 0;
    public float firePowerMultipler = 5;
    public GameObject fireSpot;
    public float maxGunAngle = 90;
    Rigidbody2D bulletRigid;
    public float forwardDirection;
    float lastScroll;
    public float turnaroundScroll = - 0.2f;
    public Text AngleText;
    public Text PowerText;

    // Use this for initialization
    void Start () {
        bulletRigid = bullet.GetComponent<Rigidbody2D>();
        forwardDirection = 1f;
    }
	
    public void Fire ()
    {
        bullet.transform.SetParent(transform);
        bulletRigid.angularVelocity = 0f;
        bulletRigid.rotation = transform.eulerAngles.z;
        bulletRigid.velocity = Vector3.zero;
        bulletRigid.AddRelativeForce((new Vector2(forwardDirection, 0f)) * firePower, ForceMode2D.Impulse);
        bullet.transform.position = fireSpot.transform.position;
    }

    public void GunAngleChange (Vector2 scroll)
    {
        if (scroll.y < turnaroundScroll && lastScroll > turnaroundScroll) TurnAround();
        lastScroll = scroll.y;

        float gunAngle = (1f - scroll.y) * maxGunAngle * forwardDirection + transform.parent.eulerAngles.z;
        transform.eulerAngles = new Vector3(0f,0f,gunAngle);
        //AngleText.text = transform.eulerAngles.z.ToString();
        AngleText.text = (transform.eulerAngles.z + transform.parent.eulerAngles.z).ToString();
    }

    public void GunPowerChange (Vector2 scroll)
    {
        firePower = (1f - scroll.y) * firePowerMultipler;
        PowerText.text = firePower.ToString();
    }

    public void TurnAround() {
        forwardDirection *= -1f;
        transform.parent.localScale = Vector3.Scale(transform.parent.localScale, new Vector3(-1f,1f,1f));
    }

    // Update is called once per frame
    void Update () {


    }
}
