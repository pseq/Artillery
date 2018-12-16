using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {


	public Dictionary<int,int> testPool;

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);

        testPool = new Dictionary<int, int>();

        for (int i = 0; i < 5; i ++) {
            testPool.Add(i, i+10);
        }

        //Debug.Log(testPool);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnDropchanged(int choose) {
        Debug.Log("TESTOBJECT DDChoosed:"+ choose);
    } 


}
