using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroInventoryController : InventoryController
{

    public void SetupInventory(Inventory inventory = null)
    {
        if (inventory == null)
            inventory = Inventory.CreateInstance();

        this.inventory = inventory;
        
        // Get reference to main canvas object
        GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");

        // Get reference to equipment inventory container
        GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;

        // Instantiate inventory panel
        base.inventoryPanel = Instantiate(inventoryPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);

        // Tell inventory what file to write objects to
        // base.inventory.SetInventoryFileName(string.Format("inventory-{0}.json", GetComponent<SetupLocalHero>().netId.ToString()));

        // Load inventory items from file
        // base.inventory.LoadInventory();

        // Layout/render inventory on canvas
        base.LayoutInventory();
    }

    public void UpdateCharacterDBPosition()
    {
        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;

        APIManager manager = GameObject.FindWithTag("APIManager").GetComponent<APIManager>();

        Dictionary<string, float> data = new Dictionary<string, float>();
        data.Add("position_x", posX);
        data.Add("position_y", posX);

        // manager.PreparePUTRequest();

    }


}
