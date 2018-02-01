using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    [SerializeField]
    float walkSpeed = 5f;

    //[SerializeField]
    //float stopForce = 20f;

    [SerializeField]
    ParticleSystem flameThrower;

    [SerializeField]
    Quaternion flameThrowerRotation = Quaternion.Euler(0f, 0f, 0f);
    float lastFTRotation = 0f;

    //Light halo;
    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

        //halo = GetComponent<Light>();
        //halo.enabled = false;

        if (flameThrower == null) 
        {
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particleSystem in particleSystems) 
            {
                if (particleSystem.gameObject.tag == "FlameThrower") 
                {
                    flameThrower = particleSystem;
                    break;
                }
            }
        }

        flameThrower.Stop();

	}
	
	// Update is called once per frame
	void Update() {

        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        float posX = 0f;
        float posY = 0f;

        float rotationX = lastFTRotation;

        if (moveRight)
            posX = walkSpeed * Time.deltaTime;
        else if (moveLeft)
            posX = -walkSpeed * Time.deltaTime;

        if (moveUp)
            posY = walkSpeed * Time.deltaTime;
        else if (moveDown)
            posY = -walkSpeed * Time.deltaTime;

        if (flameThrower.isPlaying) 
        {
            if (moveRight && !moveUp && !moveDown)
                rotationX = 0f;
            else if (moveRight && moveUp && !moveDown)
                rotationX = -45f;
            else if (moveRight && moveDown && !moveUp)
                rotationX = 45f;
            else if (moveLeft && !moveUp && !moveDown)
                rotationX = -180f;
            else if (moveLeft && moveUp && !moveDown)
                rotationX = -135f;
            else if (moveLeft && moveDown && !moveUp)
                rotationX = 135f;
            else if (moveDown && !moveRight && !moveLeft)
                rotationX = 90f;
            else if (moveUp && !moveRight && !moveLeft)
                rotationX = -90f;
            else if (!moveRight && !moveUp && !moveLeft && !moveDown)
                rotationX = lastFTRotation;

            Quaternion rotation = Quaternion.Euler(rotationX, 90f, 0f);

            flameThrower.transform.rotation = rotation;

            if (lastFTRotation != rotationX) 
            {
                lastFTRotation = rotationX;
            }
        }

        transform.position += new Vector3(posX, posY, 0f);

        //float moveHor = Input.GetAxis("Horizontal");
        //float moveVert = Input.GetAxis("Vertical");

        //float posX = 0f;
        //float posY = 0f;

        //if (Mathf.Abs(body.velocity.x) < walkSpeed)
        //{
        //    posX = walkSpeed * moveHor;
        //}

        //if (Mathf.Abs(body.velocity.y) < walkSpeed) 
        //{
        //    posY = walkSpeed * moveVert;
        //}

        //if (Mathf.Abs(moveHor) < 0.01f && Mathf.Abs(body.velocity.x) > 0f)
        //{
        //    posX = -body.velocity.x * stopForce;
        //}

        //if (Mathf.Abs(moveVert) < 0.01f && Mathf.Abs(body.velocity.y) > 0f)
        //{
        //    posY = -body.velocity.y * stopForce;
        //}

        //body.AddForce(new Vector2(posX, posY));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MagicPumpkinPatch")
        {
            flameThrower.Play();
            //halo.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MagicPumpkinPatch")
        {
            flameThrower.Stop();
            //halo.enabled = false;
        }
    }

}
