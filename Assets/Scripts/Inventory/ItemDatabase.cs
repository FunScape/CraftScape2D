using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    public const string itemSpritesPath = "Sprites/RPG_inventory_icons/";

    private List<GameItem> database = new List<GameItem>();

    private JsonData itemData;

	void Start () {
        
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();

    }

    public GameItem GetItemById(int id)
    {
        foreach (GameItem item in database)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {

            JsonData data = itemData[i];
// (Sprite)Resources.Load("Sprites/RPG_inventory_icons/f", typeof(Sprite));
            int id = (int)data["id"];
            string spritePath = itemSpritesPath + data["spriteName"].ToString();
            Debug.Log(spritePath);
            Sprite sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
            string title = data["title"].ToString();
            string description = data["description"].ToString();
            double value = (double)data["value"];
            int stackSize = (int)data["maxStackSize"];

            JsonData stats = data["stats"];
            double power = (double)stats["power"];
            double defense = (double)stats["defense"];
            double vitality = (double)stats["vitality"];
            double healAmount = (double)stats["healAmount"];

            List<string> types = new List<string>();
            JsonData typesJson = data["stats"];
            for (var j = 0; j < typesJson.Count; j++)
            {
                types.Add(typesJson[j].ToString());
            }

            GameItem item = new GameItem(id, sprite, title, description, 
                (float)value, stackSize, (float) power, (float)defense,
                (float)vitality, (float)healAmount, types);
            database.Add(item);
        }
        // Debug.LogFormat("Loaded {0} items into database.", database.Count.ToString());
    }

}
