using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAnimationBehaviour : MonoBehaviour {

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
			this.anim.SetTrigger("WalkUp");
		} 
        else if (moveDown && !moveRight && !moveLeft) 
        {
			this.anim.SetTrigger("WalkDown");
		} 
        else if (moveRight) {
			this.anim.SetTrigger("WalkSide");
            GetComponent<NetworkHeroAnimationBehaviour>().CmdOnFlipX(true);
		} 
        else if (moveLeft) 
        {
			this.anim.SetTrigger("WalkSide");
            GetComponent<NetworkHeroAnimationBehaviour>().CmdOnFlipX(false);
		} else 
        {
			this.anim.SetTrigger("Idle");
		}
	}


}
