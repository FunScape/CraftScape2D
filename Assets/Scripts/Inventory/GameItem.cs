using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem {

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


