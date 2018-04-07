using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class StaticGameItem : ScriptableObject {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string SpriteName { get; private set; }
    public string Description { get; private set; }
    public int MaxStack { get; private set; }
    public int Value { get; private set; }
    public bool Equipable { get; private set; }
    public int Rarity { get; private set; }
    public int MinLevel { get; private set; }
    public int BaseDurability { get; private set; }
    public bool Soulbound { get; private set; }
    public double Power { get; private set; }
    public double Defense { get; private set; }
    public double Vitality { get; private set; }
    public double HealAmount { get; private set; }
    public List<string> ItemTypes { get; private set; }

    public static StaticGameItem CreateInstance()
    {
        return ScriptableObject.CreateInstance("StaticGameItem") as StaticGameItem;
    }

    void Init(int id, string name, string spriteName, string description, int maxStack, int value, bool equipable, int rarity, int minLevel, int baseDurability, bool soulbound,
        double power, double defense, double vitality, double healAmount, List<string> itemTypes) {
            Id = id;
            Name = name;
            SpriteName = spriteName;
            Description = description;
            MaxStack = maxStack;
            Value = value;
            Equipable = equipable;
            Rarity = rarity;
            MinLevel = minLevel;
            BaseDurability = baseDurability;
            Soulbound = soulbound;
            Power = power;
            Defense = defense;
            Vitality = vitality;
            HealAmount = healAmount;
            ItemTypes = itemTypes;
    }

    public static StaticGameItem Parse(JsonData data)
    {
        int id = (int) data["id"];
        string name = data["name"].ToString();
        string spriteName = data["sprite_name"].ToString();
        string description = data["description"].ToString();
        int maxStack = (int) data["max_stack"];
        int value = (int) (double) data["value"];
        bool equipable = (bool) data["equipable"];
        int rarity = (int) data["rarity"];
        int minLevel = (int) data["min_level"];
        int baseDurability = (int) data["base_durability"];
        bool soulbound = (bool) data["soulbound"];
        double power = (double) data["power"];
        double defense = (double) data["defense"];
        double vitality = (double) data["vitality"];
        double healAmount = (double) data["heal_amount"];
        List<string> itemTypes = new List<string>();
        foreach (JsonData type in data["item_type"]) { itemTypes.Add(type.ToString()); }
        StaticGameItem item = StaticGameItem.CreateInstance();
        item.Init(id, name, spriteName, description, maxStack, value, equipable, rarity, minLevel, baseDurability, soulbound, power, defense, vitality, healAmount, itemTypes);
        return item;
    }

    public InventoryItem ToInventoryItem()
    {
        GameItem item = GameItem.CreateInstance(this);
        return InventoryItem.CreateInstance(item);
    }

}