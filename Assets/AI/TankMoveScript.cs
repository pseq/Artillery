using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveScript : MonoBehaviour
{

    
    public float fuel  = 10000f;
    public float changePositionDistance  = 20f;
    public float positionSharpness = 1f;
    public float speed = 180f;
    public float motorPower = 1000f;

    public WheelJoint2D[] wheels;
    JointMotor2D motor;
    public GameObject leftWheelObj, rightWheelObj;
    public int cantMoveFrames = 10;
    public float cantMoveDencity = .5f;
    //public bool isMoving = false;

    public Transform test;
    public float oldtestX;

    // Start is called before the first frame update
    void Start()
    {

        wheels = GetComponentsInChildren<WheelJoint2D>();
        //motor.maxMotorTorque = motorPower;
        //motor.motorSpeed = 270;
        //foreach(WheelJoint2D wheel in wheels) wheel.motor = motor;
    }

    // Update is called once per frame
    void Update()
    {
        //test
        if(oldtestX != test.position.x) Move(test.position.x);
        oldtestX = test.position.x;
    }

    public float GetTrackReserve() {
        // TODO make this
        return fuel;
    }

    public void Move(float pos) {
        // Движение в точку
        StartCoroutine(MoveCoroutine(pos));
    }

    public void Test(int dir) {
        //Debug.Log("MOVE " + (Direction)dir);
        Move((Direction)dir);
    }

    public void Move(Direction dir) {
        // Движение в направлении
        float tankPos = transform.position.x;
        int side = (int)dir;
        float sign = Mathf.Sign(tankPos - GetComponent<TankScript>().target.transform.position.x);
        StartCoroutine(MoveCoroutine(tankPos - changePositionDistance * side * sign));
        //if (tankPos - enemyPos > 0)     StartCoroutine(MoveCoroutine(tankPos - changePositionDistance * side));
          //                      else    StartCoroutine(MoveCoroutine(tankPos + changePositionDistance * side));
    }
    
    IEnumerator MoveCoroutine(float pos) {
        // move
        Direction dir = Direction.Left;
        if (pos > transform.position.x) dir = Direction.Right;
        StartMove(dir);
        
        float lastPosition = transform.position.x;
        int i = 0;

        while (!TestReach(pos) && fuel > 0) {

            //cant move test
            i++;
            if (i % cantMoveFrames == 0)  {
                Debug.Log("MOVE dens=" + Mathf.Abs(lastPosition - transform.position.x));
                if (Mathf.Abs(lastPosition - transform.position.x) < cantMoveDencity) break;
                lastPosition = transform.position.x;
            }
            yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
        }
        // stop
        Debug.Log("NOW STOP");
        StopMove();
    }
    
    IEnumerator FuelDecreaseCoroutine() {
        while (true) {
            FuelDecrease();
            yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
        }
    }
	
    //NOT WORKING
    bool TestReach(float pos) {
        //Debug.Log(Mathf.Abs(pos - transform.position.x));
        return(Mathf.Abs(pos - transform.position.x) < positionSharpness);
    }
	
    void FuelDecrease() {
         fuel--;
         if (fuel < 0) {
             fuel = 0;
             StopMove();
         }   
    }

    public void StartMove(int dir) {
        StartMove((Direction) dir);
    }

    public void StartMove(Direction dir) {
        StopMove();
        //rightWheelRigid.freezeRotation = false;
        //leftWheelRigid.freezeRotation = false;
        motor.maxMotorTorque = motorPower;

        if (dir == Direction.Right) motor.motorSpeed = speed;
            else motor.motorSpeed = -speed;
        Debug.Log("MOVE motor.speed=" + motor.motorSpeed);      
        foreach(WheelJoint2D wheel in wheels) wheel.motor = motor;  
        StartCoroutine("FuelDecreaseCoroutine");
    }

    public void StopMove() {
        //rightWheelRigid.freezeRotation = true;
        //leftWheelRigid.freezeRotation = true;
        //rightWheel.useMotor = false;
        //leftWheel.useMotor = false;
        motor.motorSpeed = 0;
        foreach(WheelJoint2D wheel in wheels) wheel.motor = motor;        
        StopCoroutine("FuelDecreaseCoroutine");
    }
}
