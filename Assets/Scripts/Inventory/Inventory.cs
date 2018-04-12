using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

[CreateAssetMenu(fileName="Inventory", menuName="Inventory/Inventory", order=1)]
public class Inventory : ScriptableObject {

	public int Id;
	public int Position;
	public int CharacterId;
	public int Size;
	public GameItem[] GameItems;

	private List<GameItem> trash = new List<GameItem> ();

	public static Inventory CreateInstance()
	{
		return Inventory.CreateInstance(-1, -1, -1, 8);
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

	public GameItem RemoveItem(GameItem item)
	{
		for (int i = 0; i < GameItems.Length; i++)
		{
			if (GameItems[i] != null && GameItems[i].Uuid == item.Uuid)
			{
				GameItem temp = GameItems [i].Clone (true);
				GameItems[i] = null;
				trash.Add (temp);
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

	public void Save(InventoryController controller)
	{
		APIManager apiManager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();

//		controller.StartCoroutine (apiManager.UpdateInventory (this, (inv) => {

		for (int i = 0; i < GameItems.Length; i++)
		{
			if (GameItems[i] != null && GameItems[i].Dirty == true && GameItems[i].Locked == false)
			{
				GameItem item = GameItems[i];
				item.InventoryId = this.Id;
				item.Position = i;
				item.Locked = true;  // Lock the game item to prevent further editing

				if (item.Id == -1)
				{
					item.CreatedById = this.CharacterId;
					controller.StartCoroutine(apiManager.CreateGameItem(item, (newItem) => {
						item.Locked = false;  // Unlock the game item to allow editing
						item.Map(newItem);
					}));
				}
				else
				{
					controller.StartCoroutine(apiManager.UpdateGameItem(item, (updatedItem) => {
						item.Locked = false;  // Unlock the game item to allow editing
						item.Map(updatedItem);
					}));
				}
				
			}
		}

//		}));
	}



	public void Load()
	{
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
