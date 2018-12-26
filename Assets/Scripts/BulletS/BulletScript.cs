using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float explDiam;
    public GameObject terrain;
    public GameObject trailer_pref;
    public GameObject trailer;
    Rigidbody2D rigid;
    public GameObject fragment_pref;
    //public GameObject fragment;
    public int fragNum;
    public float explForce;
    GameObject[] fragment_pool;
    Collider2D collider;
    DamageControllerScript damageControllerScript;
    public int damage = 10;


    // Use this for initializationz
    void Start () {
        terrain = GameObject.Find("Terrain");
        rigid = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        damageControllerScript = FindObjectOfType<DamageControllerScript>();

        // создаем трейл
        trailer = Instantiate(trailer_pref);
        //trailer.SetActive(false);

        if (fragNum > 0) {
            fragment_pool = new GameObject[fragNum];
            for(int i = 0; i < fragNum; i++) {
                GameObject fragment = Instantiate(fragment_pref);
                //fragment.SetActive(false);
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
            //collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);
            //Destroy(gameObject);
        }

        MakeDamage();
        SetFragmentOnAndSpeed();
        TrailerOff();
        BackToPool();
    }
    void OnEnable() {
        TrailerOn();
        if (rigid) rigid.WakeUp();
    }

    void TrailerOn() {
        if (trailer) {
            //Debug.Log("BULLET FRAG TRAILER "+ gameObject.name);
            trailer.transform.SetParent(transform);
            trailer.transform.localPosition = Vector3.zero;
            trailer.gameObject.SetActive(true);
        }
    }

    public void TrailerOff() {
        if (trailer) {
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
            float angle = 2f * Mathf.PI * (float) i / (float) fragNum;
            fragment.GetComponent<Rigidbody2D>().AddForce(new Vector2(explForce*Mathf.Cos(angle), explForce*Mathf.Sin(angle)), ForceMode2D.Impulse);
        }
    }

    void MakeDamage() {
        // TODO to Start for speed
        Damagable[] damagables = damageControllerScript.GetDamagables();
        foreach(Damagable dmg in damagables) {
            if (collider.IsTouching(dmg.GetCollider())) dmg.GetHealth().HealthDecrease(damage);
        }
    }
}
