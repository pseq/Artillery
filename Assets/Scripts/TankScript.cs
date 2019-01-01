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

        //Debug.Log("TANK CHILD 0 " + transform.GetChild(0).name);
        //Debug.Log("TANK CHILD 1 " + transform.GetChild(1).name);
        //Debug.Log("TANK CHILD 2 " + transform.GetChild(2).name);
        //StartCoroutine(MoveCoroutine(1f));
    }

    public void Move(float dist) {
        // формула вычисления времени из расстояния
        float time = dist/speed;

        //Debug
        //Debug.Break();
        StartCoroutine(MoveCoroutine(time));
    }

    IEnumerator MoveCoroutine(float time) {
        // move
        rightWheelRigid.freezeRotation = false;
        leftWheelRigid.freezeRotation = false;
        if (time > 0) leftWheel.useMotor = true;
        else rightWheel.useMotor = true;
        //wait
        //Debug.Log("TANK START MOVE " + transform.name);

        yield return new WaitForSeconds(Mathf.Abs(time));
        // stop
        //Debug.Log("TANK STOP MOVE " + transform.name);

        rightWheelRigid.freezeRotation = true;
        leftWheelRigid.freezeRotation = true;
        rightWheel.useMotor = false;
        leftWheel.useMotor = false;
    }
}
