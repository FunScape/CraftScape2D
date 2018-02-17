using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	private Sprite m_sprite;
	public Sprite sprite { get { return m_sprite; } }

	private string m_id;
	public string id { get { return m_id; } }

	private int m_maxStackSize;
	public int maxStackSize { get { return m_maxStackSize; } }

	private string _itemName;
	public string itemName { get { return _itemName; } }

	private string _description;
	public string description { get { return _description; } }

	public static Item CreateItem(string id, Sprite sprite, string itemName, int maxStackSize=1, string description="")
	{
		Item item = new Item();
		item.m_id = id;
		item.m_sprite = sprite;
		item._itemName = itemName;
		item._description = description;
		item.m_maxStackSize = maxStackSize;

		return item;
	}

	// Make constructor private
	private Item() {}

}
