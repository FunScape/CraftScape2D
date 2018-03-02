using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItem : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject parentContainer = transform.parent.gameObject;
        InventorySlot inventorySlot = parentContainer.GetComponent<InventorySlot>();
        inventorySlot.OnDropInventoryItem(eventData.pointerDrag.gameObject);
    }
}
