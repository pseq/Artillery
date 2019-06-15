using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public GameObject trailer_pref;
    public GameObject[] trailers;
    GameObject trailer;
    Rigidbody2D rigid;
    public GameObject fragment_pref;
    public int fragNum;

    bool first = true;

    public GameObject[] fragmentPool;
    public DamagerBulletScript damager;
    PoolManagerScript poolManager;

    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        //audioSource = GetComponent<AudioSource>();

        //trailers;
        // создаем массив трейлов. Нужно чтобы трейлы разных выстрелов могли существовать одновременно
        for(int i = 0; i < trailers.Length; i++)
            trailers[i] = Instantiate(trailer_pref);

        if (fragNum > 0) {
            fragmentPool = new GameObject[fragNum];
            for(int i = 0; i < fragNum; i++) {
                GameObject fragment = Instantiate(fragment_pref);
                fragmentPool[i] = fragment;
            }
        }

        poolManager = GameObject.FindGameObjectWithTag("poolManager").GetComponent<PoolManagerScript>();
        // деактивируем снаряд при добавлении в пул
        BackToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(transform.name + " " + collision.transform.name);

        if (damager) damager.MakeDamage();
        TrailerOff();
        BackToPool();
    }

    void OnEnable() {
        if (rigid) rigid.WakeUp();

        //сообщаем менеджеру пула, что количество активных снарядов увеличилось
        if (!first) poolManager.IncreaseActiveBullets();
    }

    public void TrailerOn() {
        // чтобы у снаряда был только один трейлер
        TrailerOff();
        //выбираем первый неактивный трейл из массива
        for(int i = 0; i < trailers.Length; i++)
            if (!trailers[i].activeSelf) trailer = trailers[i];

        if (trailer) {
            trailer.transform.SetParent(transform);
            trailer.transform.localPosition = Vector3.zero;
            trailer.gameObject.SetActive(true);
        }
    }

    public void TrailerOff() {
        if (trailer) trailer.transform.SetParent(null);   
    }

    public void BackToPool() {
        rigid.Sleep();

        //сообщаем менеджеру пула, что снаряд деактивируется для учета конца залпа
        if (!first) poolManager.DecreaseActiveBullets();
        first = false;
  
        gameObject.SetActive(false);
    }

    public int GetDamage() {
        if (damager) return damager.damage;
        else return 0;
    }

    public GameObject[] GetFragmentPool() {
        return fragmentPool;
    }

    //public void SetPoolManager() => poolManager = pool;
}
