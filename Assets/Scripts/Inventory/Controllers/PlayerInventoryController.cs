using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerInventoryController : InventoryController {

	// Use this for initialization
	void Start () {
		inventory = Inventory.CreateInstance();
		GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");
		GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;
		inventoryPanel = Instantiate(inventoryPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);
		// inventory.SetInventoryFileName(string.Format("inventory-{0}.json", GetComponent<SetupLocalPlayer>().netId.ToString()));
		// inventory.LoadInventory();

		LayoutInventory();
	}

}
