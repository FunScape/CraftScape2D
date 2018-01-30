﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float walkSpeed = 5f;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool moveRight = Input.GetKey(KeyCode.D);
		bool moveLeft = Input.GetKey(KeyCode.A);
		bool moveUp = Input.GetKey(KeyCode.W);
		bool moveDown = Input.GetKey(KeyCode.S);

		float posX = 0f;
		float posY = 0f;

		if (moveRight) {
			posX = walkSpeed * Time.deltaTime;
		} else if (moveLeft) {
			posX = -walkSpeed * Time.deltaTime;
		}

		if (moveUp) {
			posY = walkSpeed * Time.deltaTime;
		} else if (moveDown) {
			posY = -walkSpeed * Time.deltaTime;
		}

		if (moveUp && !moveRight && !moveLeft) {
			anim.SetTrigger("WalkUp");
		} else if (moveDown && !moveRight && !moveLeft) {
			anim.SetTrigger("WalkDown");
		} else if (moveRight) {
			anim.SetTrigger("WalkSide");
			GetComponent<SpriteRenderer>().flipX = true;
		} else if (moveLeft) {
			anim.SetTrigger("WalkSide");
			GetComponent<SpriteRenderer>().flipX = false;
		} else {
			anim.SetTrigger("Idle");
		}

		transform.position += new Vector3(posX, posY, 0f);

	}
}