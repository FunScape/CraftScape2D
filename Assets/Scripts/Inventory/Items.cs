using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

	public Item GetItem(string itemId)
	{
		string spriteName;
		string itemName;
		int stackSize = 1;
		string description;

		switch (itemId) {
		case "bag":
			spriteName = "bag";
			itemName = "bag";
			stackSize = 1;
			description = "This is a bag.";
			break;
		case "apple":
			spriteName = "apple";
			itemName = "apple";
			stackSize = 10;
			description = "This is an apple.";
			break;
		default:
			throw new UnityException("Invalid item id");
		}

		return Item.CreateItem(itemId, Resources.Load(spriteName, typeof(Sprite)) as Sprite, itemName, stackSize, description);

	}

}
