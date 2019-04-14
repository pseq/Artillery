using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    //GameObject terrain;
    public GameObject trailer_pref;
    public GameObject[] trailers;
    GameObject trailer;
    Rigidbody2D rigid;
    public GameObject fragment_pref;
    //public GameObject fragment;
    public int fragNum;
    public float explForce;
    GameObject[] fragment_pool;
    public DamagerBulletScript damager;
    AudioSource audioSource;
    public AudioClip shootSound;


    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();

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
        //if (collision.gameObject.name == "Terrain") collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);

        //Debug.Log(collision.collider.gameObject.name);
        //Debug.Break();

        damager.MakeDamage();
        SetFragmentOnAndSpeed();
        TrailerOff();
        BackToPool();
    }

    void OnEnable() {
        if (rigid) rigid.WakeUp();
    }

    public void Shoot(float firePower, Direction forwardDirection) {
        audioSource.PlayOneShot(shootSound);
        TrailerOn();
        transform.position = transform.parent.position;
        rigid.rotation = transform.parent.eulerAngles.z;
        rigid.AddRelativeForce((new Vector2((float) forwardDirection, 0f)) * firePower, ForceMode2D.Impulse);
    }

    public void TrailerOn() {
        // чтобы у снаряда был только один трейлер
        TrailerOff();
        //выбираем первый неактивный трейл из массива
        for(int i = 0; i < trailers.Length; i++)
            //if (!trailers[i].activeSelf) trailer = trailers[i];
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

    public int GetDamage() {
        return damager.damage;
    }
}
