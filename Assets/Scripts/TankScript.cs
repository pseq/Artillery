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

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftWheelObj = transform.GetChild(1).gameObject;
        GameObject rightWheelObj = transform.GetChild(2).gameObject;
        rightWheel = rightWheelObj.GetComponent<WheelJoint2D>();
        leftWheel = leftWheelObj.GetComponent<WheelJoint2D>();
        rightWheelRigid = rightWheelObj.GetComponent<Rigidbody2D>();
        leftWheelRigid = leftWheelObj.GetComponent<Rigidbody2D>();
    }

    public void Move(float dist) {
        // формула вычисления времени из расстояния
        float time = dist/speed;

        //Debug
        //Debug.Break();
        StartCoroutine(MoveCoroutine(time));
    }
    
    public void Aim() {
        float angle = ShootAngleSearch(gameObject, target);
        Debug.Log("TANK MIN ANGLE " + angle);

        float angle2 = ShootAngleSearch(target, gameObject);
        Debug.Log("TANK MIN ANGLE2 " + angle2);
    }

    float ShootAngleSearch(GameObject self, GameObject enemy) {
        LayerMask mask = LayerMask.GetMask("Terrain");
        Transform selfTransform = self.transform;
        Transform enemyTransform = enemy.transform;
        float direction = selfTransform.localScale.x/Mathf.Abs(selfTransform.localScale.x);
        Vector2 lineTo = enemyTransform.position ;
        float enemyAngle = Mathf.Atan2(lineTo.y - selfTransform.position.y, (lineTo.x - selfTransform.position.x) * direction) * Mathf.Rad2Deg;
        while(enemyAngle < 90f) {
            float tga = Mathf.Tan(enemyAngle * Mathf.Deg2Rad);
            lineTo.y = direction * (lineTo.x - selfTransform.position.x) * tga + selfTransform.position.y;

            RaycastHit2D rkHit = Physics2D.Linecast( selfTransform.position, lineTo, mask);
            

            //Debug.Log("TANK RAYCAST COLLISION: " + rkHit.transform);
            Debug.DrawLine(selfTransform.position, lineTo, new Color(1,enemyAngle/10f,enemyAngle/10f,1));
            Debug.DrawLine(enemyTransform.position, rkHit.point, new Color(1,1,0,1));

            if(!rkHit) break;
            enemyAngle += angleSearchStepGrad;
        }


        Debug.Break();
        return enemyAngle;
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
