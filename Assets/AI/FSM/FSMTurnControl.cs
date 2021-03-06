﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTurnControl : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //если ход противника - установить у него в FSM флажок myTurn
        animator.GetComponent<CommonTankScripts>().EnemyFSMTurn();
        
        animator.SetBool("wasShooted", false);
        animator.SetBool("shootStarted", false);
        animator.SetBool("shootEnded", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
   // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
        // Implement code that processes and affects root motion
       //Debug.Log("MOVE"); 

   // }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
