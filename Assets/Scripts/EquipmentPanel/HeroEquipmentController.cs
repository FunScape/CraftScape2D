using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroEquipmentController : EquipmentController
{

	new void ToggleEquipment()
	{
		if (GameManager.GetLocalPlayer().GetComponent<SetupLocalHero> ().isLocalPlayer) {

			if (Input.GetKeyDown (KeyCode.B)) 
			{
				base.ToggleEquipment();
			}

		}
	}

}
