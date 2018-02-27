using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerInventoryController : MonoBehaviour {

	public GameObject inventoryPanelPrefab;

	public GameObject inventorySlotPrefab;

	GameObject inventoryPanel;

	const string inventorySlotsContainerName = "InventorySlotsContainer";

	public Inventory inventory;

	bool showInventory = false;

	bool didOpenInventoryOnce = false;

	Color clearColor { get { Color color = Color.white; color.a = 0f; return color; } }

	float maxSpriteSizeOnDrag = 1.0f;

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
			showInventory = !showInventory;
			OpenInventory(showInventory);
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
		}
	}

	void OpenInventory(bool show)
	{
		if (!didOpenInventoryOnce) {
			UpdateInventoryPanelUI();
			didOpenInventoryOnce = true;
		}

		float height = Camera.main.pixelHeight;
		float width = Camera.main.pixelWidth;
		
		if (!show)
			width += 1000f;

		inventoryPanel.transform.position = new Vector3(width, height, 0f); 
	}

	void UpdateInventoryPanelUI()
	{
		List<GameObject> slots = GetInventorySlots();

		for (int i = 0; i < inventory.size; i++)
		{
			GameObject slotItem = slots[i].transform.Find("InventorySlotItem").gameObject;
			Image slotItemImage = slotItem.GetComponent<Image>();
			if (inventory.GetItem(i) != null)
			{
				slotItemImage.sprite = inventory.GetItem(i).sprite;
				if (slotItemImage.sprite == null)
					slotItemImage.color = clearColor;
				slotItemImage.color = Color.white;
			}
			else
			{
				slotItemImage.color = clearColor;
			}

		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name != null)
		{
			inventory.AddItem(other.gameObject.name);
			UpdateInventoryPanelUI();
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


	GameObject ghostInventoryItemImage;

	public GameObject OnBeginDragInventoryItem(int slotIndex)
	{
		List<GameObject> inventorySlots = GetInventorySlots();
		foreach (GameObject slot in inventorySlots)
		{
			slot.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		
		if (inventory.GetItem(slotIndex) != null)
		{
			ghostInventoryItemImage = new GameObject();
			ghostInventoryItemImage.transform.SetParent(inventoryPanel.transform);
			ghostInventoryItemImage.AddComponent<Image>();
			ghostInventoryItemImage.GetComponent<Image>().sprite = inventory.GetItem(slotIndex).sprite;
			ghostInventoryItemImage.GetComponent<Image>().raycastTarget = false;
			ghostInventoryItemImage.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1f);
		}

		GameObject inventorySlot = inventorySlots[slotIndex];
		GameObject inventorySlotItem = inventorySlot.transform.Find("InventorySlotItem").gameObject;
		inventorySlotItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
		inventorySlotItem.GetComponent<Image>().color = clearColor;
		return inventorySlotItem;
	}
	
	public void OnDragInventoryItem(PointerEventData eventData)
	{
		if (ghostInventoryItemImage != null)
		{
			ghostInventoryItemImage.transform.position = eventData.position;
		}
	}

	public void OnEndDragInventoryItem(PointerEventData eventData)
	{
		if (ghostInventoryItemImage != null)
		{
			Destroy(ghostInventoryItemImage);
		}

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

		UpdateInventoryPanelUI();
		inventory.SaveInventory();
		
	}

}
