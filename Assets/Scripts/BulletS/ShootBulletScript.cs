using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletScript : MonoBehaviour
{
 //   AudioSource audioSource;
 //   public AudioClip shootSound;
    Rigidbody2D rigid;
    BulletScript bScript;
    float shootPower;


    // Start is called before the first frame update
    //void Start()
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bScript = GetComponent<BulletScript>();
     //   audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() {
        // выстрел
        if (transform.parent) {
            // параметры берём из firespot
            shootPower = transform.parent.GetComponent<FirespotScript>().GetShootPower();
            if (shootPower != 0) {
                bScript.TrailerOn();
                transform.position = transform.parent.position;
                rigid.rotation = transform.parent.eulerAngles.z;
                rigid.AddRelativeForce((new Vector2(shootPower, 0f)), ForceMode2D.Impulse);
            }
        }
    }
}
