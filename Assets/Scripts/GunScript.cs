using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    public GameObject poolObject;
    public GameObject dropDown;
    PoolManagerScript poolManager;
    DDScript ddScript;
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
    int[] arsKeys;

    void Awake() {
        poolManager = poolObject.gameObject.GetComponent<PoolManagerScript>();
        ddScript = dropDown.gameObject.GetComponent<DDScript>();
    }

    // Use this for initialization
    void Start () {
        tick = gameObject.GetComponent<AudioSource>();
        gunAngle = transform.eulerAngles.z  + transform.parent.eulerAngles.z;
        firePower = firePowerMultipler * 0.5f;
        SelectBullet(0);

        //poolObject.SetActive(true);
        //bullet = SelectBullet(1);
        //bulletRigid = bullet.GetComponent<Rigidbody2D>();
    }

    public void SelectBullet(int key) {
        bullet = poolManager.GetFromPool(arsKeys[key]);
    }

    public int[] MakeArsenal() {
        // создание арсенала
        arsenal = new Dictionary<int,int>();
        //arsenal.Add(0,5); // bullet_fug
        arsenal.Add(1,6); // bullet_sub
        arsenal.Add(7,5); // bullet_frag
        arsenal.Add(0,4); // bullet_frag
        arsenal.Add(6,3); // bullet_frag
        arsenal.Add(2,2); // bullet_frag
        arsenal.Add(5,1); // bullet_frag

        // получение ключей арсенала
        arsKeys = new int[arsenal.Count];
        int[] arsValues = new int[arsenal.Count];
        arsenal.Keys.CopyTo(arsKeys,0);
        arsenal.Values.CopyTo(arsValues,0);
        // создаем список снарядов в меню
        ddScript.CreateDDList(arsKeys, arsValues);

        return arsKeys;
    }

/*
    public int[] GetArsenalKeys() {
        int[] keys = new int[arsenal.Count];
        arsenal.Keys.CopyTo(keys,0);
		Debug.Log("keys " + keys);

        return keys;
    }
*/
    public void Fire ()
    {
        // delete
        // bullet = SelectBullet(1);
        bulletRigid = bullet.GetComponent<Rigidbody2D>();

        

        bullet.SetActive(true);
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
}
