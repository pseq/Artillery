using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
public float speed = 1f;
WheelJoint2D leftWheel, rightWheel;
Rigidbody2D leftWheelRigid, rightWheelRigid;
public GameObject target;
public float angleSearchStepGrad = 5f;
public GunScript gunScript;
float leftAimPoint;
float rightAimPoint;
float lastHitPoint;
float lastEnemyPosition;
float gunPower;
public float maxGunPower = 25;
bool isFirst = true;
bool moveDirChanged = false;
float cantMovinDelta  = 5f;
float changePositionDistance  = 1f;
int moveDirection = 1;
float lastGunPower;
public float minShootAngle = -360;
public float maxShootAngle = 360;

////// test
public GameObject test;
public int floorAngleSign = 1;
public int side = 1;


    // Start is called before the first frame update
    void Start()
    {
        GameObject leftWheelObj = transform.GetChild(1).gameObject;
        GameObject rightWheelObj = transform.GetChild(2).gameObject;
        rightWheel = rightWheelObj.GetComponent<WheelJoint2D>();
        leftWheel = leftWheelObj.GetComponent<WheelJoint2D>();
        rightWheelRigid = rightWheelObj.GetComponent<Rigidbody2D>();
        leftWheelRigid = leftWheelObj.GetComponent<Rigidbody2D>();
// TODO сделать выбор из размеров terrain
        leftAimPoint = 0;
        //rightAimPoint = GameObject.Find("Terrain").GetComponent<Mesh>().bounds.center.x;
        rightAimPoint = 150;
        lastHitPoint = target.transform.position.x;
        lastEnemyPosition = lastHitPoint;
        //gunPower = gunScript.GunPowerToPoint(target.transform.position, 45f);
        gunPower = 10;
    }

    public void Move(float dist) {
        // TODO формула вычисления времени из расстояния
        float time = dist/speed;
        StartCoroutine(MoveCoroutine(time));
    }

    bool ChangePosition() {
        lastGunPower = gunPower;

        float startPos = transform.position.x;
        Move(changePositionDistance * moveDirection);
        isFirst = false;
        if (Mathf.Abs(startPos - transform.position.x) > 0) return true;
        else return false;
    }
    
    public void Aim() {

        Transform selfTransform = gameObject.transform;
        Transform enemyTransform = target.transform;
        TurnaroundToEnemy(selfTransform, enemyTransform);

        // угол между танком и противником
        float floorAngle = floorAngleSign * Mathf.Atan2(enemyTransform.position.y - selfTransform.position.y, 
                                        enemyTransform.position.x - selfTransform.position.x) * Mathf.Rad2Deg;

        

        // находим углы для попадания по цели для обоих танков, и берем больший
        // ! float angle = Mathf.Max(ShootAngleSearch(target, gameObject), ShootAngleSearch(gameObject, target));
        //float alpha = ShootAngleSearch(gameObject, target);
        //float beta = ShootAngleSearch(target, gameObject);
        //float angle = Mathf.Max(alpha - floorAngle * Mathf.Sign(alpha), (beta + floorAngle) * Mathf.Sign(beta)) + floorAngle;
        float angle = ShootAngleSearch(gameObject, side);
        



        // чтобы мощность выстрела не превышала максимальную, увеличиваем угол до 45
        //while (gunScript.GunPowerToPoint(target.transform.position, angle) > maxGunPower && angle < 45f) angle += angleSearchStepGrad;

        Debug.Log("TANK FLOOR " + floorAngle + " " + gameObject.name);
        //Debug.Log("TANK A " + ShootAngleSearch(gameObject, target) + " " + gameObject.name);
        //Debug.Log("TANK B " + ShootAngleSearch(target, gameObject) + " " + gameObject.name);
        Debug.Log("TANK ANGLE " + angle);

        // Power Calc to Enemy
        gunPower =  gunScript.GunPowerToPoint(target.transform.position, angle);
        Debug.Log("TANK CLCLTED GP " + gunPower + " MAX GP " + maxGunPower + " " + gameObject.name);

        if (gunPower <= maxGunPower) {
            // завершаем алгоритм
            Debug.Log("TANK gunPower <= maxGunPower AiminOK");
            AiminOK(angle);
            return;
        } 
        else if (!isFirst) {
            if (gunPower > lastGunPower) {
                if (ChangeMovinDirection()) {
                    Debug.Log("TANK gunPower > lastGunPower AiminOK");

                    AiminOK(angle);
                    return;
                } else if (ChangePosition()) Aim(); // рекурсия            
            } else if (ChangePosition()) Aim();    
        } else if (ChangePosition()) Aim(); // рекурсия
    }

    bool ChangeMovinDirection() {
        moveDirection *= -1;
        if (!ChangePosition() && !moveDirChanged) {
            moveDirChanged = true;
            ChangeMovinDirection();
        }
        return moveDirChanged;
    }

    void AiminOK(float angle) {
        // SET GUN AIM
        isFirst = true;
        moveDirChanged = false;

        gunScript.transform.eulerAngles = new Vector3(0f,0f,angle);
        //gunScript.transform.eulerAngles = new Vector3(0f,0f,angle + transform.rotation.z * Mathf.Rad2Deg);
        //gunScript.transform.eulerAngles = new Vector3(0f,0f,angle + transform.eulerAngles.z);
        Debug.Log("TANK REAL GUN ANGLE " + angle + transform.eulerAngles.z);
        gunScript.Fire();
    }

    float ShootDistanceSelect() {
        float enemyPosition = target.transform.position.x;
        if (lastHitPoint > enemyPosition) {
            rightAimPoint = lastHitPoint;
            leftAimPoint = enemyPosition - lastEnemyPosition + leftAimPoint;
        }
        if (lastHitPoint < enemyPosition) {
            leftAimPoint = lastHitPoint;
            rightAimPoint = enemyPosition - lastEnemyPosition + rightAimPoint;
        }
        lastEnemyPosition = enemyPosition;
        return enemyPosition;
        //return Random.Range(leftAimPoint, rightAimPoint);
    } 

    float ShootAngleSearch(GameObject self, int course) {
        LayerMask mask = LayerMask.GetMask("Terrain");
        Transform selfTransform = self.transform;
        //Transform enemyTransform = enemy.transform;
        //float direction = Mathf.Sign(enemyTransform.position.x - selfTransform.position.x);
        //Vector2 lineTo = enemyTransform.position ;
        //float enemyAngle = Mathf.Atan2(lineTo.y - selfTransform.position.y, (lineTo.x - selfTransform.position.x) * direction) * Mathf.Rad2Deg;

        //Debug.Log("TANK TEST AN " + Mathf.Atan2(test.transform.position.y, test.transform.position.x) * Mathf.Rad2Deg);

        float i = 0;

        for (float a = 90 - 180 * course; (90 - 90 * course + a * course) <= 90; a += angleSearchStepGrad * course) {
            Vector2 direction = Quaternion.Euler(0, 0, a) * Vector2.right;
            Debug.DrawLine(Vector2.zero, direction, new Color(1,1,i,1));
            i += .1f;
        }


/*
        // search angle
        while(enemyAngle < 90f) {
            float tga = Mathf.Tan(enemyAngle * Mathf.Deg2Rad);
            lineTo.y = direction * (lineTo.x - selfTransform.position.x) * tga + selfTransform.position.y;

            RaycastHit2D rkHit = Physics2D.Linecast( selfTransform.position, lineTo, mask);
            

            Debug.DrawLine(selfTransform.position, lineTo, Color.green);
            //Debug.DrawLine(enemyTransform.position, rkHit.point, new Color(1,1,0,1));

            // ТОДО не искать угол врага, если цикл отработал сразу

            enemyAngle += angleSearchStepGrad;
            if(!rkHit) break;
        }
*/
        //Debug
        Debug.Break();
        //return enemyAngle;
        return 0;
    }

    void TurnaroundToEnemy(Transform self, Transform target) {
        if (target.position.x > self.position.x && self.localScale.x < 0) gunScript.TurnAround(); 
        if (target.position.x < self.position.x && self.localScale.x > 0) gunScript.TurnAround();
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
