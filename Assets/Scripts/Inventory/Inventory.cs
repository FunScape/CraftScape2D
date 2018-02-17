using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;

	// private float inventoryWidth, inventoryHeight;

	public int slotCount;

	public int rowCount;

	public float slotPadding, inventoryPadding;

	public float slotSize;

	public GameObject slotPrefab;

	private List<GameObject> slots;

	public const string defaultSlotName = "Slot";

	void Start()
	{
		inventoryRect = GetComponent<RectTransform>();
		CreateLayout();		
	}

	private void CreateLayout()
	{
		slots = new List<GameObject>();

		int columnCount = slotCount / rowCount;

		Debug.Log(slotCount.ToString() + " / " + rowCount.ToString() + " = " + columnCount.ToString());

		float inventoryWidth = (columnCount * (slotSize + 2 * slotPadding)) + (inventoryPadding);
		float inventoryHeight = (rowCount * (slotSize + 2 * slotPadding)) + (inventoryPadding);

		Debug.Log("Inventory Size: " + inventoryWidth.ToString() + "x" + inventoryHeight.ToString());

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < columnCount; col++) 
			{
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);

				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				newSlot.name = defaultSlotName;

				newSlot.transform.SetParent(this.transform.parent);

				float posX = (col * slotSize + (col + 1) * inventoryPadding) - (inventoryWidth / 2f) + 2 * inventoryPadding;
				float posY = (-row * slotSize - (row + 1) * inventoryPadding) + (inventoryHeight / 2f) - 2 * inventoryPadding;

				slotRect.localPosition = inventoryRect.localPosition + new Vector3(posX, posY, 0f);

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

				slots.Add(newSlot);
			}
		}

	}

}
