using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followD : StateMachineBehaviour {

    private GameObject[] enemies;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
       enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)    {
        foreach (GameObject enemy in enemies){
            RaycastHit2D hit = Physics2D.Raycast(
                animator.gameObject.transform.position,
                enemy.transform.position - animator.gameObject.transform.position,
                1000
            );
            if (hit.collider != null && hit.collider.gameObject == enemy){
                animator.SetBool("isFiring", true);
            }
        }
    }
}
