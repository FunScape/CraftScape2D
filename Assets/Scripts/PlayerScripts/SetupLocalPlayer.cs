using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar (hook="OnChangeMakeBlue")]
	public bool isBlue = false;
    bool m_isBlue = false;

    GameObject localPlayer;

	void Start () {
		if (isLocalPlayer)
		{
			GetComponent<HeroMovement>().enabled = true;
			GetComponent<AnimationBehaviour>().enabled = true;
			CameraFollow.player = this.gameObject.transform;
		}
		else
		{
			GetComponent<HeroMovement>().enabled = false;
			GetComponent<AnimationBehaviour>().enabled = false;
		}
	}

    void OnChangeMakeBlue(bool state) {
        Color color = state ? Color.blue : Color.white;
        Debug.Log("Making blue... " + color.ToString());
        GetComponent<SpriteRenderer>().color = color;
    }

    void MakeBlue(bool state) {
        this.isBlue = state;
	}

    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            bool state = GUI.Toggle(new Rect(25, 15, 35, 25), m_isBlue, "Set");
            if (state != m_isBlue)
            {
                m_isBlue = state;            
				CmdMakeBlue(m_isBlue);
            }
        }
    }

    [Command]
	void CmdMakeBlue(bool toggle) {
        isBlue = toggle;
        Color color = isBlue ? Color.blue : Color.white;
        GetComponent<SpriteRenderer>().color = color;
	}

}
