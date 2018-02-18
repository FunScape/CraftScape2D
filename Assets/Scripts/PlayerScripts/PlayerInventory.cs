using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	public Inventory inventoryPrefab;
	Inventory inventory;

	Vector3 inventoryOffscreenPosition = new Vector3(1000f, 1000f, 0f);
	Vector3 inventoryOnScreenPosition = new Vector3(-20f, -20f, 0f);

	private bool _displayInventory = false;
	public bool displayInventory {  
		get { return _displayInventory; } 
		set { _displayInventory = value; inventory.inventoryPanel.transform.position = _displayInventory ? inventoryOnScreenPosition : inventoryOffscreenPosition; } 
	}

	// Use this for initialization
	void Start () {
		inventory = GameObject.Instantiate(inventoryPrefab);
		inventoryOnScreenPosition = inventory.inventoryPanel.transform.position;
		displayInventory = false;
	}
	
	bool isPressingB = false;

	// Update is called once per frame
	void Update () {
		bool pressedB = Input.GetKey(KeyCode.B);

		if (pressedB && !isPressingB)
		{
			isPressingB = true;
			displayInventory = !displayInventory;
		}
		else if (!pressedB && isPressingB)
		{
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
