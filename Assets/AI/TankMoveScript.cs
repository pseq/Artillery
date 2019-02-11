using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveScript : MonoBehaviour
{

    
    float cantMovinDelta  = 5f;
    public float changePositionDistance  = 1f;
    public float speed = 1f;
    WheelJoint2D leftWheel, rightWheel;
    Rigidbody2D leftWheelRigid, rightWheelRigid;
    public GameObject leftWheelObj, rightWheelObj;

    // Start is called before the first frame update
    void Start()
    {
        //leftWheelObj = transform.GetChild(1).gameObject;
        //rightWheelObj = transform.GetChild(2).gameObject;
        rightWheel = rightWheelObj.GetComponent<WheelJoint2D>();
        leftWheel = leftWheelObj.GetComponent<WheelJoint2D>();
        rightWheelRigid = rightWheelObj.GetComponent<Rigidbody2D>();
        leftWheelRigid = leftWheelObj.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetTrackReserve() {
        // TODO make this
        return 200;
    }

    public void Move(float dist) {
        // Движение в точку
        // TODO формула вычисления времени из расстояния
        float time = dist/speed;
        Debug.Log("НЕ ТОТ МУВ");
        //StartCoroutine(MoveCoroutine(time));
    }

    public void Test(int dir) {
        Debug.Log("MOVE " + (Direction)dir);
        Move((Direction)dir);
    }

    public void Move(Direction dir) {
        // Движение в направлении
        float enemyPos = GetComponent<TankScript>().target.transform.position.x;
        int side = (int)dir;
        if (transform.position.x > enemyPos) side *= -1;
        float time = changePositionDistance/speed * side;
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
    
    IEnumerator MoveCoroutine2(float pos) {
        // move
        rightWheelRigid.freezeRotation = false;
        leftWheelRigid.freezeRotation = false;
        if (pos > transform.position.x) leftWheel.useMotor = true;
        else rightWheel.useMotor = true;
        //wait
        //Debug.Log("TANK START MOVE " + transform.name);
		while (!TestReach(pos) && !CantMove)
        yield return new WaitForSeconds(Mathf.Abs(time));
        // stop
        //Debug.Log("TANK STOP MOVE " + transform.name);
        rightWheelRigid.freezeRotation = true;
        leftWheelRigid.freezeRotation = true;
        rightWheel.useMotor = false;
        leftWheel.useMotor = false;
    }
}
