using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 5f;

    Rigidbody2D body;

    [SerializeField]
    ParticleSystem magicEffect;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        magicEffect.Stop();
	}
	
	// Update is called once per frame
	void Update () {

        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);

        float posX = 0f;
        float posY = 0f;

        if(moveUp)
        {
            posY = moveSpeed;
        }
        else if(moveDown)
        {
            posY = -moveSpeed;
        }

        if(moveRight)
        {
            posX = moveSpeed;
        }
        else if(moveLeft)
        {
            posX = -moveSpeed;
        }

        Vector2 moveVect = new Vector2(posX, posY);
        moveVect.Normalize();
        moveVect *= moveSpeed;
        moveVect *= Time.deltaTime;

        transform.Translate(moveVect);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MagicPatch")
        {
            magicEffect.Play();
        }
    }
}
