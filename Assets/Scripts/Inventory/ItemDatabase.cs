using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class ItemDatabase : MonoBehaviour {

    public static ItemDatabase instance;

    public const string itemSpritesPath = "Sprites/RPG_inventory_icons/";

    private List<GameItem> database = new List<GameItem>();

    private JsonData itemData;

	void Start () {
        
        // Implement singleton pattern
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/ItemDatabase.json"));
        ConstructItemDatabase();

    }

    public GameItem GetItemById(int id)
    {
        foreach (GameItem item in database)
        {
            if (item.id == id)
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

    public void WriteToFile(Inventory inventory)
    {
        List<GameItem> items = inventory.GetGameItems();
        WriteToFile(inventory.inventoryFilePath, items.ToArray());
    }

    public void WriteToFile(string filePath, GameItem[] items)
    {
        string data = SerializeGameItems(items, true);
        File.WriteAllText(filePath, data);
    }

    public void WriteToFile(string filePath, JsonData json)
    {
        File.WriteAllText(filePath, json.ToJson());
    }

    public GameItem ReadOneFromFile(string filePath, string uuid)
    {
        JsonData itemJson = ReadOneFromFileRaw(filePath, uuid);

        if (itemJson != null)
            return DeserializeGameItem(itemJson);
        else
            return null;
    }

    public JsonData ReadOneFromFileRaw(string filePath, string uuid)
    {
        JsonData json = JsonMapper.ToObject(File.ReadAllText(filePath));

        foreach (JsonData data in json)
        {
            JsonData metadataJson = data["metadata"];

            try {
                string uuidJson = metadataJson["uuid"].ToString();
                if (uuidJson == uuid)
                    { return data; }
            } catch (KeyNotFoundException) {
                continue;
            }
        }

        return null;
    }

    public void WriteOneToFile(string filePath, GameItem item)
    {
        JsonData itemJson = JsonMapper.ToObject(SerializeGameItem(item));
        WriteOneToFileRaw(filePath, itemJson);
    }

    public void WriteOneToFileRaw(string filePath, JsonData item)
    {
        JsonData json = ReadFromFileRaw(filePath);

        bool didFindMatchingItem = false;

        for (int i = 0; i < json.Count; i++)
        {
            JsonData metadataJson = json[i]["metadata"];
            try {
                string uuidJson = metadataJson["uuid"].ToString();
                if (uuidJson == item["metadata"]["uuid"].ToString()) {
                    didFindMatchingItem = true;
                    item[i] = item;
                    break;
                }
            } catch (KeyNotFoundException) {
                continue;
            }
        }

        if (!didFindMatchingItem)
        {
            json.Add(item);
        }

        WriteToFile(filePath, json);

    }

    public List<GameItem> ReadFromFile(string filePath)
    {
        List<GameItem> items = new List<GameItem>();
        JsonData data = ReadFromFileRaw(filePath);
        foreach (JsonData itemData in data)
        {
            items.Add(DeserializeGameItem(itemData));
        }
        return items;
    }

    public JsonData ReadFromFileRaw(string filePath)
    {
        JsonData json = JsonMapper.ToObject(File.ReadAllText(filePath));
        return json;
    }

    string SerializeGameItems(GameItem[] gameItems, bool prettyPrint=true)
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = prettyPrint;

        writer.WriteArrayStart();
        foreach(GameItem gameItem in gameItems)
        {
            SerializeGameItem(gameItem, writer);
        }
        writer.WriteArrayEnd();

        return sb.ToString();
    }

    string SerializeGameItem(GameItem gameItem, bool prettyPrint=true)
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = prettyPrint;
        SerializeGameItem(gameItem, writer);
        return sb.ToString();
    }

    GameItem DeserializeGameItem(JsonData data)
    {

        int id = (int)data["id"];
        string spritePath = itemSpritesPath + data["spriteName"].ToString();
        Sprite sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
        string title = data["title"].ToString();
        string description = data["description"].ToString();
        double value = (double)data["value"];
        int maxStackSize = (int)data["maxStackSize"];
        int stackSize = (int)data["stackSize"];
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

        JsonData metadata = data["metadata"];

        int inventoryPosition;
        try {
            inventoryPosition = (int)metadata["inventoryPosition"];
        } catch (KeyNotFoundException) {
            inventoryPosition = -1;
        }

        string uuid;
        try {
            uuid = (string)metadata["uuid"];
        } catch (KeyNotFoundException) {
            uuid = System.Guid.NewGuid().ToString();
        }

        return new GameItem(id, uuid, sprite, title, description, 
            (float)value, maxStackSize, stackSize, (float) power, (float)defense,
            (float)vitality, (float)healAmount, types, inventoryPosition);
    }

    
    void SerializeGameItem(GameItem item, JsonWriter writer)
    {
        writer.WriteObjectStart();
        writer.WritePropertyName("id");
        writer.Write(item.id);
        writer.WritePropertyName("spriteName");
        writer.Write(item.sprite.name);
        writer.WritePropertyName("title");
        writer.Write(item.title);
        writer.WritePropertyName("description");
        writer.Write(item.description);
        writer.WritePropertyName("maxStackSize");
        writer.Write(item.maxStackSize);
        writer.WritePropertyName("stackSize");
        writer.Write(item.stackSize);
        writer.WritePropertyName("value");
        writer.Write(item.value);
        writer.WritePropertyName("stats"); // Write "stats"
        writer.WriteObjectStart();
        writer.WritePropertyName("power");
        writer.Write(item.power);
        writer.WritePropertyName("defense");
        writer.Write(item.defense);
        writer.WritePropertyName("vitality");
        writer.Write(item.vitality);
        writer.WritePropertyName("healAmount");
        writer.Write(item.healAmount);
        writer.WriteObjectEnd(); // end "stats"
        writer.WritePropertyName("types"); // write "types"
        writer.WriteArrayStart();
        foreach (string type in item.types.ToArray())
            writer.Write(type);
        writer.WriteArrayEnd(); // end types

        writer.WritePropertyName("metadata"); // metadata start
        writer.WriteObjectStart();
        writer.WritePropertyName("inventoryPosition");
        writer.Write(item.inventoryPosition);
        writer.WritePropertyName("uuid");
        writer.Write(item.uuid);
        writer.WriteObjectEnd(); // metadata end

        writer.WriteObjectEnd(); // end main object
    }

}
