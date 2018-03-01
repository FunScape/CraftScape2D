using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	bool showEquipmentPanel = false;

	bool didOpenEquipmentPanelOnce = false;

	public GameObject equipmentPanelPrefab;

	GameObject equipmentPanel;

	// Use this for initialization
	void Start () {
		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		equipmentPanel = GameObject.Instantiate (equipmentPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.G)) {
			ToggleEquipment ();
		}
	}

	void ToggleEquipment()
	{
		showEquipmentPanel = !showEquipmentPanel;

		if (!didOpenEquipmentPanelOnce) {
			UpdateEquipmentPanelUI();
			didOpenEquipmentPanelOnce = true;
		}

		float height = Camera.main.pixelHeight;
		float width = Camera.main.pixelWidth;

		if (!showEquipmentPanel)
			width -= 1000f;

		equipmentPanel.transform.position = new Vector3(width, height, 0f); 
	}

	void UpdateEquipmentPanelUI()
	{

	}
	
}
