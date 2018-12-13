using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    public GameObject fireSpot;
    public float firePowerMultipler = 6f;
    public float maxGunAngle = 90f;
    public float forwardDirection = 1f;
    public float turnaroundScroll = - 0.1f;
    public float angleStep = 1f;
    public float powerStep = 0.1f;
    public Text angleText;
    public Text powerText;
    float firePower;
    float gunAngle;
    float lastScroll;
    Rigidbody2D bulletRigid;
    AudioSource tick;
    Dictionary<int,int> arsenal;
    //tmp
    public GameObject bullet_fug;
    public GameObject bullet_sub;
    public GameObject bullet_frag;


    // Use this for initialization
    void Start () {
        bulletRigid = bullet.GetComponent<Rigidbody2D>();
        tick = gameObject.GetComponent<AudioSource>();

        gunAngle = transform.eulerAngles.z  + transform.parent.eulerAngles.z;
        firePower = firePowerMultipler * 0.5f;

        //Заполняем арсенал
        arsenal = MakeArsenal();
        //arsenal = new Dictionary<int, int>();
    }
	
    public Dictionary<int,int> MakeArsenal() {
        Dictionary<int,int> ars = new Dictionary<int,int>();
        ars.Add(0,5); // bullet_fug
        ars.Add(1,4); // bullet_sub
        ars.Add(2,3); // bullet_frag
        return ars;
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
        //Turnaround
        if (scroll.y < turnaroundScroll && lastScroll > turnaroundScroll) TurnAround();
        lastScroll = scroll.y;

        float newGunAngle = (1f - scroll.y) * maxGunAngle * forwardDirection;
        gunAngle = GunValuechange(gunAngle, newGunAngle, angleStep, angleText);
        transform.eulerAngles = new Vector3(0f,0f,gunAngle + transform.parent.eulerAngles.z);
    }

    public void GunPowerChange (Vector2 scroll)
    {
        float newFirePower = (1f - scroll.y) * firePowerMultipler;
        firePower = GunValuechange(firePower, newFirePower, powerStep, powerText);
    }

    float GunValuechange (float oldVal, float newVal, float step, Text textField) {
        float delta = Math.Abs(oldVal - newVal);
        if (delta >= step) {
            newVal = oldVal - step * ((oldVal - newVal)/Math.Abs(oldVal - newVal));
            textField.text = newVal.ToString();
            tick.Play();
            return newVal;
        }
        else return oldVal;
    }

    public void TurnAround() {
        forwardDirection *= -1f;
        gunAngle *= -1f;
        transform.parent.localScale = Vector3.Scale(transform.parent.localScale, new Vector3(-1f,1f,1f));
    }

    public void BulletChooser(int bulletTag) {
        // здесь выбрать из словаря снаряд
    }
}
