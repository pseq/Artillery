using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletScript : MonoBehaviour
{
 //   AudioSource audioSource;
 //   public AudioClip shootSound;
    Rigidbody2D rigid;
    BulletScript bScript;
    Vector2 shootParams;


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
            shootParams = transform.parent.GetComponent<FirespotScript>().GetShootParams();
            if (shootParams != Vector2.zero) {
                //audioSource.PlayOneShot(shootSound);
                bScript.TrailerOn();
                transform.position = transform.parent.position;
                rigid.rotation = transform.parent.eulerAngles.z;
                rigid.AddRelativeForce((new Vector2(shootParams.y, 0f)) * shootParams.x, ForceMode2D.Impulse);
            }
        }
    }
}
