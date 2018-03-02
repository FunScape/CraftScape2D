using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class Inventory : ScriptableObject {

	protected string inventoryFileName { get; set; }

	protected string inventoryFileDirectory { get { return Application.streamingAssetsPath + "/SaveData/inventories/"; }}

	protected Database database;

	public const int DEFAULT_INVENTORY_SIZE = 16;

	InventoryItem[] items;

	public int size { get { return this.items.Length; } }

	public static Inventory CreateInstance(int size=DEFAULT_INVENTORY_SIZE)
	{
		Inventory inventory = ScriptableObject.CreateInstance("Inventory") as Inventory;
		inventory.Init(size);
		return inventory;
	}

	public void Init(int size)
	{
		if (size == 0)
			throw new System.Exception("Inventory size must be greater than 0.");
		this.items = new InventoryItem[size];
		this.database = new Database();
	}

	public void SetInventoryFileName(string name) {
		this.inventoryFileName = name;
	}

	public string GetInventoryFilePath() {
		return inventoryFileDirectory + this.inventoryFileName;
	}

	public InventoryItem GetItem(int index)
	{
		return this.items[index];
	}

	public void SetItem(int index, InventoryItem item)
	{
		this.items[index] = item;
	}

	public void SwapItems(int index1, int index2)
	{
		InventoryItem temp = this.items[index1];
		this.items[index1] = this.items[index2];
		this.items[index2] = temp;
	}

	public InventoryItem FindDatabaseItem(string name)
	{
		return database.GetItem(name);
	}

	public void AddItem(InventoryItem item)
	{
		bool didFindMatchingItem = false;

		for (int i = 0; i < items.Length; i++)
		{
			InventoryItem currentItem = items[i];

			if (currentItem == null)
				continue;

			if (currentItem.id == item.id && currentItem.stackSize < currentItem.maxStackSize)
			{
				currentItem.stackSize++;
				didFindMatchingItem = true;
				break;
			}
		}

		if (!didFindMatchingItem)
		{
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] == null)
				{
					items[i] = item.Clone();
					items[i].inventoryPosition = i;
					break;
				}
			}
		}

		SaveInventory();
	}

	public void AddItem(int id)
	{
		AddItem(database.GetItem(id));
	}

	public void AddItem(string name)
	{
		InventoryItem item = database.GetItem(name);
		if (item != null)
		{
			Debug.LogFormat("Picked up item: {0}", name);
			AddItem(item);
		}
	}

	public void RemoveItem(InventoryItem item)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i] != null && this.items[i].id == item.id)
			{
				this.items[i] = null;
				break;
			}
		}
	}

	public void RemoveItem(int slotIndex)
	{
		this.items[slotIndex] = null;
	}

	public void RemoveItems(int slotIndex)
	{

	}

	public void SaveInventory()
	{
		// Update inventory positions
		for (int i = 0; i < this.items.Length; i++)
		{
			if (items[i] != null)
				items[i].inventoryPosition = i;
		}
		string path = GetInventoryFilePath();
		this.database.WriteToFile(path, this.items);
	}

	public List<InventoryItem> GetGameItems()
	{
		return null;
	}

	public void LoadInventory()
	{
		List<InventoryItem> savedItems = database.ReadFromFile(GetInventoryFilePath());
		for (int i = 0; i < savedItems.Count; i++)
		{
			this.items[savedItems[i].inventoryPosition] = savedItems[i];
		}
	}

}
