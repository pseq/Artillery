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

    public void UpsideDown() {
        animator.SetTrigger("isTurnedOver");
    }
    
    public void CantMove() {
        animator.SetTrigger("cantMove");
    }

    public void CantShoot() {
        animator.SetTrigger("cantReachShot");
    }
    public void Shooted() {
        animator.SetBool("wasShooted", true);
    }

    public void EndTurn() {
        animator.SetBool("wasShooted", false);
        animator.SetTrigger("endTurn");
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
