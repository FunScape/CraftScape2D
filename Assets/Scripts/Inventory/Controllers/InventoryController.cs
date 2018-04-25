using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{

    public GameObject inventoryPanelPrefab;

    public GameObject inventorySlotPrefab;

    public GameObject inventoryStackLabelPrefab;

    protected GameObject inventoryPanel;

    protected const string inventorySlotsContainerName = "InventorySlotsContainer";

    public Inventory inventory;

    protected bool showInventory;

    protected bool didOpenInventoryOnce;

    public Color clearColor { get { Color color = Color.white; color.a = 0f; return color; } }

    protected void Update()
    {
		
			
		if (Input.GetKeyDown(KeyCode.B)) 
		{
			GameObject player = GameManager.GetLocalPlayer();
			if (player.Equals(this.gameObject)) {
				ToggleInventory ();
			}
		}

    }

    void OnApplicationQuit()
	{
		inventory.Save();		
	}

    protected void LayoutInventory()
    {
        GameObject slotContainer = inventoryPanel.transform.Find(inventorySlotsContainerName).gameObject;
        for (int i = 0; i < this.inventory.Size; i++)
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

    protected void ToggleInventory()
    {
        showInventory = !showInventory;

        if (!didOpenInventoryOnce)
        {
            UpdateInventoryPanelUI();
            didOpenInventoryOnce = true;
        }

        float height = Camera.main.pixelHeight;
        float width = Camera.main.pixelWidth;

        if (!showInventory)
            width += 1000f;

        inventoryPanel.transform.position = new Vector3(width, height, 0f);
    }

    public void UpdateInventoryPanelUI()
    {
        List<GameObject> slots = GetInventorySlots();

        for (int i = 0; i < inventory.Size; i++)
        {
            GameObject slotItem = slots[i].transform.Find("InventorySlotItem").gameObject;
            slotItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Image slotItemImage = slotItem.GetComponent<Image>();
            GameItem item = inventory.GameItems[i];
            if (item != null)
            {
                slotItemImage.sprite = item.sprite;

                slotItemImage.color = slotItemImage.sprite == null ? clearColor : Color.white;

                if (item.MaxStackSize > 1)
                    slots[i].GetComponentInChildren<Text>().text = item.StackSize.ToString();
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

    protected void OnTriggerEnter2D(Collider2D other)
    {
		if (GameManager.instance.LocalPlayer().Equals(this.gameObject)) {
            if (other.gameObject.name != null)
            {
                StaticGameItem item = GameItemDatabase.instance.GetItem(other.gameObject.name);
                if (item != null)
                {
                    inventory.AddItem(item, this);
                    Destroy(other.gameObject);
                    UpdateInventoryPanelUI();
                }
            }
        }
			
		
        
    }

    protected List<GameObject> GetInventorySlots()
    {
        GameObject container = inventoryPanel.transform.Find(inventorySlotsContainerName).gameObject;
        List<GameObject> slots = new List<GameObject>();
        foreach (Transform child in container.transform)
        {
            slots.Add(child.gameObject);
        }
        return slots;
    }


    protected GameObject draggedInventoryItem;

    public GameObject OnBeginDragInventoryItem(int slotIndex)
    {
        // Set the inventoryPanel's sibling index to the equipmentPanel's 
        // sibling index, so images belonging to the inventoryPanel
        // will render on top of the equipment panel game object.
        GameObject equipmentPanel = GameObject.FindWithTag("EquipmentPanel");
        if (equipmentPanel.transform.GetSiblingIndex() > inventoryPanel.transform.GetSiblingIndex())
        {
            inventoryPanel.transform.SetSiblingIndex(equipmentPanel.transform.GetSiblingIndex());
        }

        List<GameObject> inventorySlots = GetInventorySlots();
        foreach (GameObject slot in inventorySlots)
        {
            slot.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if (inventory.GameItems[slotIndex] != null)
        {
            GameObject draggedSlotItem = inventorySlots[slotIndex].transform.Find("InventorySlotItem").gameObject;
            draggedInventoryItem = Instantiate(draggedSlotItem);
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

        inventory.Save(this);
    }

    public void OnDropEquipmentItem(GameObject inventorySlotObject, GameObject equipmentSlotObject)
    {
//        EquipmentController equipmentController = GetComponent<EquipmentController>();

        InventorySlot inventorySlot = inventorySlotObject.GetComponent<InventorySlot>();
        EquipmentSlot equipmentSlot = equipmentSlotObject.GetComponent<EquipmentSlot>();

        GameItem inventoryItem = inventory.GameItems[inventorySlot.slotIndex];
        if (inventoryItem == null)
        {
            inventory.GameItems[inventorySlot.slotIndex] = equipmentSlot.UnEquipItem();
            UpdateInventoryPanelUI();
        }
        else
        {
            Debug.Log("That inventory slot is occupied!");
        }
    }

    public void RemoveInventoryItem(GameObject inventorySlot)
    {
        int itemIndex = inventorySlot.GetComponent<InventorySlot>().slotIndex;
        inventory.RemoveItem(itemIndex);
        UpdateInventoryPanelUI();
    }


}
