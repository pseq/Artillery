using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveScript : MonoBehaviour
{
    public float fuel;
    public float changePositionDistance  = 20f;
    public float positionSharpness = 1f;
    public float speed = 180f;
    public float motorPower = 1000f;
    public WheelJoint2D[] wheels;
    JointMotor2D motor;
    public int cantMoveFrames = 10;
    public float cantMoveDencity = .5f;

    // todo заменить на формулу
    public float distPerFuel = 10f;
    //public Transform test;
    //public float oldtestX;

    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<WheelJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //test
        //if(oldtestX != test.position.x) Move(test.position.x);
        //oldtestX = test.position.x;
    }

    public float GetTrackReserve() {
        return fuel/distPerFuel;
    }

    public void Move(float pos) {
        // Движение в точку
        StartCoroutine(MoveCoroutine(pos));
    }

    public void Move(Direction dir) {
        // Движение в направлении
        float tankPos = transform.position.x;
        int side = (int)dir;
        float sign = Mathf.Sign(tankPos - GetComponent<TankScript>().target.transform.position.x);
        StartCoroutine(MoveCoroutine(tankPos - changePositionDistance * side * sign));
    }
    
    IEnumerator MoveCoroutine(float pos) {
        Direction dir = Direction.Left;
        if (pos > transform.position.x) dir = Direction.Right;
        StartMove(dir);
        
        float lastPosition = transform.position.x;
        int i = 0;

        while (!TestReach(pos) && fuel > 0) {

            i++;
            // проверяем периодически изменение позиции, и если не едет - прерываем цикл, и устанавливаем триггер
            if (i % cantMoveFrames == 0)  {
                if (Mathf.Abs(lastPosition - transform.position.x) < cantMoveDencity) {
                    GetComponent<TankAIScript>().CantMove();
                    break;
                }
                lastPosition = transform.position.x;
            }
            yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
        }
        // stop
        StopMove();

    }
    
    IEnumerator FuelDecreaseCoroutine() {
        while (true) {
            FuelDecrease();
            //yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
            yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
        }
    }
	
    bool TestReach(float pos) {
        Debug.Log("MOVE TESTReach d=" + Mathf.Abs(pos - transform.position.x) + " shrp=" + positionSharpness);
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
        GetComponent<TankAIScript>().Moving(true);  
        motor.maxMotorTorque = motorPower;
        if (dir == Direction.Right) motor.motorSpeed = speed;
            else motor.motorSpeed = -speed;
        Debug.Log("MOVE motor.speed=" + motor.motorSpeed);      
        foreach(WheelJoint2D wheel in wheels) wheel.motor = motor;  
        StartCoroutine("FuelDecreaseCoroutine");
    }

    public void StopMove() {
        GetComponent<TankAIScript>().Moving(false);
        motor.motorSpeed = 0;
        foreach(WheelJoint2D wheel in wheels) wheel.motor = motor;        
        StopCoroutine("FuelDecreaseCoroutine");        
        // если сдвинулись - то считаем, что ушли от выстрела
        GetComponent<Animator>().SetBool("wasShooted", false);
    }
}
