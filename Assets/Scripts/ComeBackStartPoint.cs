using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeBackStartPoint : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);   
        Debug.Log("comeback start=============");
        animator.tag = "Untagged";
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        //Debug.Log("update Comeback111111111111111==============");
        if(animator.gameObject.GetComponent<EnemyController>().isComebackStartPoint == true)
       {    
           Debug.Log("Comeback111111111111111==============");
           Vector3 vt = animator.gameObject.GetComponent<EnemyController>().startPoint;
           float speed = animator.gameObject.GetComponent<EnemyController>().returnSpeedDefender;
           animator.transform.position = Vector3.MoveTowards(animator.transform.position, vt, speed * Time.deltaTime);
           //transform.position = Vector3.MoveTowards(transform.position, point, normalSpeedDefender * Time.deltaTime);
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
