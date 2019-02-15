using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveScript : MonoBehaviour
{

    
    public float fuel  = 10000f;
    public float changePositionDistance  = 1f;
    public float positionSharpness = 1f;
    public float speed = 1f;
    WheelJoint2D leftWheel, rightWheel;
    Rigidbody2D leftWheelRigid, rightWheelRigid;
    public GameObject leftWheelObj, rightWheelObj;
    public int cantMoveFrames = 10;
    public float cantMoveDencity = .5f;

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
        return fuel/speed;
    }

    public void Move(float dist) {
        // Движение в точку
        // TODO формула вычисления времени из расстояния
        float time = dist/speed;
        //Debug.Log("НЕ ТОТ МУВ");
        //StartCoroutine(MoveCoroutine(time));
    }

    public void Test(int dir) {
        //Debug.Log("MOVE " + (Direction)dir);
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
    
    IEnumerator MoveCoroutine(float pos) {
            // move
            rightWheelRigid.freezeRotation = false;
            leftWheelRigid.freezeRotation = false;
            if (pos > transform.position.x) leftWheel.useMotor = true;
            else rightWheel.useMotor = true;
            float lastPosition = transform.position.x;
            int i = 0;
            //wait
            //Debug.Log("TANK START MOVE " + transform.name);
            while (!TestReach(pos) && fuel > 0) {
                i++;
                if (i % cantMoveFrames == 0)  {
                    Debug.Log("MOVE dens=" + Mathf.Abs(lastPosition - transform.position.x));
                    if (Mathf.Abs(lastPosition - transform.position.x) < cantMoveDencity) break;
                    lastPosition = transform.position.x;
                }
                FuelDecrease();
                yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f); или physicsUpdate
            }
            // stop
            rightWheelRigid.freezeRotation = true;
            leftWheelRigid.freezeRotation = true;
            rightWheel.useMotor = false;
            leftWheel.useMotor = false;
    }
	
    bool TestReach(float pos) {
                return(Mathf.Abs(pos - transform.position.x) < positionSharpness);
    }
	
    void FuelDecrease() {
         fuel--;
         if (fuel < 0) fuel = 0;   
    }

    public void StartCor(int l) {
        StartCoroutine(MoveCoroutine(l));
    }

    public void Stopcor() {
        StopCoroutine(MoveCoroutine(0));
    }
}
