using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBehaviour : MonoBehaviour {

	Animator anim;

	public Toggle blueToggle;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		// blueToggle.onValueChanged.AddListener(MakeBlue);
	}
	
	// Update is called once per frame
	void Update () {
		bool moveRight = Input.GetKey(KeyCode.D);
		bool moveLeft = Input.GetKey(KeyCode.A);
		bool moveUp = Input.GetKey(KeyCode.W);
		bool moveDown = Input.GetKey(KeyCode.S);

		if (moveUp && !moveRight && !moveLeft) {
			anim.SetTrigger("WalkUp");
		} else if (moveDown && !moveRight && !moveLeft) {
			anim.SetTrigger("WalkDown");
		} else if (moveRight) {
			anim.SetTrigger("WalkSide");
			
			GetComponent<SpriteRenderer>().flipX = true;
			transform.localScale.Set(-1f, 1f, transform.localScale.z);
		} else if (moveLeft) {
			anim.SetTrigger("WalkSide");
			GetComponent<SpriteRenderer>().flipX = false;
			// GetComponent<NetworkAnimationBehaviour>().flipX = false;
		} else {
			anim.SetTrigger("Idle");
		}
	}

	public void MakeBlue(bool state)
	{
		// Debug.Log("Clicked blue toggle");
		GetComponent<SetupLocalPlayer>().MakeBlue(state);
	}


}
