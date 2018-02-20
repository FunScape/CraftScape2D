using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	static Sprite defaultSprite;

	GameObject backgroundImage { get { return transform.Find("Background").gameObject; } }
	GameObject slotItem { get { return transform.Find("SlotItem").gameObject; } }

	GameObject draggedSlotItem;

	private GameItem _item;
	public GameItem Item { get { return _item; } }

	void Start()
	{
		if (defaultSprite == null)
			defaultSprite = (Sprite)Resources.Load("Sprites/RPG_inventory_icons/f", typeof(Sprite));
	}


	public void SetItem(GameItem item)
	{
		_item = item;
		slotItem.GetComponent<SlotItem>().item = item;
		try
		{
			slotItem.GetComponent<Image>().enabled = true;
		}catch (System.Exception){}

		slotItem.gameObject.GetComponent<Image>().sprite = item.Sprite;
	}

	void OnEnable()
	{
		if (Item != null && Item.Sprite != null)
		{
			slotItem.gameObject.GetComponent<Image>().enabled = true;
			slotItem.gameObject.GetComponent<Image>().sprite = _item.Sprite;
		}
		else
		{
			slotItem.gameObject.GetComponent<Image>().enabled = false;
		}
	}

	public void SetEnabledIfItemExists()
	{
		slotItem.gameObject.GetComponent<Image>().enabled = _item != null;
	}

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
		{
			draggedSlotItem = this.slotItem;
			draggedSlotItem.transform.SetParent(this.transform.parent);
			draggedSlotItem.transform.position = eventData.position;
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
		if (_item != null)
		{
			draggedSlotItem.transform.position = this.transform.position;
			draggedSlotItem.transform.SetParent(this.transform);
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.SetItem(draggedSlotItem.GetComponent<SlotItem>().item);
			draggedSlotItem = null;
		}
    }

}
