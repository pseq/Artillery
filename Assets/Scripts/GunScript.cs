using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GunScript : MonoBehaviour {

    public GameObject bullet;
    int currentBulletKey;
    //public GameObject poolObject;
    public GameObject dropDown;
    public PoolManagerScript poolManager;
    DDScript ddScript;
    //public GameObject fireSpot;
    public Transform fireSpot;
    public float firePowerMultipler;
    public float maxGunAngle = 90;
    //public float forwardDirection = 1f;
    public Direction forwardDirection = Direction.Right;
    public float turnaroundScroll = - 0.1f;
    public float angleStep = 1f;
    public float powerStep = 0.1f;
    public Text angleText;
    public Text powerText;
    float firePower;
    public float gunAngle; // SET TO NOT PUBLIC!!!
    float lastScroll;
    Rigidbody2D bulletRigid;
    AudioSource audioSource;
    public AudioClip tickSound;
    //public AudioClip shootSound;
    Dictionary<int,int> arsenal;
    int[] arsKeys;
    Transform hBar;
    public AimUIScript aimUI;
    public Transform aimSectorCurrent;
    public Transform aimSectorLast;
    public float aimSectorMax;
    Quaternion lastSectorRot;

    void Awake() {
        //poolManager = poolObject.gameObject.GetComponent<PoolManagerScript>();
        ddScript = dropDown.gameObject.GetComponent<DDScript>();
    }

    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        gunAngle = transform.eulerAngles.z  + transform.parent.eulerAngles.z;
        firePower = firePowerMultipler * 0.5f;
        // TODO изменить
        hBar = transform.parent.GetChild(3);

        if (aimSectorCurrent) {
            aimSectorCurrent.GetChild(0).localScale = Vector3.one * aimSectorMax * firePower;
        //aimSectorCurrent.localScale = Vector3.one * aimSectorMax * firePower;
            aimSectorLast.GetChild(0).localScale = Vector3.zero;
        }
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
        arsenal.Add(1,100); // bullet_sub
        arsenal.Add(2,150); // bullet_sub
        arsenal.Add(3,300); // bullet_sub
        //arsenal.Add(7,500); // bullet_frag
        //arsenal.Add(0,4); // bullet_frag
        //arsenal.Add(6,300); // bullet_frag
        //arsenal.Add(2,200); // bullet_frag

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
        bullet.transform.SetParent(fireSpot);
        fireSpot.GetComponent<FirespotScript>().SetShootParams(new Vector2(firePower, (float)forwardDirection));
        bullet.SetActive(true);
        // даем ИИ знать, что выстрел совершен
        transform.parent.GetComponent<TankAIScript>().ShootStarted();

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

        // рисуем указатель последнего выстрела
        if (aimSectorLast) {
            lastSectorRot = aimSectorCurrent.rotation;
            aimSectorLast.rotation = lastSectorRot;
            aimSectorLast.GetChild(0).localScale = aimSectorCurrent.GetChild(0).localScale;
            aimSectorLast.localScale = aimSectorCurrent.localScale;
            }
    }

    public float GunPowerToPoint (Vector2 target, float realAngle, int side) {
        realAngle = realAngle * side;
        float destX = Mathf.Abs(target.x - fireSpot.position.x);
        float destY = target.y - fireSpot.position.y;
        float g = Mathf.Abs(Physics2D.gravity.y) * bulletRigid.gravityScale;
        float a = Mathf.Deg2Rad * realAngle;
        float sin2a = Mathf.Sin(a*2);
        float cosa = Mathf.Cos(a);

        // TODO
        // добавить обработку ошибок
        firePower = bulletRigid.mass * Mathf.Sqrt(g*destX*destX / (destX * sin2a - 2 * destY * cosa * cosa));

        return firePower;
    }
 
    float GunValuechange (float oldVal, float newVal, float step, Text textField) {
        float delta = Math.Abs(oldVal - newVal);
        if (delta >= step) {
            newVal = oldVal - step * Math.Sign(oldVal - newVal);
            textField.text = newVal.ToString();
            audioSource.PlayOneShot(tickSound);
            return newVal;
        }
        else return oldVal;
    }

    public void GunAngleChange (Vector2 scroll)
    {
        //Turnaround
        if (scroll.y < turnaroundScroll && lastScroll > turnaroundScroll) TurnAround();
        lastScroll = scroll.y;

        float newGunAngle = (1f - scroll.y) * maxGunAngle * (float)forwardDirection;
        gunAngle = GunValuechange(gunAngle, newGunAngle, angleStep, angleText);
        transform.eulerAngles = new Vector3(0f,0f,gunAngle + transform.parent.eulerAngles.z);

        AimSectorUpdate();
    }

    public void GunPowerChange (Vector2 scroll)
    {
        float newFirePower = (1f - scroll.y) * firePowerMultipler;
        firePower = GunValuechange(firePower, newFirePower, powerStep, powerText);

        AimSectorUpdate();
    }

    public void AimSectorUpdate() {
        if (aimSectorCurrent) {
            aimSectorCurrent.position = transform.position;
            aimSectorCurrent.rotation = transform.rotation;
            aimSectorCurrent.GetChild(0).localScale = Vector3.one * aimSectorMax * firePower;
        }
    }

    public void TurnAround() {
        forwardDirection = (Direction) (-1f * (float)forwardDirection);
        gunAngle *= -1f;
        hBar.localScale = Vector3.Scale(hBar.localScale, new Vector3(-1f,1f,1f));
        transform.parent.localScale = Vector3.Scale(transform.parent.localScale, new Vector3(-1f,1f,1f));
        // сбрасываем вращение прицельной сетки
        if (aimUI) {
            aimUI.Reset();
            aimSectorLast.rotation = lastSectorRot;
            aimSectorLast.localScale = Vector3.Scale(aimSectorLast.localScale, new Vector3(-1f,1f,1f));
        }
    }
}
