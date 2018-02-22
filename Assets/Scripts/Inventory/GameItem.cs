using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : System.Object {

	public int id { get; set; }
	public string uuid { get; set; }
	public Sprite sprite { get; set; }
	public string spriteName { get; set; }
	public int maxStackSize { get; set; }
	public int stackSize { get; set; }
	public string title { get; set; }
	public string description { get; set; }
    public float value { get; set; }
	public float power { get; set; }
	public float defense { get; set; }
	public float vitality { get; set; }
	public float healAmount { get; set; }
	public List<string> types { get; set; }
	public int inventoryPosition { get; set; }

	public GameItem(int id, string uuid, Sprite sprite, string title, string description, 
	float value, int maxStackSize, int stackSize, float power, float defense, float vitality,
	float healAmount, List<string> types, int inventoryPosition=-1)
	{
		this.id = id;
		this.uuid = uuid;
		this.sprite = sprite;
		this.spriteName = sprite.name;
		this.title = title;
		this.description = description;
		this.maxStackSize = maxStackSize;
		this.stackSize = stackSize;
        this.value = value;
		this.power = power;
		this.defense = defense;
		this.vitality = vitality;
		this.healAmount = healAmount;
		this.types = types;
		this.inventoryPosition = inventoryPosition;
	}

	public GameItem Clone()
	{
		return new GameItem(this.id, this.uuid, this.sprite, this.title, this.description, this.value, this.maxStackSize, this.stackSize, this.power,
		this.defense, this.vitality, this.healAmount, this.types, this.inventoryPosition);
	}

	public void Save(Inventory inventory)
	{
		throw new System.NotImplementedException();
	}


}

[System.Serializable]
public class SerializableGameItem : System.Object {
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

	public SerializableGameItem(GameItem item)
	{
		this.id = item.id;
		this.spriteName = item.spriteName;
		this.maxStackSize = item.maxStackSize;
		this.stackSize = item.stackSize;
		this.title = item.title;
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

