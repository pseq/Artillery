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
        // TODO формула вычисления времени из расстояния
        float startPos = transform.position.x;
        Move(changePositionDistance * moveDirection);
        isFirst = false;
        if (Mathf.Abs(startPos - transform.position.x) > 0) return true;
        else return false;
    }
    
    public void Aim() {
        TurnaroundToEnemy(gameObject.transform, target.transform);
        // находим углы для попадания по цели для обоих танков, и берем больший
        float angle = Mathf.Max(ShootAngleSearch(target, gameObject), ShootAngleSearch(gameObject, target));
        Debug.Log("TANK CLCLTED ANGLE " + angle + " " + gameObject.name);

        // Power Calc to Enemy
        lastGunPower = gunPower;
        gunPower =  gunScript.GunPowerToPoint(target.transform.position, angle);
        Debug.Log("TANK CLCLTED GP " + gunPower + " MAX GP " + maxGunPower + " " + gameObject.name);

        if (gunPower <= maxGunPower) {
            // завершаем алгоритм
            AiminOK(angle);
            return;
        } 
        else if (!isFirst) {
            if (gunPower > lastGunPower) {
                if (ChangeMovinDirection()) {
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

        gunScript.transform.eulerAngles = new Vector3(0f,0f,angle + transform.rotation.z);
        Debug.Log("TANK gunScript.transform.eulerAngles " + gunScript.transform.eulerAngles + 
            " angle " + angle + 
            " transform.rotation.z " + transform.rotation.z);
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
        return Random.Range(leftAimPoint, rightAimPoint);
    } 

    float ShootAngleSearch(GameObject self, GameObject enemy) {
        LayerMask mask = LayerMask.GetMask("Terrain");
        Transform selfTransform = self.transform;
        Transform enemyTransform = enemy.transform;
        float direction = Mathf.Sign(selfTransform.localScale.x);
        Vector2 lineTo = enemyTransform.position ;
        float enemyAngle = Mathf.Atan2(lineTo.y - selfTransform.position.y, (lineTo.x - selfTransform.position.x) * direction) * Mathf.Rad2Deg;

        // search angle
        while(enemyAngle < 90f) {
            float tga = Mathf.Tan(enemyAngle * Mathf.Deg2Rad);
            lineTo.y = direction * (lineTo.x - selfTransform.position.x) * tga + selfTransform.position.y;

            RaycastHit2D rkHit = Physics2D.Linecast( selfTransform.position, lineTo, mask);
            

            Debug.DrawLine(selfTransform.position, lineTo, new Color(1,enemyAngle/10f,enemyAngle/10f,1));
            Debug.DrawLine(enemyTransform.position, rkHit.point, new Color(1,1,0,1));

            if(!rkHit) break;
            enemyAngle += angleSearchStepGrad;
        }

        //Debug
        //Debug.Break();
        return enemyAngle;
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
