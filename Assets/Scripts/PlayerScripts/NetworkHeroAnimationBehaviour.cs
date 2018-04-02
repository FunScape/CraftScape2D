using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHeroAnimationBehaviour : NetworkBehaviour {

    [SyncVar(hook = "OnFlipX")]
    bool flipX = false;

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            GetComponent<HeroAnimationBehaviour>().enabled = true;
        }
        else
        {
            GetComponent<HeroAnimationBehaviour>().enabled = false;
        }
	}
	
    // this function does something
    void OnFlipX(bool flip)
    {
        if (flip != flipX)
        {
            flipX = flip;
			GetComponent<SpriteRenderer>().flipX = flipX;
        }
    }

    [Command]
    public void CmdOnFlipX(bool flip)
    {
        OnFlipX(flip);
    }

}
