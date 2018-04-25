using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject hoverTextPrefab;
    GameObject hoverText;
    GameItem item;
	InventorySlot parent;

    Vector3 offset = new Vector3(-120f, -100f, 0f);

    public void OnDrop(PointerEventData eventData)
    {
		if (eventData.button == PointerEventData.InputButton.Left) {
            GameObject parentContainer = transform.parent.gameObject;
            InventorySlot inventorySlot = parentContainer.GetComponent<InventorySlot>();
            inventorySlot.OnDropInventoryItem(eventData.pointerDrag.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		UpdateItem ();
        StartHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		UpdateItem ();
        StopHover();
    }

    void StartHover() {
		GameObject canvas = GameObject.FindWithTag("MainCanvas");
		hoverText = GameObject.Instantiate(hoverTextPrefab);
		hoverText.transform.SetParent(canvas.transform);
        UpdateText();
        UpdateItem();
    }

    void StopHover() {
        if (hoverText != null)
            Destroy(hoverText);
    }

    void Update() {

		if (hoverText != null && item != null && Input.GetKey(KeyCode.LeftControl)) {

			hoverText.transform.position = Input.mousePosition + offset;

		} else if (hoverText != null) {
			hoverText.transform.position = new Vector3 (1000f, 1000f, 0);
		}
    }

	void UpdateItem()
	{
		if (GameManager.GetLocalPlayer().GetComponent<SetupLocalHero>().isLocalPlayer) {
			parent = transform.parent.gameObject.GetComponent<InventorySlot> ();
			GameObject player = GameManager.GetLocalPlayer();
			InventoryController controller = player.GetComponent<InventoryController>();
			item = controller.inventory.GameItems[parent.slotIndex];
		}
	}

    void UpdateText()
    {
        if (hoverText != null && item != null) {
            hoverText.transform.Find("Name").GetComponent<Text>().text = item.Name;
            hoverText.transform.Find("UUID").GetComponent<Text>().text = item.Uuid;
            hoverText.transform.Find("Id").GetComponent<Text>().text = item.Id.ToString();
        }
    }
}
