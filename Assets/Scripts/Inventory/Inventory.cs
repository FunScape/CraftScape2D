using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

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

		// AddItemById(1);
		// AddItemById(2);
		// AddItemById(3);

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

	void OnDisable()
	{

	}

	void OnEnable()
	{
		inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
		slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;		
	}
	

}
