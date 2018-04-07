using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameItem : ScriptableObject {

    public int Id { get; private set; }
    public string Url { get; private set; }
    public int Position { get; set; }
    public int InventoryId { get; set; }
    public int StackSize { get; set; }
    public int CreatedById { get; private set; }
    public Character CreatedBy { get; private set; }
    public string CreatedByName { get; private set; }
    public int StaticGameItemId { get; private set; }
    public StaticGameItem staticGameItem { get; private set; }

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

}