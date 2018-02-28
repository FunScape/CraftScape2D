using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerInventoryController : MonoBehaviour {

	public GameObject inventoryPanelPrefab;

	public GameObject inventorySlotPrefab;

	public GameObject inventoryStackLabelPrefab;

	GameObject inventoryPanel;

	const string inventorySlotsContainerName = "InventorySlotsContainer";

	public Inventory inventory;

	bool showInventory = false;

	bool didOpenInventoryOnce = false;

	Color clearColor { get { Color color = Color.white; color.a = 0f; return color; } }

	float maxSpriteSizeOnDrag = 0.6f;

	// Use this for initialization
	void Start () {
		inventory = Inventory.CreateInstance();
		GameObject inventoryCanvas = GameObject.FindWithTag("InventoryCanvas");
		inventoryPanel = Instantiate(inventoryPanelPrefab, Vector3.zero, Quaternion.identity, inventoryCanvas.transform);
		inventory.SetInventoryFileName(string.Format("inventory-{0}.json", GetComponent<SetupLocalPlayer>().netId.ToString()));
		inventory.LoadInventory();

		LayoutInventory();
	}
	
	void Update () {			

		if (Input.GetKeyDown(KeyCode.B))
		{
			ToggleInventory();
		}

	}

	void LayoutInventory()
	{
		GameObject slotContainer = inventoryPanel.transform.Find(inventorySlotsContainerName).gameObject;
		for (int i = 0; i < this.inventory.size; i++)
		{
			GameObject slot = Instantiate(
				inventorySlotPrefab, 
				Vector3.zero, 
				Quaternion.identity, 
				slotContainer.transform
			);
			slot.GetComponent<InventorySlot>().slotIndex = i;
			GameObject stackCountLabel = Instantiate(
				inventoryStackLabelPrefab, 
				Vector3.zero, 
				Quaternion.identity
			);
			Transform slotItemTransform = slot.transform.Find("InventorySlotItem");
			stackCountLabel.transform.SetParent(slotItemTransform);
			stackCountLabel.transform.localPosition = new Vector3(-4f, 0f, 0f);
			stackCountLabel.GetComponent<Text>().text = "";
		}
	}

	void ToggleInventory()
	{
		showInventory = !showInventory;

		if (!didOpenInventoryOnce) {
			UpdateInventoryPanelUI();
			didOpenInventoryOnce = true;
		}

		float height = Camera.main.pixelHeight;
		float width = Camera.main.pixelWidth;
		
		if (!showInventory)
			width += 1000f;

		inventoryPanel.transform.position = new Vector3(width, height, 0f); 
	}

	void UpdateInventoryPanelUI()
	{
		List<GameObject> slots = GetInventorySlots();

		for (int i = 0; i < inventory.size; i++)
		{
			GameObject slotItem = slots[i].transform.Find("InventorySlotItem").gameObject;
			slotItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
			Image slotItemImage = slotItem.GetComponent<Image>();
			InventoryItem item = inventory.GetItem(i);
			if (item != null)
			{
				slotItemImage.sprite = item.sprite;

				slotItemImage.color = slotItemImage.sprite == null ? clearColor : Color.white;

				if (item.maxStackSize > 1)
					slots[i].GetComponentInChildren<Text>().text = item.stackSize.ToString();
				else
					slots[i].GetComponentInChildren<Text>().text = "";

			}
			else
			{
				slotItemImage.color = clearColor;
				slots[i].GetComponentInChildren<Text>().text = "";
			}

		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name != null)
		{
			InventoryItem item = inventory.FindDatabaseItem(other.gameObject.name);
			if (item != null)
			{
				inventory.AddItem(item);
				Destroy(other.gameObject);
				UpdateInventoryPanelUI();
			}
		}
	}

	List<GameObject> GetInventorySlots()
	{
		GameObject container = inventoryPanel.transform.Find(inventorySlotsContainerName).gameObject;
		List<GameObject> slots = new List<GameObject>();
		foreach (Transform child in container.transform)
		{
			slots.Add(child.gameObject);
		}
		return slots;
	}


	GameObject draggedInventoryItem;

	public GameObject OnBeginDragInventoryItem(int slotIndex)
	{
		List<GameObject> inventorySlots = GetInventorySlots();
		foreach (GameObject slot in inventorySlots)
		{
			slot.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		
		if (inventory.GetItem(slotIndex) != null)
		{
			GameObject draggedSlotItem = inventorySlots[slotIndex].transform.Find("InventorySlotItem").gameObject;
			draggedInventoryItem = GameObject.Instantiate(draggedSlotItem);
			draggedInventoryItem.transform.SetParent(inventoryPanel.transform);
			draggedInventoryItem.GetComponent<Image>().raycastTarget = false;
			Destroy(draggedInventoryItem.GetComponentInChildren<Text>());
		}

		GameObject inventorySlot = inventorySlots[slotIndex];
		GameObject inventorySlotItem = inventorySlot.transform.Find("InventorySlotItem").gameObject;
		inventorySlotItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
		inventorySlotItem.GetComponent<Image>().color = clearColor;
		return inventorySlotItem;
	}
	
	public void OnDragInventoryItem(PointerEventData eventData)
	{
		if (draggedInventoryItem != null)
		{
			draggedInventoryItem.transform.position = eventData.position;
		}
	}

	public void OnEndDragInventoryItem(int slotIndex)
	{
		List<GameObject> inventorySlots = GetInventorySlots();
		foreach (GameObject slot in inventorySlots)
		{
			slot.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}

		if (draggedInventoryItem != null)
		{
			Destroy(draggedInventoryItem);
			draggedInventoryItem = null;
		}
		
		UpdateInventoryPanelUI();
	}

	public void SwapInventorySlots(GameObject from, GameObject to)
	{
		foreach (GameObject slot in GetInventorySlots())
		{
			slot.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}

		int index1 = from.GetComponent<InventorySlot>().slotIndex;
		int index2 = to.GetComponent<InventorySlot>().slotIndex;

		inventory.SwapItems(index1, index2);

		from.transform.Find("InventorySlotItem").gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		to.transform.Find("InventorySlotItem").gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

		from.transform.Find("InventorySlotItem").gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
		to.transform.Find("InventorySlotItem").gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

		from.transform.Find("InventorySlotItem").gameObject.GetComponent<Image>().color = Color.white;
		to.transform.Find("InventorySlotItem").gameObject.GetComponent<Image>().color = Color.white;

		inventory.SaveInventory();
	}

	public void RemoveInventoryItem(GameObject inventorySlot)
	{
		int itemIndex = inventorySlot.GetComponent<InventorySlot>().slotIndex;
		inventory.RemoveItem(itemIndex);
		inventory.SaveInventory();
		UpdateInventoryPanelUI();
	}


}
