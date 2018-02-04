using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

	void Start () {
		if (isLocalPlayer)
		{
			GetComponent<HeroMovement>().enabled = true;
			CameraFollow.player = this.gameObject.transform;
		}
		else
		{
			GetComponent<HeroMovement>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
