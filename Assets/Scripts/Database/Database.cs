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

    private List<InventoryItem> GameItems = new List<InventoryItem>();
    private List<StaticGameItem> StaticGameItems = new List<StaticGameItem>();

    private const bool prettyPrintToFile = true;

    public Database(JsonData data)
    {
        foreach (JsonData item in data) {
            // if (JsonDataContainsKey(item, "static_game_item"))
                GameItems.Add(DeserializeGameItem(item));
            // else
            //     gameItems.Add(DeserializeStaticGameItem(item));
        } 
    }

    public Database(Inventory inventory, List<StaticGameItem> items)
    {
        StaticGameItems = items;
    }

    public Database()
    {
        ConstructDatabase();
    }

    public void Save(string filePath=null)
    {
        if (filePath == null)
            filePath = defaultSavePath;

        WriteToFile(filePath, GameItems.ToArray());
    }

    public void SaveStaticGameItems()
    {
        string filePath = defaultSavePath;
        WriteStaticItemsToFile(filePath, GameItems.ToArray());
    }

    // Reads in a json file and serializes the json file into a list of GameItems.
    void ConstructDatabase()
    {
        // JsonData itemData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/GameData/ItemDatabase.json"));
        // for (int i = 0; i < itemData.Count; i++)
        // {
        //     gameItems.Add(DeserializeStaticGameItem(itemData[i]));
        // }
    }

    // Finds a game item in database by ID.
    public InventoryItem GetItem(int id)
    {
        foreach (InventoryItem item in GameItems)
        {
            if (item.id == id)
                return item.Clone();
        }
        return null;
    }

    // Finds a game item in database by item name.
    public InventoryItem GetItem(string itemName)
    {
        foreach (InventoryItem item in GameItems)
        {
            if (item.name == itemName)
                return item.Clone();
        }
        return null;
    }

    public InventoryItem GetStaticItem(string itemName)
    {
        foreach (StaticGameItem item in StaticGameItems)
        {
            if (item.Name == itemName)
                return item.ToInventoryItem();
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

    public void WriteStaticItemsToFile(string filePath, InventoryItem[] items)
    {
        items = items.Where(item => item != null).ToArray();
        Dictionary<object,object>[] serializedItems = new Dictionary<object,object>[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            serializedItems[i] = SerializableInventoryItem.SerializableStaticItemDictionary(items[i]);
        }
        string data = SerializeGameItems(serializedItems);
        WriteToFile(filePath, data);
    }

    string SerializeGameItems(Dictionary<object, object>[] items)
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = Database.prettyPrintToFile;

        writer.WriteArrayStart();
        foreach(Dictionary<object, object> item in items)
        {
            JsonMapper.ToJson(item, writer);
            //string jsonString = JsonMapper.ToJson(item);
            //Debug.Log(jsonString);
        }
        writer.WriteArrayEnd();

        return sb.ToString();
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
            Dictionary<object, object> serializable = SerializableInventoryItem.SerializableStaticItemDictionary(gameItem);
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
        Debug.Log("DESERIALIZING DATA");
		IDictionary dict = data as IDictionary;
		foreach (string key in dict.Keys){
		    Debug.Log(key);
		}

        int id = (int)data["id"];
        int staticID = (int)data["static_game_item"]["id"];
        string spritePath = itemSpritesPath + data["static_game_item"]["sprite_name"].ToString();
        Sprite sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
        string name = data["static_game_item"]["name"].ToString();
        string description = data["static_game_item"]["description"].ToString();
        double value = (double)data["static_game_item"]["value"];
        int maxStackSize = (int)data["static_game_item"]["max_stack"];
        int stackSize = (int)data["stack_size"];
        double power = (double)data["static_game_item"]["power"];
        double defense = (double)data["static_game_item"]["defense"];
        double vitality = (double)data["static_game_item"]["vitality"];
        double healAmount = (double)data["static_game_item"]["heal_amount"];
        bool equipable = (bool)data["static_game_item"]["equipable"];
        int rarity = (int)data["static_game_item"]["rarity"];
        int minLevel = (int)data["static_game_item"]["min_level"];
        int baseDurability = (int)data["static_game_item"]["base_durability"];
        bool soulbound = (bool)data["static_game_item"]["soulbound"];
        int inventoryID = (int)data["inventory"];
        int createdByID = (int)data["created_by"];
        string createdByName = data["created_by_name"].ToString();
        int inventoryPosition = (int)data["inventory_position"];

        List<string> types = new List<string>();
        foreach(JsonData type in data["item_type"])
        {
           types.Add(type.ToString());
        }

        GameItem gameItem = GameItem.Parse(data);

        return InventoryItem.CreateInstance(gameItem);
    }

    // InventoryItem DeserializeStaticGameItem(JsonData data)
    // {
        //IDictionary dict = data as IDictionary;
        //foreach (string key in dict.Keys)
        //{
        //    Debug.Log(key);
        //}

    //     int staticID = (int)data["id"];
    //     string spritePath = itemSpritesPath + data["sprite_name"].ToString();
    //     Sprite sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
    //     string name = data["name"].ToString();
    //     string description = data["description"].ToString();
    //     double value = (double)data["value"];
    //     int maxStackSize = (int)data["max_stack"];
    //     double power = (double)data["power"];
    //     double defense = (double)data["defense"];
    //     double vitality = (double)data["vitality"];
    //     double healAmount = (double)data["heal_amount"];
    //     bool equipable = (bool)data["equipable"];
    //     int rarity = (int)data["rarity"];
    //     int minLevel = (int)data["min_level"];
    //     int baseDurability = (int)data["base_durability"];
    //     bool soulbound = (bool)data["soulbound"];

    //     List<string> types = new List<string>();
    //     foreach (JsonData type in data["item_type"])
    //     {
    //         types.Add(type.ToString());
    //     }

    //     return new InventoryItem(-1, staticID, sprite, name, description, (float)value, maxStackSize, 1, (float)power,
    //                              (float)defense, (float)vitality, (float)healAmount, equipable, rarity, minLevel, baseDurability, soulbound,
    //                              1, 1, "", types);
    // }

    public bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }



}
