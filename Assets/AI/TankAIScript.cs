using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIScript : MonoBehaviour
{
    public float framesUpsideCheck = 10;
    public Animator animator;
    public float upsideReturnUp = 10f;
    public bool isPlayer = false;
    Rigidbody2D tankRigid;


    // Start is called before the first frame update
    void Start()
    {
        tankRigid = GetComponent<Rigidbody2D>();
        if (isPlayer) animator.SetTrigger("isPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        // периодически проверяем, не перевернулся ли танк
        if (Time.frameCount % framesUpsideCheck == 0) {
	    //UpsideCheck();
	    }
    }

    void UpsideCheck() {
        if (
            Mathf.Abs(Mathf.DeltaAngle(0, transform.eulerAngles.z)) > 90 && 
            Mathf.Abs(tankRigid.angularVelocity) < .1f
            ) animator.SetTrigger("isTurnedOver");
    }

    public void UpsideDown() {
Debug.Log("UpsideDown");
        animator.SetTrigger("isTurnedOver");
    }

    // вызывается в анимации
    public void TurnUp() {
        transform.position = (Vector2)transform.position + Vector2.up * upsideReturnUp;
        transform.eulerAngles = Vector2.zero;
        animator.ResetTrigger("isTurnedOver");
    }

    // вызывается в анимации
    public void FreezeUnfreeze() {
        if (tankRigid.constraints != RigidbodyConstraints2D.None) tankRigid.constraints = RigidbodyConstraints2D.None;
        else tankRigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
