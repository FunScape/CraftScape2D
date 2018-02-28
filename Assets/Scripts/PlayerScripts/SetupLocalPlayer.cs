using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SetupLocalPlayer : NetworkBehaviour {

    GameObject localPlayer;

	void Start () {
		if (isLocalPlayer)
		{
            GetComponent<PlayerController>().enabled = true;
//			GetComponent<AnimationBehaviour>().enabled = true;
			CameraFollow.player = gameObject.transform;
		}
		else
		{
            GetComponent<PlayerController>().enabled = false;
			GetComponent<AnimationBehaviour>().enabled = false;
		}
	}


}
