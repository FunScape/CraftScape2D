using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkAnimationBehaviour : NetworkBehaviour {

	[SyncVar (hook = "OnFlipX")]
	public bool flipX = false;

	// Use this for initialization
	void Start () {
		
	}

	void OnFlipX(bool flip)
	{
		if (!isServer)
			return;
			
		RpcFlipX(flip);
	}

	[ClientRpc]
	void RpcFlipX(bool flip)
	{
		this.gameObject.GetComponent<SpriteRenderer>().flipX = flip;
	}
	

}
