using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragPoolScript : MonoBehaviour
{
    public GameObject fragment_pref;
    //public GameObject fragment;
    public int fragNum;
    public float explForce;
    GameObject[] fragment_pool;
    public float testDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        fragment_pool = new GameObject[fragNum];
        for(int i = 0; i < fragNum; i++) {
        GameObject fragment = Instantiate(fragment_pref);
        //fragment.SetActive(false);
        fragment_pool[i] = fragment;
        }
        
        // деактивируем снаряд при добавлении в пул
        //gameObject.SetActive(false);
    }

    void OnEnable() {
        //StartCoroutine(Test());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // при столкновении снаряда с повехностью делаем дырку и удаляем снаряд
        if (collision.gameObject.name == "Terrain")
        {
            //collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);
            //Destroy(gameObject);
        }

        SetFragmentOnAndSpeed();
        //BackToPool();
    }

    void SetFragmentOnAndSpeed() {
        for(int i = 0; i < fragNum; i ++) {
            GameObject fragment = fragment_pool[i];
            fragment.transform.position = transform.position;
            fragment.SetActive(true);
            float angle = 2f * Mathf.PI * (float) i / (float) fragNum;
            //fragment.GetComponent<Rigidbody2D>().AddRelativeForce((Quaternion.Euler(0f, 0f, (180f*(float)i/(float)fragNum)) * Vector2.up * explForce), ForceMode2D.Impulse);
            fragment.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(explForce*Mathf.Cos(angle), explForce*Mathf.Sin(angle)), ForceMode2D.Impulse);
            //fragment.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * explForce, ForceMode2D.Impulse);
        }
        //gameObject.SetActive(false);
    }
}
