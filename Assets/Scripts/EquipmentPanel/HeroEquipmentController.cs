using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroEquipmentController : EquipmentController
{
    new void Update()
    {
        base.Update();
        
        if (equipment != null && equipment.Feet != null)
		{
			GameManager.instance.LocalPlayer().GetComponent<HeroController>().walkSpeed = 10f;
		} else {
			GameManager.instance.LocalPlayer().GetComponent<HeroController>().walkSpeed = 5f;
		}
    }
}
