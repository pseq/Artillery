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
    CommonTankScripts common;
    TankMoveScript moveScript;


    // Start is called before the first frame update
    void Start()
    {
        tankRigid = GetComponent<Rigidbody2D>();
        if (isPlayer) animator.SetTrigger("isPlayer");
        common = GetComponent<CommonTankScripts>();
        moveScript = GetComponent<TankMoveScript>();
    }

    public void UpsideDown() {
        animator.SetTrigger("isTurnedOver");
    }
    
    public void CantMove() {
        animator.SetTrigger("cantMove");
    }

    public void Moving(bool moving) {
        animator.SetBool("moving", moving);
    }

    public void CantShoot() {
        animator.SetTrigger("cantReachShot");
    }
    public void Shooted() {
        //animator.SetBool("wasShooted", false); //reset on move and end turn
        animator.SetBool("wasShooted", true);
    }

    public void EndTurn() {
        //Debug.Log("AIscr END TURN " + name);
        animator.SetTrigger("endTurn");
    }

    public void MyTurn() {
        Debug.Log("AIscr MY TURN " + name);
        animator.SetTrigger("myTurn");
    }

    public void ShootOK() {
        //Debug.Log(gameObject.name + " shootOK");
        animator.SetTrigger("shootOK");
    }

    public void CritHealth(bool crit) {
        animator.SetBool("healthCrit", crit);
    }

    public void EnemyIsWeak(bool weakness) {
        animator.SetBool("healthCritEnemy", weakness);
    }

    public void GoToCover() {
        float coverPos = GetComponent<CoveringScript>().GetCover(common.DirAwayEnemy());
        if(coverPos != 0) moveScript.Move(coverPos);
        else {
            coverPos = GetComponent<CoveringScript>().GetCover(common.DirToEnemy());
            if(coverPos != 0) moveScript.Move(coverPos);
            else animator.SetTrigger("noCovers");
        }

    }

    public void MoveAwayEnemy() {
        moveScript.Move(common.DirAwayEnemy());
    }

    public void MoveToEnemy() {
        moveScript.Move(common.DirToEnemy());
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
