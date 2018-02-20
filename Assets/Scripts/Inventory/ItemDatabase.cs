using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class ItemDatabase : MonoBehaviour {

    public const string itemSpritesPath = "Sprites/RPG_inventory_icons/";

    private List<GameItem> database = new List<GameItem>();

    private JsonData itemData;

	void Start () {
        
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/ItemDatabase.json"));
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
            database.Add(DeserializeGameItem(itemData[i]));
        }
    }

    public void WriteToJsonFile(Inventory inventory)
    {
        List<GameItem> items = inventory.GetGameItems();

        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;

        writer.WriteArrayStart();
        foreach (GameItem item in items)
        {
            SerializeGameItem(item, writer);
        }
        writer.WriteArrayEnd();

        // Debug.LogFormat("DATA TO JSON: {0}", sb.ToString());

        File.WriteAllText(Application.streamingAssetsPath + "/PlayerInventory.json", sb.ToString());

    }

    public List<GameItem> LoadFromFile(string filePath)
    {
        List<GameItem> items = new List<GameItem>();
        JsonData data = JsonMapper.ToObject(File.ReadAllText(filePath));
        foreach (JsonData itemData in data)
        {
            items.Add(DeserializeGameItem(itemData));
        }
        return items;
    }

    void SerializeGameItem(GameItem item, JsonWriter writer)
    {
        writer.WriteObjectStart();
        writer.WritePropertyName("id");
        writer.Write(item.Id);
        writer.WritePropertyName("spriteName");
        writer.Write(item.Sprite.name);
        writer.WritePropertyName("title");
        writer.Write(item.Title);
        writer.WritePropertyName("description");
        writer.Write(item.Description);
        writer.WritePropertyName("maxStackSize");
        writer.Write(item.MaxStackSize);
        writer.WritePropertyName("value");
        writer.Write(item.Value);
        writer.WritePropertyName("stats"); // Write "stats"
        writer.WriteObjectStart();
        writer.WritePropertyName("power");
        writer.Write(item.Power);
        writer.WritePropertyName("defense");
        writer.Write(item.Defense);
        writer.WritePropertyName("vitality");
        writer.Write(item.Vitality);
        writer.WritePropertyName("healAmount");
        writer.Write(item.HealAmount);
        writer.WriteObjectEnd(); // end "stats"
        writer.WritePropertyName("types"); // write "types"
        writer.WriteArrayStart();
        foreach (string type in item.Types.ToArray())
            writer.Write(type);
        writer.WriteArrayEnd(); // end types
        writer.WriteObjectEnd(); // end main object
    }

    GameItem DeserializeGameItem(JsonData data)
    {

        int id = (int)data["id"];
        string spritePath = itemSpritesPath + data["spriteName"].ToString();
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
        JsonData typesJson = data["types"];
        for (var j = 0; j < typesJson.Count; j++)
        {
            types.Add(typesJson[j].ToString());
        }
        return new GameItem(id, sprite, title, description, 
            (float)value, stackSize, (float) power, (float)defense,
            (float)vitality, (float)healAmount, types);
    }

}
