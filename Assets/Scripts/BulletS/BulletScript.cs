using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float explDiam;
    GameObject terrain;
    public GameObject trailer_pref;
    public GameObject[] trailers;
    GameObject trailer;
    Rigidbody2D rigid;
    public GameObject fragment_pref;
    //public GameObject fragment;
    public int fragNum;
    public float explForce;
    GameObject[] fragment_pool;
    Collider2D collider;
    DamageControllerScript damageControllerScript;
    public int damage;
    Damagable[] damagables;


    // Use this for initializationz
    void Start () {
        terrain = GameObject.Find("Terrain");
        rigid = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        damageControllerScript = FindObjectOfType<DamageControllerScript>();
        damagables = damageControllerScript.GetDamagables();

        //trailers;
        // создаем массив трейлов. Нужно чтобы трейлы разных выстрелов могли существовать одновременно
        for(int i = 0; i < trailers.Length; i++)
            trailers[i] = Instantiate(trailer_pref);

        if (fragNum > 0) {
            fragment_pool = new GameObject[fragNum];
            for(int i = 0; i < fragNum; i++) {
                GameObject fragment = Instantiate(fragment_pref);
                fragment_pool[i] = fragment;
            }
        }

        // деактивируем снаряд при добавлении в пул
        BackToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // при столкновении снаряда с повехностью делаем дырку и удаляем снаряд
        if (collision.gameObject.name == "Terrain")
        {
            collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);
            //Destroy(gameObject);
        }

        MakeDamage();
        SetFragmentOnAndSpeed();
        TrailerOff();
        BackToPool();
    }

    void OnEnable() {
        if (rigid) rigid.WakeUp();
    }

    public void TrailerOn() {
        // чтобы у снаряда был только один трейлер
        TrailerOff();
        //выбираем первый неактивный трейл из массива
        for(int i = 0; i < trailers.Length; i++)
            //if (!trailers[i].activeSelf) trailer = trailers[i];
            if (!trailers[i].activeSelf) trailer = trailers[i];

        if (trailer) {
  //      Debug.Log(this.name + " Trailer ON" );
            trailer.transform.SetParent(transform);
            trailer.transform.localPosition = Vector3.zero;
            
            trailer.gameObject.SetActive(true);
            //Debug.Log(this.name + " TRAILER STATUS SELF:" + trailer.gameObject.activeSelf + " TRAILER STATUS HIE:" + trailer.gameObject.activeInHierarchy);
        }
    }

    public void TrailerOff() {
        if (trailer) {
//        Debug.Log(this.name + " Trailer OFF" );

            trailer.transform.SetParent(null);
        }    
    }

    void BackToPool() {
        rigid.Sleep();
        gameObject.SetActive(false);
    }

    void SetFragmentOnAndSpeed() {
        for(int i = 0; i < fragNum; i ++) {
            GameObject fragment = fragment_pool[i];
            fragment.transform.position = transform.position;
            fragment.SetActive(true);
            fragment.GetComponent<BulletScript>().TrailerOn();

            float angle = 2f * Mathf.PI * (float) i / (float) fragNum;
            fragment.GetComponent<Rigidbody2D>().AddForce(new Vector2(explForce*Mathf.Cos(angle), explForce*Mathf.Sin(angle)), ForceMode2D.Impulse);
        }
    }

    void MakeDamage() {
        foreach(Damagable dmg in damagables) {
            if (collider.IsTouching(dmg.GetCollider())) {
                HealthControllerScript healthScript = dmg.GetHealth();
                healthScript.HealthDecrease(damage);
                healthScript.Shooted();
            }
        }
    }
}
