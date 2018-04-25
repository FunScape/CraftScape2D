using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour {

	bool showEquipmentPanel = false;

	bool didOpenEquipmentPanelOnce = false;

	public GameObject equipmentPanelPrefab;

	GameObject equipmentPanel;

	private Equipment _equipment;
	public Equipment equipment {
		get { return _equipment; }
		set { _equipment = value; UpdateEquipmentPanelUI(); }
	}

	EquipmentSlot Ring { get { return GetEquipmentSlot("Ring"); } }
	EquipmentSlot Neck { get { return GetEquipmentSlot("Neck"); } }
	EquipmentSlot Head { get { return GetEquipmentSlot("Head"); } }
	EquipmentSlot Chest { get { return GetEquipmentSlot("Chest"); } }
	EquipmentSlot MainHand { get { return GetEquipmentSlot("MainHand"); } }
	EquipmentSlot Back { get { return GetEquipmentSlot("Back"); } }
	EquipmentSlot Hands { get { return GetEquipmentSlot("Hands"); } }
	EquipmentSlot Feet { get { return GetEquipmentSlot("Feet"); } }
	EquipmentSlot Legs { get { return GetEquipmentSlot("Legs"); } }

	// Use this for initialization
	void Start () {
		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;
		equipmentPanel = Instantiate (equipmentPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);
		equipmentPanel.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width + 1000f, 0f, 0f);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.G)) {
			
			if (GameManager.GetLocalPlayer().GetComponent<SetupLocalHero>().isLocalPlayer) {

				if (Input.GetKeyDown (KeyCode.B)) 
				{
					ToggleEquipment();
				}

			}
		}


	}
	
	void OnApplicationQuit()
	{
		equipment.Save();		
	}

	protected void ToggleEquipment()
	{
		showEquipmentPanel = !showEquipmentPanel;

		if (!didOpenEquipmentPanelOnce) {
			UpdateEquipmentPanelUI();
			didOpenEquipmentPanelOnce = true;
		}

		float height = 0f;
		float width = -Screen.width / 2f + equipmentPanel.GetComponent<RectTransform>().rect.width;

		if (!showEquipmentPanel)
			width = Screen.width + equipmentPanel.GetComponent<RectTransform>().rect.width;

		equipmentPanel.transform.localPosition = new Vector3(width, height, 0f); 
	}

	void UpdateEquipmentPanelUI()
	{
		Ring.EquipItem(equipment.Ring);
		Neck.EquipItem(equipment.Neck);
		Head.EquipItem(equipment.Head);
		Chest.EquipItem(equipment.Chest);
		MainHand.EquipItem(equipment.MainHand);
		Back.EquipItem(equipment.Back);
		Hands.EquipItem(equipment.Hands);
		Feet.EquipItem(equipment.Feet);
		Legs.EquipItem(equipment.Legs);
	}

	GameObject draggedItem;

	public void OnBeginDragEquipedItem(GameObject equipmentSlot)
	{
		// Set the inventoryPanel's sibling index to the equipmentPanel's 
		// sibling index, so images belonging to the inventoryPanel
		// will render on top of the equipment panel game object.
		GameObject inventoryPanel = GameObject.FindWithTag("InventoryPanel");
		if (inventoryPanel.transform.GetSiblingIndex() > equipmentPanel.transform.GetSiblingIndex())
		{
			equipmentPanel.transform.SetSiblingIndex(inventoryPanel.transform.GetSiblingIndex());
		}

		draggedItem = Instantiate(equipmentSlot);
		draggedItem.transform.SetParent(equipmentPanel.transform);
		draggedItem.GetComponent<Image>().raycastTarget = false;

		equipmentSlot.GetComponent<EquipmentSlot>().ShowAsEmpty(true);
	}


	public void OnDragEquipedItem(PointerEventData eventData)
	{
		if (draggedItem != null)
		{
			draggedItem.transform.position = eventData.position;
		}
	}

	public void OnEndDragEquipedItem(GameObject draggedItemObject, string slotName)
	{
		if (draggedItem != null)
		{
			Destroy(draggedItem);
		}
		draggedItemObject.GetComponent<EquipmentSlot>().ShowAsEmpty(false);
		RemoveItem(slotName);
		GetInventoryController().inventory.Save();
	}

	public void OnDropGameItem(GameObject equipmentSlotObject, GameObject droppedObject)
	{
		Inventory inventory = GameManager.GetLocalPlayer().GetComponent<InventoryController>().inventory;
		InventorySlot inventorySlot = droppedObject.GetComponent<InventorySlot>();
		GameItem item = inventory.GameItems[inventorySlot.slotIndex];

		if (equipmentSlotObject.name == "Ring" && item.Types.Contains("ring"))
			equipment.Ring = item;
		else if (equipmentSlotObject.name == "Neck" && item.Types.Contains("neck"))
			equipment.Neck = item;
		else if (equipmentSlotObject.name == "Head" && item.Types.Contains("head"))
			equipment.Head = item;
		else if (equipmentSlotObject.name == "Chest" && item.Types.Contains("chest"))
			equipment.Chest = item;
		else if (equipmentSlotObject.name == "MainHand" && (item.Types.Contains("weapon") || item.Types.Contains("mainHand")))
			equipment.MainHand = item;
		else if (equipmentSlotObject.name == "Back" && item.Types.Contains("back"))
			equipment.Back = item;
		else if (equipmentSlotObject.name == "Hands" && item.Types.Contains("hands"))
			equipment.Hands = item;
		else if (equipmentSlotObject.name == "Feet" && item.Types.Contains("feet"))
			equipment.Feet = item;
		else if (equipmentSlotObject.name == "Legs" && item.Types.Contains("legs"))
			equipment.Legs = item;
		else {
			Debug.Log("You can't equip that!");
			return;
		}

		inventory.RemoveItem(inventorySlot.slotIndex);
		GetInventoryController().UpdateInventoryPanelUI(); 
		UpdateEquipmentPanelUI();

		if (equipment.Dirty)
		{
			equipment.Save();
		}
	}

    InventoryController GetInventoryController()
	{
		return GameManager.GetLocalPlayer().GetComponent<InventoryController>();
	}

	EquipmentSlot GetEquipmentSlot(string name)
	{
		GameObject equipmentPanel = GameObject.FindWithTag ("EquipmentPanel");
		EquipmentSlot[] slots = equipmentPanel.GetComponentsInChildren<EquipmentSlot>();
		foreach (EquipmentSlot slot in slots)
		{
			if (slot.gameObject.name == name)
				return slot;
		}
		return null;
	}

	void RemoveItem(string slotName)
	{
		if (slotName == "Ring")
			equipment.Ring = null;
		else if (slotName == "Neck")
			equipment.Neck = null;
		else if (slotName == "Head")
			equipment.Head = null;
		else if (slotName == "Chest")
			equipment.Chest = null;
		else if (slotName == "MainHand")
			equipment.MainHand = null;
		else if (slotName == "Back")
			equipment.Back = null;
		else if (slotName == "Hands")
			equipment.Hands = null;
		else if (slotName == "Feet")
			equipment.Feet = null;
		else if (slotName == "Legs")
			equipment.Legs = null;
		equipment.Save();
	}

}
