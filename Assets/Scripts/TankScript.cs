using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
public float speed = 1f;
WheelJoint2D leftWheel, rightWheel;
Rigidbody2D leftWheelRigid, rightWheelRigid;

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftWheelObj = transform.GetChild(1).gameObject;
        GameObject rightWheelObj = transform.GetChild(2).gameObject;
        rightWheel = rightWheelObj.GetComponent<WheelJoint2D>();
        leftWheel = leftWheelObj.GetComponent<WheelJoint2D>();
        rightWheelRigid = rightWheelObj.GetComponent<Rigidbody2D>();
        leftWheelRigid = leftWheelObj.GetComponent<Rigidbody2D>();

        Debug.Log("TANK CHILD 0 " + transform.GetChild(0).name);
        Debug.Log("TANK CHILD 1 " + transform.GetChild(1).name);
        Debug.Log("TANK CHILD 2 " + transform.GetChild(2).name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float dist) {
        // формула вычисления времени из расстояния
        float time = dist/speed;

        StartCoroutine(MoveCoroutine(time));
    }

    IEnumerator MoveCoroutine(float time) {
        // move
        rightWheelRigid.freezeRotation = false;
        leftWheelRigid.freezeRotation = false;
        if (time > 0) leftWheel.useMotor = true;
        else rightWheel.useMotor = true;
        //wait
        yield return new WaitForSeconds(Mathf.Abs(time));
        // stop
        rightWheelRigid.freezeRotation = true;
        leftWheelRigid.freezeRotation = true;
        rightWheel.useMotor = false;
        leftWheel.useMotor = false;
    }
}
