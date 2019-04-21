using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootKatusha : MonoBehaviour
{
    //GameObject[] fragment_pool;
    Vector2 shootParams;
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
            shootParams = transform.parent.GetComponent<FirespotScript>().GetShootParams();
            if (shootParams != Vector2.zero) {
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
        fragment.GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(shootParams.y, 0f)) * shootParams.x, ForceMode2D.Impulse);
    }

}
