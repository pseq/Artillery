using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSplitScript : MonoBehaviour
{

    Rigidbody2D rigid;
    BulletScript bulletScript;
    public float splitAngle;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletScript = GetComponent<BulletScript>();
    }

    void OnEnable() {
        transform.rotation = Quaternion.identity;
    }


    void Update()
    {
        // доворачиваем сенсор по направлению движения
        transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, rigid.velocity), Vector3.forward);
    }

    private void OnTriggerEnter2D() {
        Split(bulletScript.GetFragmentPool());
        bulletScript.TrailerOff();
        bulletScript.BackToPool();
    }

    void Split(GameObject[] fragmentPool) {

            float k = fragmentPool.Length / 2 + .5f;
            for(int i = 0; i < fragmentPool.Length; i ++) {
            GameObject fragment = fragmentPool[i];
            fragment.transform.position = transform.position;
            fragment.SetActive(true);
            fragment.GetComponent<BulletScript>().TrailerOn();

            fragment.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, splitAngle * (i - k)) * rigid.velocity;
        }
    }

}
