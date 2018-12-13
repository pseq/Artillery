using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnDropchanged(int choose) {
        Debug.Log("Choosed:"+ choose);
    } 
}
