using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName="Inventory", menuName="Inventory/Inventory", order=1)]
public class Inventory : ScriptableObject {

	public int Id { get; set; }
	public int Position { get; set; }
	public int CharacterId { get; set; }
	public int Size { get; set; }
	public GameItem[] GameItems;

	private List<GameItem> trash = new List<GameItem> ();

	public static Inventory CreateInstance()
	{
		return Inventory.CreateInstance(-1, -1, -1, 16);
	}

	public static Inventory CreateInstance(int id, int position, int characterId, int size, GameItem[] items = null)
	{
		if (items == null)
			items = new GameItem[size];

		Inventory inventory = ScriptableObject.CreateInstance("Inventory") as Inventory;
		inventory.Id = id;
		inventory.Position = position;
		inventory.CharacterId = characterId;
		inventory.Size = size;
		inventory.GameItems = items;
		inventory.UpdateItemPositions();
		return inventory;
	}

	public static Inventory Parse(JsonData data) {
		int id = (int) data["id"];
		int position = (int) data["position"];
		int characterId = (int) data["character"];
		int size = (int) data["size"];
		GameItem[] items = new GameItem[size];
		for (int i = 0; i < data["game_items"].Count; i++) 
		{
			items[i] = GameItem.Parse(data["game_items"][i]);
		}
		return Inventory.CreateInstance(id, position, characterId, size, items);
	}

	public void SwapItems(int index1, int index2)
	{
		GameItem temp = GameItems[index1];
		GameItems[index1] = GameItems[index2];
		GameItems[index2] = temp;
		if (GameItems[index1] != null)
			GameItems [index1].Position = index1;
		if (GameItems[index2] != null)
			GameItems [index2].Position = index2;
	}

	public void AddItem(GameItem item, InventoryController controller)
	{
		bool didFindMatchingItem = false;

		for (int i = 0; i < GameItems.Length; i++)
		{
			GameItem currentItem = GameItems[i];

			if (currentItem == null)
				continue;

			if (currentItem.Name == item.Name && currentItem.StackSize < currentItem.MaxStackSize)
			{
				currentItem.StackSize++;
				didFindMatchingItem = true;
				break;
			}
		}

		if (!didFindMatchingItem)
		{
			for (int i = 0; i < GameItems.Length; i++)
			{
				if (GameItems[i] == null)
				{
					GameItems[i] = item.Clone();
					GameItems[i].Position = i;
					break;
				}
			}
		}

		Save(controller);

	}

	public void AddItem(StaticGameItem item, InventoryController controller)
	{
		GameItem gameItem = GameItem.CreateInstance(item);
		AddItem(gameItem, controller);
	}

	public void AddItem(int id, InventoryController controller)
	{
		AddItem(GameItemDatabase.instance.GetItem(id), controller);
	}

	public void AddItem(string name, InventoryController controller)
	{
		AddItem(GameItemDatabase.instance.GetItem(name), controller);
	}

    public int CheckQuantity(int checkItemId)
    {

        int qty = 0;

        foreach (GameItem item in GameItems)
        {
            if (item != null)
            {
                if (item.StaticGameItemId == checkItemId)
                    qty += item.StackSize;
            }
        }

        return qty;
    }


    public GameItem RemoveItem(GameItem item)
	{
		for (int i = 0; i < GameItems.Length; i++)
		{
			if (GameItems[i] != null && GameItems[i].Uuid == item.Uuid)
			{
				GameItem temp = GameItems [i].Clone (true);
				GameItems[i] = null;
				trash.Add (temp);
				Save ();
				return temp;
			}
		}
		return null;
	}

	public GameItem RemoveItem(StaticGameItem item)
	{
		for (int i = 0; i < GameItems.Length; i++)
		{
			if (GameItems[i] != null && GameItems[i].staticGameItem.Equals(item))
			{
				GameItem temp = GameItems [i].Clone (true);
				GameItems[i] = null;
				trash.Add (temp);
				Save ();
				return temp;
			}
		}
		return null;
	}

	public GameItem RemoveItem(int index)
	{
		return RemoveItem (GameItems [index]);
	}

	void UpdateItemPositions()
	{
		for (int i = 0; i < GameItems.Length; i++)
		{
			if (GameItems[i] != null && GameItems[i].Position != i)
			{
				SwapItems(i, GameItems[i].Position);
			}
		}
	}

    public void RemoveQtyOfItems(int id, int qty)
    {
        foreach (GameItem item in GameItems)
        {
            if (item != null)
            {
                if (item.StaticGameItemId == id)
                {
                    if (item.StackSize> qty)
                    {
                        item.StackSize -= qty;
                        qty = 0;
                    }
                    else if (item.StackSize == qty)
                    {
                        RemoveItem(item.Position);
                        qty = 0;
                    }
                    else if (item.StackSize < qty)
                    {
                        qty -= item.StackSize;
                        RemoveItem(item.Position);
                    }
                }

                if (qty <= 0)
                {
                    Save();
                    return;
                }
            }
        }
    }

    public void Save(InventoryController controller = null)
	{
		if (PlayerPrefs.GetInt("IsLocalPlayer") == 1)
            return;

		if (controller == null)
			controller = GameManager.instance.LocalPlayer().GetComponent<InventoryController>();

		APIManager apiManager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();

		for (int i = 0; i < GameItems.Length; i++)
		{
			GameItem item = GameItems [i];

			if (item == null)
				continue;

			if (item.Position != i){
				item.Position = i;
			}

			if (item.CreatedById == -1) {
				item.CreatedById = CharacterId;
			}

			if (item.Dirty && !item.Locked)
			{
				item.InventoryId = this.Id;
				item.Locked = true;  // Lock the game item to prevent further editing

				if (item.Id == -1)
				{
					item.CreatedById = this.CharacterId;
					controller.StartCoroutine(apiManager.CreateGameItem(item, (newItem) => {
						item.Locked = false;  // Unlock the game item to allow editing
						if (newItem != null)
							item.Map(newItem);
					}));
				}
				else
				{
					controller.StartCoroutine(apiManager.UpdateGameItem(item, (updatedItem) => {
						item.Locked = false;  // Unlock the game item to allow editing
						if (updatedItem != null)
							item.Map(updatedItem);
					}));
				}
				
			}
		}

		foreach (GameItem item in trash) 
		{
			if (item.Deleted)
				continue;
			
			item.InventoryId = null;
			item.Position = -1;
			controller.StartCoroutine(apiManager.UpdateGameItem(item, (updatedItem) => {
				Debug.Log("Removed item: " + item.Name);
				item.Deleted = true;
			}));
		}

	}



	public void Load()
	{
		if (PlayerPrefs.GetInt("IsLocalPlayer") == 1)
            return;

		APIManager apiManager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();
		apiManager.GetInventory(Id, (inventory) => {
			Id = inventory.Id;
			Position = inventory.Position;
			CharacterId = inventory.CharacterId;
			Size = inventory.Size;
			GameItems = inventory.GameItems;
		});
	}

}
