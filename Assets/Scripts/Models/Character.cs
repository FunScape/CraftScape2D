using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class Character : ScriptableObject {

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Currency { get; private set; }
    public double WalkSpeed { get; private set; }
    public Vector2 Position;
    public List<Inventory> Inventories { get; private set; }
    public List<string> inventoryUrls { get; set; }
    public Equipment equipment;
    public string equipmentUrl;

    void Init(int id, int userId, string name, int health, int maxHealth, int currency, double walkSpeed, List<string> inventoryUrls, string equipmentUrl)
    {
        this.Id = id;
        this.UserId = userId;
        this.Name = name;
        this.Health = health;
        this.MaxHealth = maxHealth;
        this.Currency = currency;
        this.WalkSpeed = walkSpeed;
        this.inventoryUrls = inventoryUrls;
        this.equipmentUrl = equipmentUrl;
    }

    void Init(int id, int userId, string name, int health, int maxHealth, int currency, double walkSpeed, List<Inventory> inventories, Equipment equipment) 
    {
        Init(id, userId, name, health, maxHealth, currency, walkSpeed, new List<string>(), equipment.Url);
        Inventories = inventories;
        this.equipment = equipment;
    }

    public static Character Parse(JsonData data)
    {
        int id = (int) data["id"];
        int userId = (int) data["user"];
        string name = data["name"].ToString();
        int health = (int) (double) data["health"];
        int maxHealth = (int) (double) data["max_health"];
        int currency = (int) data["currency"];
        double walkSpeed = (double) data["walk_speed"];
        
        double? posX = null;
        double? posY = null;
        try {
            posX = (double) data["x_pos"];
            posY = (double) data["y_pos"];
        } catch (System.Exception) {}

        List<string> inventoryUrls = new List<string>();
        foreach(JsonData url in data["inventories"]) { inventoryUrls.Add(url.ToString()); }
        string equipmentUrl = data["equipment"].ToString();
        Character character = Character.CreateInstance("Character") as Character;
        character.Init(id, userId, name, health, maxHealth, currency, walkSpeed, inventoryUrls, equipmentUrl);
        if (posX != null && posY != null)
            character.Position = new Vector2((float)posX, (float)posY);
        return character;
    }

}