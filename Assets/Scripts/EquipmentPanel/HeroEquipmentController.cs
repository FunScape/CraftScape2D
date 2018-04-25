using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroEquipmentController : EquipmentController
{

	void ToggleEquipment()
	{
		if (GameObject.FindWithTag ("Player").GetComponent<SetupLocalHero> ().isLocalPlayer) {

			if (Input.GetKeyDown (KeyCode.B)) 
			{
				base.ToggleEquipment();
			}

		}
	}

}
