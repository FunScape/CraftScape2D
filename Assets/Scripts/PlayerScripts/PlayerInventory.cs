using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

	public Inventory inventoryPrefab;
	Inventory inventory;

	bool displayInventory = false;

	void Start () {
		inventory = GameObject.Instantiate(inventoryPrefab);
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

			float height = Camera.main.pixelHeight;
			float width = Camera.main.pixelWidth;

			if (displayInventory)
			{
				// Moves inventory panel to top right corner of screen - CB 2/19
				inventory.inventoryPanel.transform.position = new Vector3(width, height, 0f); 
			}
			else
			{
				// move inventory panel offscreen - CB 2/19
				inventory.inventoryPanel.transform.position = new Vector3(width + 1000f, height, 0); 
			}

			foreach (GameObject slot in inventory.slots)
			{
				slot.GetComponent<Slot>().SetEnabledIfItemExists();
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

}
