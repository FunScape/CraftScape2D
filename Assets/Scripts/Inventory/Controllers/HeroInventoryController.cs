using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroInventoryController : InventoryController
{

    public void SetupInventory(Inventory inventory = null)
    {
        if (inventory == null)
            inventory = Inventory.CreateInstance();

        this.inventory = inventory;
        
        // Get reference to main canvas object
        GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");

        // Get reference to equipment inventory container
        GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;

        // Instantiate inventory panel
        base.inventoryPanel = Instantiate(inventoryPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);

        // Layout/render inventory on canvas
        base.LayoutInventory();
    }

	new void ToggleInventory()
	{
		if (GameManager.GetLocalPlayer().GetComponent<SetupLocalHero> ().isLocalPlayer) {

			if (Input.GetKeyDown (KeyCode.B)) 
			{
				base.ToggleInventory();
			}

		}
	}



}
