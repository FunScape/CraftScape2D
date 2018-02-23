using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

	public Inventory inventoryPrefab;

	public string inventoryFileName = "/PlayerInventory.json";
	public string inventoryFilePath { get { return Application.streamingAssetsPath + "/PlayerInventory" + inventoryFileName; } }

	Inventory inventory;

	bool displayInventory = false;

	void Start () {
		inventory = GetComponent<Inventory>();
		inventory.SetOwner(this.gameObject);
		inventory.SetFilePath(this.inventoryFilePath);
	}
	
	bool isPressingB = false;
	
	void Update () {
		bool inventoryKey = Input.GetKey(KeyCode.B);
		OpenInventory(inventoryKey);
	}

	void OpenInventory(bool shouldOpen)
	{
		if (shouldOpen && !isPressingB) {
			isPressingB = true;
			displayInventory = !displayInventory;

			if (displayInventory)
			{
				// Moves inventory panel to top right corner of screen - CB 2/19
				inventory.ShowInventory(true);
			}
			else
			{
				// move inventory panel offscreen - CB 2/19
				inventory.ShowInventory(false);
			}

			foreach (GameObject slot in inventory.slots)
			{
				slot.GetComponent<Slot>().UpdateItemImage();
			}

		} else if (!shouldOpen && isPressingB) {
			isPressingB = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "apple")
		{
			inventory.AddItemById(1);
		}
		else if (other.gameObject.name == "bag")
		{
			inventory.AddItemById(2);
		}
		else if (other.gameObject.name == "axe")
		{
			inventory.AddItemById(3);
		}

	}

	public void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 150, 230, 317-265));
		GUILayout.TextField(this.inventoryFileName);
		GUILayout.EndArea();
	}
	

}
