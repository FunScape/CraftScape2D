using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	static Sprite defaultSprite;

	GameObject backgroundSprite;
	GameObject itemSprite;

	private GameItem _item;
	public GameItem Item { get { return _item; } }

	void Start()
	{
		if (defaultSprite == null)
			defaultSprite = (Sprite)Resources.Load("Sprites/RPG_inventory_icons/f", typeof(Sprite));

		backgroundSprite = transform.Find("Background").gameObject;
		itemSprite = transform.Find("ItemImage").gameObject;
		
		if (backgroundSprite == null)
			Debug.LogWarning("Background Sprite is null!");

		if (itemSprite == null)
			Debug.LogWarning("Item Sprite is null!");

		itemSprite.GetComponent<Image>().enabled = false;
	}


	public void SetItem(GameItem item)
	{
		_item = item;
		// transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("Sprites/RPG_inventory_icons/axe", typeof(Sprite));

		// itemSprite.GetComponent<Image>().enabled = true;
		try
		{
			transform.Find("ItemImage").gameObject.GetComponent<Image>().enabled = true;
		}catch (System.Exception){}

		transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = item.Sprite;
	}

	void OnEnable()
	{
		if (Item != null && Item.Sprite != null)
		{
			transform.Find("ItemImage").gameObject.GetComponent<Image>().enabled = true;
			transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = _item.Sprite;
		}
		else
		{
			transform.Find("ItemImage").gameObject.GetComponent<Image>().enabled = false;
		}
	}

}
