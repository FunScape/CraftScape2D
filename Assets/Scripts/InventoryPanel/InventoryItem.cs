using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Consumable, Food, Health, Weapon, Equipable, MainHand, Inventory
}

[System.Serializable]
public class InventoryItem {

	[System.NonSerialized]
	public string uuid;

	public int id { get; set; }
	public string name;
	public string spriteName;
	public Sprite sprite;
	public string description;
	public int maxStackSize;
    public float value;
	public bool equipable;
	public int rarity;
	public int minLevel;
	public int baseDurability;
	public bool soulBound;
	public float power;
	public float defense;
	public float vitality;
	public float healAmount;
	public int inventoryID;
	public int inventoryPosition;
	public int stackSize;
	public int createdByID;
	public string createdByName;

	public InventoryItem(int id, Sprite sprite, string name, string description, 
	float value, int maxStackSize, int stackSize, float power, float defense, float vitality,
	float healAmount, bool equipable, int rarity, int minLevel, int baseDurability, bool soulBound,
	int inventoryID, int createdByID, string createdByName, int inventoryPosition=-1)
	{
		this.id = id;
		this.uuid = System.Guid.NewGuid().ToString();
		this.sprite = sprite;
		this.spriteName = sprite.name;
		this.name = name;
		this.description = description;
		this.maxStackSize = maxStackSize;
		this.stackSize = stackSize;
        this.value = value;
		this.power = power;
		this.defense = defense;
		this.vitality = vitality;
		this.healAmount = healAmount;
		this.equipable = equipable;
		this.rarity = rarity;
		this.minLevel = minLevel;
		this.baseDurability = baseDurability;
		this.soulBound = soulBound;
		this.inventoryID = inventoryID;
		this.inventoryPosition = inventoryPosition;
		this.createdByID = createdByID;
		this.createdByName = createdByName;
		this.inventoryPosition = inventoryPosition;
	}

	public InventoryItem(int id, string uuid, Sprite sprite, string title, string description, 
	float value, int maxStackSize, int stackSize, float power, float defense, float vitality,
	float healAmount, int inventoryPosition=-1)
	{
		this.id = id;
		this.uuid = uuid;
		this.sprite = sprite;

		if (this.sprite == null)
			this.spriteName = "";
		else
			this.spriteName = sprite.name;
			
		this.name = title;
		this.description = description;
		this.maxStackSize = maxStackSize;
		this.stackSize = stackSize;
        this.value = value;
		this.power = power;
		this.defense = defense;
		this.vitality = vitality;
		this.healAmount = healAmount;
		this.inventoryPosition = inventoryPosition;
	}

	public InventoryItem Clone()
	{
		return new InventoryItem(this.id, System.Guid.NewGuid().ToString(), this.sprite, this.name, this.description, this.value, this.maxStackSize, this.stackSize, this.power,
		this.defense, this.vitality, this.healAmount, this.inventoryPosition);
	}

	public void Save(Inventory inventory)
	{
		string json = JsonUtility.ToJson(this);
		Database database = new Database();
		database.WriteToFile(Application.dataPath + string.Format("/GameData/{0}.json", this.uuid), json);
	}


}

[System.Serializable]
public class SerializableInventoryItem : System.Object {
	public int id { get; set; }
	public string spriteName { get; set; }
	public int maxStackSize { get; set; }
	public int stackSize { get; set; }
	public string title { get; set; }
	public string description { get; set; }
	public double value { get; set; }
	public string[] types { get; set; }

	public Dictionary<string, object> stats;

	public Dictionary<string, object> metadata;

	public SerializableInventoryItem(InventoryItem item)
	{
		this.id = item.id;
		this.spriteName = item.spriteName;
		this.maxStackSize = item.maxStackSize;
		this.stackSize = item.stackSize;
		this.title = item.name;
		this.description = item.description;
		this.value = (double)item.value;
		this.types = item.types.ToArray();

		this.stats = new Dictionary<string, object>();
		this.stats.Add("power", (double)item.power);
		this.stats.Add("defense", (double)item.defense);
		this.stats.Add("vitality", (double)item.vitality);
		this.stats.Add("healAmount", (double)item.healAmount);

		this.metadata = new Dictionary<string, object>();
		this.metadata.Add("inventoryPosition", item.inventoryPosition);
		this.metadata.Add("uuid", item.uuid);
	}

}

