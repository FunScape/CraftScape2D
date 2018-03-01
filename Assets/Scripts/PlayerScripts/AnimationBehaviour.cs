using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBehaviour : MonoBehaviour {
    
    Animator leftArmAnimator;
    Animator rightArmAnimator;
    Animator legAnimator;

	void Start () {
        Animator[] playerAnimators = gameObject.GetComponentsInChildren<Animator>();
        
        foreach(Animator item in playerAnimators)
        {
            if(item.gameObject.name == "Legs")
            {
                legAnimator = item;
            }
            else if(item.gameObject.name== "LeftArm")
            {
                leftArmAnimator = item;
            }
            else if(item.gameObject.name== "RightArm")
            {
                rightArmAnimator = item;
            }

        }
    }

    void Update () {
		bool moveRight = Input.GetKey(KeyCode.D);
		bool moveLeft = Input.GetKey(KeyCode.A);
		bool moveUp = Input.GetKey(KeyCode.W);
		bool moveDown = Input.GetKey(KeyCode.S);

		if (moveUp && !moveRight && !moveLeft) 
        {
            //anim.SetTrigger("WalkUp");
        } 
        else if (moveDown && !moveRight && !moveLeft) 
        {
            leftArmAnimator.SetTrigger("WalkDown");
            rightArmAnimator.SetTrigger("WalkDown");
           
			//anim.SetTrigger("WalkDown");
		} 
        else if (moveRight) {
			//anim.SetTrigger("WalkSide");
         //   GetComponent<NetworkAnimationBehaviour>().CmdOnFlipX(true);
		} 
        else if (moveLeft) 
        {
			//anim.SetTrigger("WalkSide");
        //    GetComponent<NetworkAnimationBehaviour>().CmdOnFlipX(false);
		} else 
        {
            leftArmAnimator.SetTrigger("IdleDown");
            rightArmAnimator.SetTrigger("IdleDown");
			//anim.SetTrigger("Idle");
		}
	}


}
