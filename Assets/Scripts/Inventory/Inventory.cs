using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class Inventory : MonoBehaviour {
	private const string inventoryFileName = "/PlayerInventory.json";
	private string inventoryFilePath { get { return Application.streamingAssetsPath + inventoryFileName; } }

	public GameObject inventorySlotPrefab;

	public GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;

	public GameObject[] slots;

	public int slotCount = 20;

	void Start()
	{
		database = GetComponent<ItemDatabase>();

		slots = new GameObject[slotCount];

		inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
		slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;

		CreateSlots();
		LoadInventory();
	}

	public void AddItem(GameItem item)
	{
		foreach (GameObject slot in slots)
		{
			if (slot.GetComponent<Slot>().Item == null) {
				slot.GetComponent<Slot>().SetItem(item);
				break;
			}
		}
		Save();
	}

	public void AddItemById(int id)
	{
		GameItem item = database.GetItemById(id);
		AddItem(item);
	}

	private void CreateSlots()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			GameObject slot = Instantiate(inventorySlotPrefab);
			slots[i] = slot;
			slot.transform.SetParent(slotPanel.transform);
		}
	}

	public void Save()
	{
		database.WriteToJsonFile(this);
	}

	public List<GameItem> GetGameItems()
	{
		List<GameItem> items = new List<GameItem>();
		for (int i = 0; i < slots.Length; i++) 
		{ 
			GameItem item = slots[i].GetComponent<Slot>().Item; 
			if (item != null)
			{
				items.Add(item);
			}
		}
		return items;
	}

	public void LoadInventory()
	{
		List<GameItem> items = database.LoadFromFile(inventoryFilePath);
		for (var i = 0; i < items.Count; i++)
		{
			slots[i].GetComponent<Slot>().SetItem(items[i]);
		}
		Debug.Log("Loaded inventory: " + items.Count.ToString());
	}

}
