using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// [CreateAssetMenu()]
public class InventoryItem : ScriptableObject {

	[System.NonSerialized]
	public string uuid;
	public const string SpritesPath = "Sprites/RPG_inventory_icons/";
	public GameItem gameItem { get; private set; }


	public int id { get; set; }
    public int staticID { get; set; }
	public string Name { get { return gameItem.staticGameItem.Name; } }
	public string spriteName { get { return gameItem.staticGameItem.SpriteName; } }
	public Sprite sprite { get { return (Sprite)Resources.Load(SpritesPath + spriteName, typeof(Sprite)); } }
	public string description { get { return gameItem.staticGameItem.Description; } }
	public int maxStackSize { get { return gameItem.staticGameItem.MaxStack; } }
    public float value { get { return gameItem.staticGameItem.Value; } }
	public bool equipable { get { return gameItem.staticGameItem.Equipable; } }
	public int rarity { get { return gameItem.staticGameItem.Rarity; } }
	public int minLevel { get { return gameItem.staticGameItem.MinLevel; } }
	public int baseDurability { get { return gameItem.staticGameItem.BaseDurability; } }
	public bool soulbound { get { return gameItem.staticGameItem.Soulbound; } }
	public float power { get { return (float) gameItem.staticGameItem.Power; } }
	public float defense { get { return (float) gameItem.staticGameItem.Defense; } }
	public float vitality { get { return (float) gameItem.staticGameItem.Vitality; } }
	public float healAmount { get { return (float) gameItem.staticGameItem.HealAmount; } }
	public int? inventoryID { get { return gameItem.InventoryId; } set { gameItem.InventoryId = value; } }
	public int inventoryPosition { get { return gameItem.Position; } set { gameItem.Position = value; } }
	public int stackSize { get { return gameItem.StackSize; } set { gameItem.StackSize = value; } }
	public int createdByID { get { return gameItem.CreatedById; } }
	public string createdByName { get { return gameItem.CreatedByName; } }
	public List<string> types { get { return gameItem.staticGameItem.ItemTypes; } }

	public static InventoryItem CreateInstance(GameItem item)
	{
		InventoryItem invItem = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
		invItem.Init(item);
		return invItem;
	}

	void Init(GameItem item)
	{
		gameItem = item;
	}

	public InventoryItem Clone()
	{
		return InventoryItem.CreateInstance(this.gameItem);
	}

}

[System.Serializable]
public class SerializableInventoryItem : System.Object {
	int id;
    int staticID;
    int? inventoryID;
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

