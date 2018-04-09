using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[CreateAssetMenu(fileName="New Game Item", menuName="Inventory/Game Item", order=2)]
public class GameItem : ScriptableObject {

    public int Id;
    public string Url;
    public int Position;
    public int InventoryId;
    public int StackSize;
    public int CreatedById;
    public Character CreatedBy;
    public string CreatedByName;
    public int StaticGameItemId;
    public StaticGameItem staticGameItem;

    public string Name { get { return staticGameItem.Name; } }
    public int MaxStackSize { get { return staticGameItem.MaxStack; } }
    public bool Equipable { get { return staticGameItem.Equipable; } }
    public List<string> Types { get { return staticGameItem.ItemTypes; } }
    public Sprite sprite { get { return staticGameItem.sprite; } }

    public static GameItem CreateInstance()
    {
        return ScriptableObject.CreateInstance("GameItem") as GameItem;
    }

    public static GameItem CreateInstance(StaticGameItem staticItem)
    {
        GameItem item = GameItem.CreateInstance();
        item.Init(-1, null, -1, -1, 1, -1, null, staticItem.Id);
        item.staticGameItem = staticItem;
        return item;
    }

    public void Init(int id, string url, int position, int inventoryId, int stackSize, int createdById, string createdByName, int staticGameItemId)
    {
        Id = id;
        Url = url;
        Position = position;
        InventoryId = inventoryId;
        StackSize = stackSize;
        CreatedById = createdById;
        CreatedByName = createdByName;
        this.StaticGameItemId = staticGameItemId;
    }

    public static GameItem Parse(JsonData data)  
    {
        int Id = (int) data["id"];
        string Url = data["url"].ToString();
        int Position = (int) data["inventory_position"];
        int InventoryId = (int) data["inventory"];
        int StackSize = (int) data["stack_size"];
        int CreatedById = (int) data["created_by"];
        string CreatedByName = data["created_by_name"].ToString();
        int StaticGameItemId = (int) data["static_game_item"]["id"];
        GameItem item = GameItem.CreateInstance();
        item.Init(Id, Url, Position, InventoryId, StackSize, CreatedById, CreatedByName, StaticGameItemId);
        item.staticGameItem = StaticGameItem.Parse(data["static_game_item"]);
        return item;
    }

    public GameItem Clone()
    {
        return GameItem.CreateInstance(this.staticGameItem);
    }

}