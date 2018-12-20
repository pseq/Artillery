using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragScript : MonoBehaviour
{
    Rigidbody2D fragRigid;
    
    // Start is called before the first frame update
    void Start()
    {
        fragRigid = gameObject.GetComponent<Rigidbody2D>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("FRAG1 cont count " + collision.contactCount);
        Debug.Log("FRAG1 gameObject.name " + collision.gameObject.name);
        if (collision.gameObject.name != gameObject.name) {
            FragmentOff();
        }
        
    }

    void FragmentOff() {
        fragRigid.angularVelocity = 0f;
        fragRigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
