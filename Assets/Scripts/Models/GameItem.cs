using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[CreateAssetMenu(fileName="New Game Item", menuName="Inventory/Game Item", order=2)]
public class GameItem : ScriptableObject {

	public bool Dirty;
	public bool Locked;
    public bool Deleted;

	private string uuid;
	public string Uuid { get { return uuid; } }

    private int id;
    private string url;
    private int position;
    private int inventoryId;
    private int stackSize;
    private int createdById;
    private Character createdBy;
    private string createdByName;
    private int staticGameItemId;
    private StaticGameItem _staticGameItem;
    
	public StaticGameItem staticGameItem { get { return _staticGameItem; } set { _staticGameItem = value; Dirty = true;} }
	public int Id { get { return id; } set { id = value; Dirty = true;} }
	public string Url { get { return url; } set { url = value; Dirty = true;} }
	public int Position { get { return position; } set { position = value; Dirty = true;} }
	public int InventoryId { get { return inventoryId; } set { inventoryId = value; Dirty = true;} }
	public int StackSize { get { return stackSize; } set { stackSize = value; Dirty = true;} }
	public int CreatedById { get { return createdById; } set { createdById = value; Dirty = true;} }
	public Character CreatedBy { get { return createdBy; } set { createdBy = value; Dirty = true;} }
	public string CreatedByName { get { return createdByName; } set { createdByName = value; Dirty = true;} }
	public int StaticGameItemId { get { return staticGameItemId; } set { staticGameItemId = value; Dirty = true;} }

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
		uuid = System.Guid.NewGuid ().ToString ();
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
        if (data == null)
            return null;
            
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

	public GameItem Clone(bool exact = false)
    {
		GameItem item = GameItem.CreateInstance(this.staticGameItem);
		if (exact == true) {
			item.Map (this);
			item.uuid = uuid;
		}
		return item;
    }

    /*
    @description: Maps properties of the GameItem to the passed in GameItem 'other' excluding the uuid.
    @param <GameItem> other: The GameItem to map to.
     */
	public void Map(GameItem other)
	{
		Id = other.Id;
		Url = other.Url;
		Position = other.Position;
		InventoryId = other.InventoryId;
		StackSize = other.StackSize;
		CreatedById = other.CreatedById;
		CreatedBy = other.CreatedBy;
		CreatedByName = other.CreatedByName;
		StaticGameItemId = other.StaticGameItemId;
		staticGameItem = other.staticGameItem;
		Dirty = false;
	}

    public string ToJson()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("id", Id);
        data.Add("inventory", InventoryId);
        data.Add("inventory_position", Position);
        data.Add("stack_size", StackSize);
        data.Add("created_by", CreatedBy);
        return JsonMapper.ToJson(data).ToString();
    }

}