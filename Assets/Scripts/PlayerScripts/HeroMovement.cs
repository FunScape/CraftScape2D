using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{

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

    HashSet<KeyCode> keysPressed = new HashSet<KeyCode>();

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        //halo = GetComponent<Light>();
        //halo.enabled = false;

        if (flameThrower == null)
        {
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.gameObject.tag == "FlameThrower")
                {
                    flameThrower = ps;
                    break;
                }
            }
        }

        flameThrower.Stop();

    }

    // Update is called once per frame
    void Update()
    {

        Move();
        FlameThrowerBehavior();
    }

    enum FTState
    {
        On, Off
    }

    //FTState flameThrowerState = FTState.Off;
    bool isPressingSB = false;

    void FlameThrowerBehavior()
    {
        if (Input.GetKey(KeyCode.Space) && flameThrower.isStopped && !isPressingSB)
        {
            isPressingSB = true;
            flameThrower.Play();
            Debug.Log("Starting flame thrower");
        }
        else if (Input.GetKey(KeyCode.Space) && flameThrower.isPlaying && !isPressingSB)
        {
            isPressingSB = true;
            flameThrower.Stop();
            Debug.Log("Stopping flame thrower");
        }
        else if (!Input.GetKey(KeyCode.Space) && isPressingSB)
        {
            isPressingSB = false;
        }

        if (flameThrower.isPlaying)
        {
            float rotationX = lastFTRotation;

            bool moveRight = Input.GetKey(KeyCode.D);
            bool moveLeft = Input.GetKey(KeyCode.A);
            bool moveUp = Input.GetKey(KeyCode.W);
            bool moveDown = Input.GetKey(KeyCode.S);

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

            if (Mathf.Abs(lastFTRotation - rotationX) > 0.0001f)
            {
                lastFTRotation = rotationX;
            }
        }

    }

    private void Move()
    {
        //bool moveRight = Input.GetKey(KeyCode.D);
        //bool moveLeft = Input.GetKey(KeyCode.A);
        //bool moveUp = Input.GetKey(KeyCode.W);
        //bool moveDown = Input.GetKey(KeyCode.S);

        float posX = Input.GetAxis("Horizontal");
        float posY = Input.GetAxis("Vertical");

        //float posX = 0f;
        //float posY = 0f;

        //if (moveRight)
        //{
        //    posX = walkSpeed * Time.deltaTime;
        //}
        //else if (moveLeft)
        //{
        //    posX = -walkSpeed * Time.deltaTime;
        //}

        //if (moveUp)
        //{
        //    posY = walkSpeed * Time.deltaTime;
        //}
        //else if (moveDown)
        //{
        //    posY = -walkSpeed * Time.deltaTime;
        //}

        Vector2 position = new Vector2(posX, posY);
        if (position.magnitude > 1.0f ) 
        {
            position = position.normalized;
        }
        position = new Vector2(position.x + transform.position.x, position.y + transform.position.y);
        position *= walkSpeed;
        position *= Time.deltaTime;

        body.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.bodyType.Equals(RigidbodyType2D.Static))
        {
            body.velocity.Set(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MagicPumpkinPatch")
        {
            //flameThrower.Play();
            //halo.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MagicPumpkinPatch")
        {
            //flameThrower.Stop();
            //halo.enabled = false;
        }
    }

}
