using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_frag_script : MonoBehaviour
{

    public float explForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("BULLET COLLISION " + collision.collider.gameObject.name);
        SetFragmentOnAndSpeed(GetComponent<BulletScript>().GetFragmentPool());
    }

    void SetFragmentOnAndSpeed(GameObject[] fragmentPool) {
        for(int i = 0; i < fragmentPool.Length; i ++) {
            GameObject fragment = fragmentPool[i];
            fragment.transform.position = transform.position;
            fragment.SetActive(true);
            fragment.GetComponent<BulletScript>().TrailerOn();

            float angle = 2f * Mathf.PI * (float) i / (float) fragmentPool.Length;
            fragment.GetComponent<Rigidbody2D>().AddForce(new Vector2(explForce*Mathf.Cos(angle), explForce*Mathf.Sin(angle)), ForceMode2D.Impulse);
        }
    } 
}
