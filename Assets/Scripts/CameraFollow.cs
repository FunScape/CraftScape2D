using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	static public Transform player;
	public float cameraSpeed = 75;

	float zOffset = -20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player)
		{
			Vector3 playerPos = player.transform.position;
			playerPos.z = zOffset;
			this.transform.position = Vector3.Lerp(this.transform.position, playerPos, Time.deltaTime * cameraSpeed * 0.1f);
		}
	}
}
