using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;

public class Database {

    string defaultSavePath {
        get { return Application.streamingAssetsPath + "/GameData/ItemDatabase.json"; }
    }

    public const string itemSpritesPath = "Sprites/RPG_inventory_icons/";

    private List<InventoryItem> gameItems = new List<InventoryItem>();

    private const bool prettyPrintToFile = true;

    public Database(JsonData data)
    {
        foreach (JsonData item in data) {
            gameItems.Add(DeserializeGameItem(item));
        }
    }

    public Database()
    {
        ConstructDatabase();
    }

    public void Save(string filePath = null)
    {
        if (filePath == null)
            filePath = defaultSavePath;

        WriteToFile(filePath, gameItems.ToArray());
    }

    // Reads in a json file and serializes the json file into a list of GameItems.
    void ConstructDatabase()
    {
        JsonData itemData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/GameData/ItemDatabase.json"));
        for (int i = 0; i < itemData.Count; i++)
        {
            gameItems.Add(DeserializeGameItem(itemData[i]));
        }
    }

    // Finds a game item in database by ID.
    public InventoryItem GetItem(int id)
    {
        foreach (InventoryItem item in gameItems)
        {
            if (item.id == id)
                return item.Clone();
        }
        return null;
    }

    // Finds a game item in database by item name.
    public InventoryItem GetItem(string itemName)
    {
        foreach (InventoryItem item in gameItems)
        {
            if (item.name == itemName)
                return item.Clone();
        }
        return null;
    }

    public void WriteToFile(string filePath, InventoryItem[] items)
    {
        items = items.Where(item => item != null).ToArray();
        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
        File.WriteAllText(filePath, SerializeGameItems(items));
    }

    public void WriteToFile(string filePath, JsonData jsonData)
    {
        StringBuilder builder = new StringBuilder();
        JsonWriter writer = new JsonWriter(builder);
        writer.PrettyPrint = true;
        JsonMapper.ToJson(jsonData, writer);
        
        File.WriteAllText(filePath, builder.ToString());
    }

    public void WriteToFile(string filePath, string json)
    {
        File.WriteAllText(filePath, json);
    }

    public void WriteOneToFile(string filePath, InventoryItem item)
    {
        JsonData itemJson = JsonMapper.ToObject(SerializeGameItem(item));
        WriteOneToFile(filePath, itemJson);
    }

    public void WriteOneToFile(string filePath, JsonData itemJson)
    {
        JsonData json = ReadJsonFromFile(filePath);

        bool didFindMatchingItem = false;

        int i = 0;
        foreach (JsonData item in json)
        {
            try
            {
                if (item["metadata"]["uuid"].ToString() == itemJson["metadata"]["uuid"].ToString())
                {
                    didFindMatchingItem = true;
                    json[i] = itemJson;
                    break;
                }
            }
            catch (KeyNotFoundException)
            {
                continue;
            }

            i++;
        }

        if (!didFindMatchingItem)
        {
            json.Add(itemJson);
        }

        WriteToFile(filePath, json);

    }

    public List<InventoryItem> ReadFromFile(string filePath)
    {
        List<InventoryItem> items = new List<InventoryItem>();
        JsonData data = ReadJsonFromFile(filePath);
        foreach (JsonData itemData in data)
        {
            items.Add(DeserializeGameItem(itemData));
        }
        return items;
    }

    public JsonData ReadJsonFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
        return JsonMapper.ToObject(File.ReadAllText(filePath));
    }

    public InventoryItem ReadOneFromFile(string filePath, string uuid)
    {
        JsonData itemJson = ReadOneJsonFromFile(filePath, uuid);

        if (itemJson != null)
            return DeserializeGameItem(itemJson);
        else
            return null;
    }

    public JsonData ReadOneJsonFromFile(string filePath, string uuid)
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }

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

    string SerializeGameItems(InventoryItem[] gameItems)
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = Database.prettyPrintToFile;

        writer.WriteArrayStart();
        foreach(InventoryItem gameItem in gameItems)
        {
            SerializableInventoryItem serializable = new SerializableInventoryItem(gameItem);
            JsonMapper.ToJson(serializable, writer);
        }
        writer.WriteArrayEnd();

        return sb.ToString();
    }

    string SerializeGameItems(List<InventoryItem> gameItems)
    {
        return SerializeGameItems(gameItems.ToArray());
    }

    string SerializeGameItem(InventoryItem item)
    {
        StringBuilder builder = new StringBuilder();
        JsonWriter writer = new JsonWriter(builder);
        writer.PrettyPrint = Database.prettyPrintToFile;

        SerializableInventoryItem serialized = new SerializableInventoryItem(item);

        JsonMapper.ToJson(serialized, writer);

        return builder.ToString();
    }  

    InventoryItem DeserializeGameItem(JsonData data)
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

        return new InventoryItem(id, uuid, sprite, title, description, 
            (float)value, maxStackSize, stackSize, (float) power, (float)defense,
            (float)vitality, (float)healAmount, types, inventoryPosition);
    }

 

}
