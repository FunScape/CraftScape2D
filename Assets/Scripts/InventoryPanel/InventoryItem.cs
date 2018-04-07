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
    public int staticID { get; set; }
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
	public bool soulbound;
	public float power;
	public float defense;
	public float vitality;
	public float healAmount;
	public int inventoryID;
	public int inventoryPosition;
	public int stackSize;
	public int createdByID;
	public string createdByName;
	public List<string> types;

	public InventoryItem(int id, int staticID, Sprite sprite, string name, string description, 
    	float value, int maxStackSize, int stackSize, float power, float defense, float vitality,
    	float healAmount, bool equipable, int rarity, int minLevel, int baseDurability, bool soulbound,
	    int inventoryID, int createdByID, string createdByName, List<string> types, int inventoryPosition=-1)
	{
		this.id = id;
        this.staticID = staticID;
		this.uuid = System.Guid.NewGuid().ToString();
		this.sprite = sprite;
        if (this.sprite != null)
            this.spriteName = sprite.name;
        else
            this.spriteName = name;
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
		this.soulbound = soulbound;
		this.inventoryID = inventoryID;
		this.inventoryPosition = inventoryPosition;
		this.createdByID = createdByID;
		this.createdByName = createdByName;
		this.inventoryPosition = inventoryPosition;
		this.types = types;
	}


	public InventoryItem(int id, 
                         int staticID,
                         string uuid, 
                         Sprite sprite, 
                         string name, 
                         string description, 
                         float value, 
                         int maxStackSize, 
                         int stackSize, 
                         float power, 
                         float defense, 
                         float vitality,
                         float healAmount, 
                         bool equipable,
                         int rarity,
                         int minLevel,
                         int baseDurability,
                         bool soulbound,
                         int inventoryID,
                         int createdByID,
                         string createdByName,
						 List<string> types,
                         int inventoryPosition=-1)
	{
		this.id = id;
        this.staticID = staticID;
		this.uuid = uuid;
		this.sprite = sprite;

		if (this.sprite == null)
			this.spriteName = "";
		else
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
        this.soulbound = soulbound;
        this.inventoryID = inventoryID;
        this.createdByID = createdByID;
        this.createdByName = createdByName;
		this.types = types;
		this.inventoryPosition = inventoryPosition;
	}

	public InventoryItem Clone()
	{
		return new InventoryItem(this.id, 
                                 this.staticID,
                                 System.Guid.NewGuid().ToString(), 
                                 this.sprite, 
                                 this.name, 
                                 this.description, 
                                 this.value, 
                                 this.maxStackSize, 
                                 this.stackSize, 
                                 this.power,
                                 this.defense, 
                                 this.vitality, 
                                 this.healAmount, 
								 this.equipable,
								 this.rarity,
								 this.minLevel,
								 this.baseDurability,
								 this.soulbound,
								 this.inventoryID,
								 this.createdByID,
								 this.createdByName,
								 this.types,
                                 this.inventoryPosition);
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
	int id;
    int staticID;
    int inventoryID;
    int inventoryPosition;
    int stackSize;
    int createdByID;
    string createdByName;
	string name;
	string spriteName;
	string description;
	int maxStack;
	double value;
	bool equipable;
	int rarity;
	int minLevel;
	int baseDurability;
	bool soulbound;
	double power;
	double defense;
	double vitality;
	double healAmount;
	string[] types;

	public SerializableInventoryItem(InventoryItem item)
	{
		id = item.id;
        staticID = item.staticID;
		inventoryID = item.inventoryID;
		inventoryPosition = item.inventoryPosition;
		stackSize = item.stackSize;
		createdByID = item.createdByID;
		createdByName = item.createdByName;
		name = item.name;
		spriteName = item.spriteName;
		description = item.description;
		maxStack = item.maxStackSize;
		value = item.value;
		equipable = item.equipable;
		rarity = item.rarity;
		minLevel = item.minLevel;
		baseDurability = item.baseDurability;
		soulbound = item.soulbound;
		power = item.power;
		defense = item.defense;
		vitality = item.vitality;
		healAmount = item.healAmount;
		types = item.types.ToArray();
	}

    public static Dictionary<object, object> SerializableStaticItemDictionary(InventoryItem item)
    {
        Dictionary<object, object> serialized = new Dictionary<object, object>();
        serialized.Add("id", item.id);
		serialized.Add("name", item.name);
		serialized.Add("sprite_name", item.spriteName);
		serialized.Add("description", item.description);
		serialized.Add("max_stack", item.maxStackSize);
		serialized.Add("value", item.value);
		serialized.Add("equipable", item.equipable);
		serialized.Add("rarity", item.rarity);
		serialized.Add("min_level", item.minLevel);
		serialized.Add("base_durability", item.baseDurability);
		serialized.Add("soulbound", item.soulbound);
		serialized.Add("power", item.power);
		serialized.Add("defense", item.defense);
		serialized.Add("vitality", item.vitality);
		serialized.Add("heal_amount", item.healAmount);
		serialized.Add("item_type", item.types.ToArray());


        return serialized;
    }

}

