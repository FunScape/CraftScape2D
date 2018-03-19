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

	// Use this for initialization
	void Start () {
		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;
		equipmentPanel = GameObject.Instantiate (equipmentPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);
		equipmentPanel.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width + 1000f, 0f, 0f);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.G)) {
			ToggleEquipment ();
		}
	}

	void ToggleEquipment()
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

		draggedItem = GameObject.Instantiate(equipmentSlot);
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

	public void OnEndDragEquipedItem(GameObject draggedItemObject)
	{
		if (draggedItem != null)
		{
			Destroy(draggedItem);
		}
		draggedItemObject.GetComponent<EquipmentSlot>().ShowAsEmpty(false);
	}

	public void OnDropInventoryItem(GameObject equipmentSlotObject, GameObject droppedObject)
	{
		Inventory inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventoryController>().inventory;
		InventorySlot inventorySlot = droppedObject.GetComponent<InventorySlot>();
		InventoryItem item = inventory.GetItem(inventorySlot.slotIndex);
		if (CanEquipItemType(item, equipmentSlotObject.name))
		{
			Debug.Log("You equiped item: " + item.title);
			equipmentSlotObject.GetComponent<EquipmentSlot>().EquipItem(item);
			inventory.RemoveItem(inventorySlot.slotIndex);
			GetPlayerInventoryController().UpdateInventoryPanelUI(); 
		}
		else
		{
			Debug.Log("You can't equip that!");
		}
	}
	

	bool CanEquipItemType(InventoryItem item, string equipmentSlotName)
	{
		if (item.types.Contains("equipable"))
		{
			if (equipmentSlotName == "MainHand")
			{
				if (item.types.Contains("weapon")) return true;
			}
			else if (equipmentSlotName == "Chest")
			{
				 if (item.types.Contains("chest")) return true;
			}
			else if (equipmentSlotName == "Ring")
			{
				if (item.types.Contains("ring")) return true;
			}
		}
		return false;
	}

	PlayerInventoryController GetPlayerInventoryController()
	{
		return GameObject.FindWithTag("Player").GetComponent<PlayerInventoryController>();
	}

}
