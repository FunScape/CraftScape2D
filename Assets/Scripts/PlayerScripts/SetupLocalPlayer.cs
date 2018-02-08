using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SetupLocalPlayer : NetworkBehaviour {

	[SyncVar]
	public bool isBlue = false;

	void Start () {
		if (isLocalPlayer)
		{
			GetComponent<HeroMovement>().enabled = true;
			GetComponent<AnimationBehaviour>().enabled = true;
			CameraFollow.player = this.gameObject.transform;
			GetComponent<SpriteRenderer>().color = Color.blue;
		}
		else
		{
			GetComponent<HeroMovement>().enabled = false;
			GetComponent<AnimationBehaviour>().enabled = false;
		}
	}

	public void MakeBlue(bool state)
	{
		Debug.Log("This got called 1: " + isLocalPlayer.ToString());
		if (!isLocalPlayer) return;
		CmdMakeBlue(state);
	}

	[Command]
	void CmdMakeBlue(bool state)
	{
		Debug.Log("This got called 2.");
		isBlue = state;
		
		RpcMakeBlue(state);
	}

	[ClientRpc]
	void RpcMakeBlue(bool state)
	{
		Debug.Log("Turning Blue");
		GetComponent<SpriteRenderer>().color = Color.blue;
	}

}
