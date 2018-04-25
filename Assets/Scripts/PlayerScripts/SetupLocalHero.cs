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
			GetComponent<InventoryController>().enabled = true;
			GetComponent<EquipmentController>().enabled = true;
			CameraFollow.player = gameObject.transform;
		}
		else
		{
			GetComponent<InventoryController>().enabled = false;
			GetComponent<EquipmentController>().enabled = false;
            GetComponent<HeroAnimationBehaviour>().enabled = false;
			GetComponent<HeroController>().enabled = false;
		}
	}


}
