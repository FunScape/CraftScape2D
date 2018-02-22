using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class Inventory : MonoBehaviour {

	private GameObject _owner; // _owner: The game object that owns the inventory. Set this value using 'SetInventoryOwner(...)'.
	
	public GameObject inventoryOwner { get { return _owner; } } // inventoryOwner: Public getter for '_owner';

	private string _inventoryFilePath;

	public string inventoryFilePath { get { return _inventoryFilePath; } }

	public GameObject inventorySlotPrefab;

	public GameObject inventoryPanel;

	InventoryTrash inventoryTrash;

	GameObject slotPanel;

	ItemDatabase database;

	public GameObject[] slots;

	public int slotCount = 20;

	void Start()
	{
		database = ItemDatabase.instance;

		slots = new GameObject[slotCount];

		inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
		slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;
		inventoryTrash = inventoryPanel.transform.Find("InventoryTrash").gameObject.GetComponent<InventoryTrash>();
		inventoryTrash.parentInventory = this;

		CreateSlots();
		LoadInventory();
	}

	public void SetOwner(GameObject owner) {
		this._owner = owner;
	}

	public void SetFilePath(string path)
	{
		this._inventoryFilePath = path;
	}

	public void AddItem(GameItem item)
	{
		if (item.maxStackSize > 1)
		{
			int firstEmptySlotIndex = -1;
			bool didIncrementItemStack = false;
			for (var i = 0; i < slots.Length; i++)
			{
				GameItem currentItem = slots[i].GetComponent<Slot>().gameItem;
				if (currentItem == null) 
				{
					if (firstEmptySlotIndex == -1) { firstEmptySlotIndex = i; }
				} 
				else if (currentItem.id == item.id && currentItem.stackSize < currentItem.maxStackSize) 
				{
					currentItem.stackSize++;
					didIncrementItemStack = true;
					slots[i].GetComponent<Slot>().UpdateItemStackLabel();
					break;
				}
			}
			if (firstEmptySlotIndex != -1 && !didIncrementItemStack)
			{
				// item.stackSize = 1;
				slots[firstEmptySlotIndex].GetComponent<Slot>().SetItem(item);
			}
		}
		else
		{
			for (int i = 0; i < slots.Length; i++)
			{
				GameObject slot = slots[i];
				if (slot.GetComponent<Slot>().gameItem == null) {
					slot.GetComponent<Slot>().SetItem(item);
					break;
				}
			}
		}

		UpdateItemInventoryPositions();

		SaveInventory();
	}

	public void UpdateItemInventoryPositions()
	{
		// Update item.inventoryPosition
		for (int i = 0; i < slots.Length; i++)
		{
			Slot slot = slots[i].GetComponent<Slot>();
			if (slot.gameItem != null)
				{ slot.gameItem.inventoryPosition = i; }
		}
	}

	public void AddItemById(int id)
	{
		GameItem item = database.GetItemById(id);
		AddItem(item);
	}

	public void RemoveItem(int slotIndex)
	{
		GameItem item;
		try
		{
			item = slots[slotIndex].GetComponent<Slot>().gameItem;
		}
		catch (System.IndexOutOfRangeException)
		{
			Debug.LogWarning("Tried removing item at index out of range.");
			return;
		}

		if (item.stackSize > 1)
			{ item.stackSize -= 1; }
		else
			{ slots[slotIndex].GetComponent<Slot>().SetItem(null); }

		slots[slotIndex].GetComponent<Slot>().UpdateItemStackLabel();

		SaveInventory();
	}

	public void RemoveItems(int slotIndex)
	{
		GameItem item;
		try {
			item = slots[slotIndex].GetComponent<Slot>().gameItem;
		} catch (System.IndexOutOfRangeException) {
			Debug.LogWarning("Tried removing items at index out of range.");
			return;
		}

		slots[slotIndex].GetComponent<Slot>().SetItem(null);
		slots[slotIndex].GetComponent<Slot>().UpdateItemStackLabel();

		SaveInventory();
	}

	private void CreateSlots()
	{
		// Check for previously created Slots.
		Slot[] previousSlots = slotPanel.GetComponentsInChildren<Slot>();
		// If previousSlots.Length == 0, then create new slots
		if (previousSlots.Length == 0)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				GameObject slot = Instantiate(inventorySlotPrefab);
				slot.GetComponent<Slot>().slotIndex = i;
				slot.transform.SetParent(slotPanel.transform);
				slots[i] = slot;
			}
		}
		// If previousSlots.Length > 0, then have 'this.slots' elements reference the previously created Slot's gameObjects.
		else
		{
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i] = previousSlots[i].gameObject;
				slots[i].GetComponent<Slot>().slotIndex = i;
			}
		}
	}

	public void SaveInventory()
	{
		database.WriteToFile(this);
	}

	public List<GameItem> GetGameItems()
	{
		List<GameItem> items = new List<GameItem>();
		for (int i = 0; i < slots.Length; i++) 
		{ 
			GameItem item = slots[i].GetComponent<Slot>().gameItem; 
			if (item != null)
			{
				items.Add(item);
			}
		}
		return items;
	}

	public void LoadInventory()
	{
		List<GameItem> items = database.ReadFromFile(inventoryFilePath);
		for (var i = 0; i < items.Count; i++)
		{
			slots[items[i].inventoryPosition].GetComponent<Slot>().SetItem(items[i]);
		}
	}

}
