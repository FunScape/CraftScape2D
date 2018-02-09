using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkAnimationBehaviour : NetworkBehaviour {

	[SyncVar (hook = "OnFlip")]
	public bool s_flipX = false;
	public bool m_flipX = false;

	// Use this for initialization
	void Start () {
		
	}

	void OnFlip(bool flip) {
		// GetComponent<SpriteRenderer>().flipX = flip;
		if (s_flipX != flip) 
		{
			s_flipX = flip;
			GetComponent<SpriteRenderer>().flipX = s_flipX;
		}
	}

	[Command]
	public void CmdFlip(bool flip) {
		OnFlip(flip);
	}
	

}
