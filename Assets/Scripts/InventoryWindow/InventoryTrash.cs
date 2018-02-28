using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTrash : MonoBehaviour, IDropHandler {

    // Use this for initialization
    void Start () {
		
	}

    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogFormat("Trash Drop GameObject name: {0}", eventData.pointerDrag.gameObject.name);
		if (eventData.pointerDrag.gameObject.tag == "InventorySlot")
        {
            GameObject player = GameObject.FindWithTag("Player");
            PlayerInventoryController controller = player.GetComponent<PlayerInventoryController>();
            controller.RemoveInventoryItem(eventData.pointerDrag.gameObject);
        }
    }


}
