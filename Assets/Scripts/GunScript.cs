using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    int currentBulletKey;
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
    public float gunAngle; // SET TO NOT PUBLIC!!!
    float lastScroll;
    Rigidbody2D bulletRigid;
    AudioSource tick;
    Dictionary<int,int> arsenal;
    int[] arsKeys;
    Transform hBar;
    //test
    //public int isRevertAngleWhenTurnar = -1;
    //GameObject testObject;
    //GameObject testObject2;
    //GameObject testObject3;

    void Awake() {
        poolManager = poolObject.gameObject.GetComponent<PoolManagerScript>();
        ddScript = dropDown.gameObject.GetComponent<DDScript>();
        //test
        //testObject = GameObject.FindGameObjectWithTag("test");
        //testObject2 = GameObject.FindGameObjectWithTag("test2");
        //testObject3 = GameObject.FindGameObjectWithTag("test3");
    }

    // Use this for initialization
    void Start () {
        tick = gameObject.GetComponent<AudioSource>();
        gunAngle = transform.eulerAngles.z  + transform.parent.eulerAngles.z;
        firePower = firePowerMultipler * 0.5f;
        hBar = transform.parent.GetChild(3);
        //SelectBullet(0);
    }

    public void SelectBullet(int key) { // key from arskey & dd
        if (arsKeys.Length > 0) {
        bullet = poolManager.GetFromPool(arsKeys[key]);
        bulletRigid = bullet.GetComponent<Rigidbody2D>();
        currentBulletKey = arsKeys[key]; // from arsenal
        }
        else bullet = null;        
    }

    public int[] MakeArsenal() {
        // создание арсенала
        arsenal = new Dictionary<int,int>();
        //arsenal.Add(0,5); // bullet_fug
        arsenal.Add(0,500); // bullet_sub
        //arsenal.Add(7,500); // bullet_frag
        //arsenal.Add(0,4); // bullet_frag
        //arsenal.Add(6,300); // bullet_frag
        //arsenal.Add(2,200); // bullet_frag
        arsenal.Add(5,100); // bullet_frag

        // получение ключей арсенала
        arsKeys = new int[arsenal.Count];
        int[] arsValues = new int[arsenal.Count];
        arsenal.Keys.CopyTo(arsKeys,0);
        arsenal.Values.CopyTo(arsValues,0);
        // создаем список снарядов в меню
        ddScript.CreateDDList(arsKeys, arsValues);

        return arsKeys;
    }

    public void Fire ()
    {
        if (bullet) {
        // fire
        bullet.SetActive(true);
        bullet.transform.SetParent(fireSpot.transform);
        //bullet.transform.SetParent(transform);
        //bulletRigid.rotation = transform.eulerAngles.z;
        bulletRigid.rotation = fireSpot.transform.eulerAngles.z;

        //Debug.Log("GUN REAL firePower " + firePower);

        bulletRigid.AddRelativeForce((new Vector2(forwardDirection, 0f)) * firePower, ForceMode2D.Impulse);
        bullet.transform.position = fireSpot.transform.position;

        // del from arsenal
        arsenal[currentBulletKey] --;
        if (arsenal[currentBulletKey] < 1) {
            int[] newArsKeys = new int[arsKeys.Length - 1];
            int i = 0;
            foreach(int arsKey in arsKeys) {
                if (arsenal[arsKey] > 0) {
                    newArsKeys[i] = arsKey;
                    i ++;
                }
            }
            arsKeys = newArsKeys;
        }
        // del from DD
        ddScript.SetCurrentBulletCount(arsenal[currentBulletKey]);
        }
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

    public void GunAngleChange (float newAngle)
    {
        //Turnaround
        //if (scroll.y < turnaroundScroll && lastScroll > turnaroundScroll) TurnAround();
        //lastScroll = scroll.y;

        //StartCoroutine(AngleChangeCoroutine(newAngle));
    }

    public float GunPowerToPoint (Vector2 target, float realAngle) {
        realAngle = realAngle * forwardDirection;
        float destX = Mathf.Abs(target.x - fireSpot.transform.position.x);
        float destY = target.y - fireSpot.transform.position.y;
        float g = Mathf.Abs(Physics2D.gravity.y);
        float a = Mathf.Deg2Rad * realAngle;
        float sin2a = Mathf.Sin(a*2);
        float cosa = Mathf.Cos(a);
    //Debug.DrawLine(Vector2.zero, new Vector2(destX,destY));
    //Debug.DrawLine(fireSpot.transform.position, target, Color.red);
    //Debug.Break();

        // TODO
        // добавить обработку ошибок
        firePower = bulletRigid.mass * Mathf.Sqrt(g*destX*destX / (destX * sin2a - 2 * destY * cosa * cosa));
        return firePower;
    }
    
    public void GunPowerChange (Vector2 scroll)
    {
        float newFirePower = (1f - scroll.y) * firePowerMultipler;
        firePower = GunValuechange(firePower, newFirePower, powerStep, powerText);
    }

    float GunValuechange (float oldVal, float newVal, float step, Text textField) {
        float delta = Math.Abs(oldVal - newVal);
        if (delta >= step) {
            newVal = oldVal - step * Math.Sign(oldVal - newVal);
            textField.text = newVal.ToString();
            tick.Play();
            return newVal;
        }
        else return oldVal;
    }

    public void TurnAround() {
        forwardDirection *= -1f;
        gunAngle *= -1f;
        hBar.localScale = Vector3.Scale(hBar.localScale, new Vector3(-1f,1f,1f));
        transform.parent.localScale = Vector3.Scale(transform.parent.localScale, new Vector3(-1f,1f,1f));
    }

/*
    IEnumerator AngleChangeCoroutine(float newAngle) {
        bool isRotated = false;
        float critAngle = transform.parent.eulerAngles.z + maxGunAngle;
        float delta90lastAngle = Mathf.DeltaAngle(critAngle, transform.eulerAngles.z);
        Debug.Log("GUN critAngle " + critAngle);
        while (Mathf.Abs(Mathf.DeltaAngle(newAngle, transform.eulerAngles.z)) > angleChandeAccuracy) {
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, newAngle, gunRotSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (Mathf.Abs(Mathf.DeltaAngle(transform.parent.eulerAngles.z, transform.eulerAngles.z)) > maxGunAngle && !isRotated) {
            TurnAround();
            isRotated = true;
            newAngle -= 180;
        }

        yield return new WaitForSeconds(Time.deltaTime);
        }
        Fire();
    }
    */
}
