using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

	GameItem equipedItem;

	Dictionary<string, Sprite> slotEmptySprites;

    void Start () {
		// Get sprites used as placeholder images for empty inventory slots.
		string spritePath = Database.itemSpritesPath;
		slotEmptySprites = new Dictionary<string, Sprite>() 
		{
			{ "Ring", (Sprite)Resources.Load(spritePath + "rings_grey", typeof(Sprite)) },
			{ "Head", (Sprite)Resources.Load(spritePath + "helmets_grey", typeof(Sprite)) },
			{ "Neck", (Sprite)Resources.Load(spritePath + "necklace_grey", typeof(Sprite)) },
			{ "MainHand", (Sprite)Resources.Load(spritePath + "sword_grey", typeof(Sprite)) },
			{ "Chest", (Sprite)Resources.Load(spritePath + "armor_grey", typeof(Sprite)) },
			{ "Back", (Sprite)Resources.Load(spritePath + "cloaks_grey", typeof(Sprite)) },
			{ "Hands", (Sprite)Resources.Load(spritePath + "gloves_grey", typeof(Sprite)) },
			{ "Legs", (Sprite)Resources.Load(spritePath + "pants_grey", typeof(Sprite)) },
			{ "Feet", (Sprite)Resources.Load(spritePath + "boots_grey", typeof(Sprite)) }
		};
	}

	public GameItem GetEquipedItem()
	{
		return this.equipedItem;
	}

	public GameItem UnEquipItem()
	{
		GameItem item = this.equipedItem;
		this.equipedItem = null;
		ShowAsEmpty(true);
		return item;
	}

	public void EquipItem(GameItem item)
	{
		this.equipedItem = item;
		GetComponent<Image>().sprite = item.sprite;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipedItem != null)
		{
			EquipmentController controller = GetEquipmentController();
			controller.OnBeginDragEquipedItem(this.gameObject);
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (equipedItem != null)
		{
			EquipmentController controller = GetEquipmentController();
			controller.OnDragEquipedItem(eventData);
			// equipedItem.gameObject.transform.position = eventData.position;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetEquipmentController().OnEndDragEquipedItem(this.gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (equipedItem == null)
		{
			// Debug.Log(eventData.pointerDrag.gameObject.name);
			EquipmentController controller = GetEquipmentController();
			controller.OnDropInventoryItem(this.gameObject, eventData.pointerDrag.gameObject);
		}
		else
		{

		}
    }

	EquipmentController GetEquipmentController()
	{
		GameObject player = GameObject.FindWithTag("Player");
		return player.GetComponent<EquipmentController>();
	}

	public void ShowAsEmpty(bool showAsEmpty)
	{
		if (showAsEmpty)
			GetComponent<Image>().sprite = GetSlotEmptySprite();
		else if (equipedItem == null)
			GetComponent<Image>().sprite = GetSlotEmptySprite();
		else
			GetComponent<Image>().sprite = equipedItem.sprite;
		// GetComponent<Image>().sprite = showAsEmpty || equipedItem == null ? GetSlotEmptySprite() : equipedItem.sprite;
	}

	Sprite GetSlotEmptySprite()
	{
		try {
			return slotEmptySprites[gameObject.name];
		} catch (KeyNotFoundException) {
			return null;
		}
	}
    
}
