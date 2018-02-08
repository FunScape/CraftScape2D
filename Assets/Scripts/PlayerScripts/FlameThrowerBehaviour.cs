using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour {

	[SerializeField]
	private ParticleSystem flameThrower;

	bool isPressingSB = false;
	float lastFTRotation;

	// Use this for initialization
	void Start () {
		flameThrower = GetComponent<ParticleSystem>();

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
	void Update () {
		FlameThrowerBehavior();
	}

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
}
