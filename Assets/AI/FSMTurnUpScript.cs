using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTurnUpScript : StateMachineBehaviour
{
    public float upsideReturnUp = 10f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform TankTransform = animator.transform;

        TankTransform.position = (Vector2)TankTransform.position + Vector2.up * upsideReturnUp;
        TankTransform.eulerAngles = Vector2.zero;
        
        animator.ResetTrigger("isTurnedOver");
    }
}
