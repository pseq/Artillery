using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootKatusha : MonoBehaviour
{
    //GameObject[] fragment_pool;
    float shootPower;
    public float shootDispersion;
    public float shootDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        //fragment_pool = GetComponent<BulletScript>().fragment_pool;

        // выстрел
        if (transform.parent) {
        // параметры берём из firespot
            shootPower = transform.parent.GetComponent<FirespotScript>().GetShootPower();
            if (shootPower != 0) {
                // стреляем по очереди каждым фрагментом
                StartCoroutine(ShootDelay(GetComponent<BulletScript>().GetFragmentPool()));
            }
        }
    }

    IEnumerator ShootDelay(GameObject[] fragment_pool)
    {   
        for(int i = 0; i < fragment_pool.Length; i ++) {
            Shoot(fragment_pool[i]);
            yield return new WaitForSeconds(shootDelay);
        }
        GetComponent<BulletScript>().BackToPool();
    }

    void Shoot(GameObject fragment) {
        fragment.SetActive(true);
        fragment.GetComponent<BulletScript>().TrailerOn();
        fragment.transform.position = transform.parent.position;
        fragment.GetComponent<Rigidbody2D>().rotation = transform.parent.eulerAngles.z + Random.Range(-shootDispersion, shootDispersion);
        fragment.GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(shootPower, 0f)), ForceMode2D.Impulse);
    }

}
