using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBehaviour_Hero : MonoBehaviour {

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		bool moveRight = Input.GetKey(KeyCode.D);
		bool moveLeft = Input.GetKey(KeyCode.A);
		bool moveUp = Input.GetKey(KeyCode.W);
		bool moveDown = Input.GetKey(KeyCode.S);

		if (moveUp && !moveRight && !moveLeft) 
        {
			anim.SetTrigger("WalkUp");
		} 
        else if (moveDown && !moveRight && !moveLeft) 
        {
			anim.SetTrigger("WalkDown");
		} 
        else if (moveRight) {
			anim.SetTrigger("WalkSide");
            GetComponent<NetworkAnimationBehaviour>().CmdOnFlipX(true);
		} 
        else if (moveLeft) 
        {
			anim.SetTrigger("WalkSide");
            GetComponent<NetworkAnimationBehaviour>().CmdOnFlipX(false);
		} else 
        {
			anim.SetTrigger("Idle");
		}
	}


}
