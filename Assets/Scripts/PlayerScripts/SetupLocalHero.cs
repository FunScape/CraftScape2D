using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SetupLocalHero : NetworkBehaviour {

    GameObject localPlayer;

	void Start () {
		if (isLocalPlayer)
		{
            GetComponent<HeroController>().enabled = true;
            GetComponent<HeroAnimationBehaviour>().enabled = true;
			CameraFollow.player = gameObject.transform;
		}
		else
		{
            GetComponent<HeroController>().enabled = false;
            GetComponent<HeroAnimationBehaviour>().enabled = false;
		}
	}


}
