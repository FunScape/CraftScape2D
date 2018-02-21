using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	static Sprite defaultSprite;

	GameObject backgroundImage;// { get { return transform.Find("Background").gameObject; } }
	GameObject slotItem;// { get { return transform.Find("SlotItem").gameObject; } }

	public GameObject draggedSlotItem;

	public int slotIndex = -1;

	Color clearColor {
		get {
			Color color = Color.white;
			color.a = 0f;
			return color;
		}
	}

	private GameItem _item;
	public GameItem Item { get { return _item; } }

	void Start()
	{
		if (defaultSprite == null)
			defaultSprite = (Sprite)Resources.Load("Sprites/RPG_inventory_icons/f", typeof(Sprite));

		backgroundImage = transform.Find("Background").gameObject;
		slotItem = transform.Find("SlotItem").gameObject;
	}

	public void SetItem(GameItem item)
	{
		if (slotItem == null) { slotItem = transform.Find("SlotItem").gameObject; }

		_item = item;

		slotItem.GetComponent<SlotItem>().item = _item;
		slotItem.GetComponent<SlotItem>().UpdateStackLabelText();
		// string spriteName = item != null ? item.spriteName.ToUpper() : "NULL";
		// Debug.Log("Adding '" + spriteName + "': @Slot.SetItem()");

		if (_item != null) {
			_item.inventoryPosition = this.slotIndex;

			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

			GameObject player = null;

			foreach (GameObject p in players)
			{
				if (p.GetComponent<SetupLocalPlayer>().isLocalPlayer)
				{
					player = p;
					break;
				}
			}

			if (player != null)
			{
				try {
					GameObject inventory = player.transform.GetChild(0).gameObject;
					string inventoryFilePath = inventory.GetComponent<Inventory>().inventoryFilePath;
					ItemDatabase.instance.WriteOneToFile(inventoryFilePath, _item);
				} catch (System.Exception) {

				}

			}

		}

		UpdateItemImage();
	}

	public void UpdateItemImage()
	{
		Image image = slotItem.gameObject.GetComponent<Image>();

		image.sprite = _item == null ? defaultSprite :
			(Sprite)Resources.Load(ItemDatabase.itemSpritesPath + _item.spriteName, typeof(Sprite));

		slotItem.gameObject.GetComponent<Image>().color = _item != null ? Color.white : clearColor;
	}

	public void UpdateItemStackLabel()
	{
		slotItem.GetComponent<SlotItem>().UpdateStackLabelText();
	}

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
		{
			Slot[] allSlots = this.transform.parent.GetComponentsInChildren<Slot>();
			foreach (Slot group in allSlots) 
				{ group.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false; }

			GetComponent<CanvasGroup>().blocksRaycasts = false;
			draggedSlotItem = this.slotItem;
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
			draggedSlotItem.transform.SetParent(this.transform.parent.parent);
			draggedSlotItem.transform.position = eventData.position;
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
		{
			draggedSlotItem.transform.position = eventData.position;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (draggedSlotItem != null)
		{
			Slot[] allSlots = this.transform.parent.GetComponentsInChildren<Slot>();
			foreach (Slot group in allSlots)
			{
				group.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}

			draggedSlotItem.transform.SetParent(this.transform);
			draggedSlotItem.transform.position = this.transform.position;
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
			GameItem updatedItem = draggedSlotItem.GetComponent<SlotItem>().item;
			// Debug.LogFormat("Item is null? {0}", updatedItem == null ? "YES" : "NO");
			draggedSlotItem = null;

			SetItem(updatedItem);
		}
    }

}
