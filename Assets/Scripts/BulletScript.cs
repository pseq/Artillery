using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float explDiam;
    public GameObject terrain;
    public GameObject trailer_pref;
    GameObject trailer;

    // Use this for initializationz
    void Start () {
        terrain = GameObject.Find("Terrain");
        // создаем трейл
        trailer = Instantiate(trailer_pref);
        trailer.SetActive(false);

        // деактивируем снаряд при добавлении в пул
        //gameObject.SetActive(false);
    }

    void OnEnable() {
        TrailerOn();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // при столкновении снаряда с повехностью делаем дырку и удаляем снаряд
        if (collision.gameObject.name == "Terrain")
        {
            //collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);
            //Destroy(gameObject);
        }

        TrailerOff();
        //BackToPool();
    }

    void BackToPool() {
        gameObject.SetActive(false);
    }

    void TrailerOff() {
        if (trailer) {
            trailer.transform.SetParent(null);
        }    
    }

    void TrailerOn() {
        if (trailer) {
            trailer.transform.SetParent(transform);
            trailer.transform.localPosition = Vector3.zero;
            trailer.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
