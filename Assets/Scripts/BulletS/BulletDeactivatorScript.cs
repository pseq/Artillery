using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeactivatorScript : MonoBehaviour
{
    public BulletScript bulletScript;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();       
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // при столкновении снаряда с повехностью делаем дырку и удаляем снаряд
        if (collision.gameObject.name == "Terrain")
        {
            //collision.gameObject.GetComponent<TerrainScript>().TerrainHole(gameObject, explDiam);
            //Destroy(gameObject);
        }

        bulletScript.TrailerOff();
        BackToPool();
    }

    void BackToPool() {
        rigid.angularVelocity = 0f;
        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }


}
