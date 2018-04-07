using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;

public class APIRoute : Object {
    public const string BASE_API_URL = "https://foostats.com/api";
    public const string authorize = BASE_API_URL + "/authorize/";
    public const string user = BASE_API_URL + "/user/";
    public const string character = BASE_API_URL + "/character/";
    public const string inventory = BASE_API_URL + "/inventory/";
    public const string staticGameItem = BASE_API_URL + "/static_game_item/";
    public const string gameItem = BASE_API_URL + "/game_item/";
    public const string skill = BASE_API_URL + "/skill/";
    public const string skillDependency = BASE_API_URL + "/skill_dependency/";
    public const string characterSkill = BASE_API_URL + "/character_skill/";
    public const string gameItemModifier = BASE_API_URL + "/game_item_modifier/";
    public const string itemModifier = BASE_API_URL + "/item_modifier/";
    public const string staticItemModifier = BASE_API_URL + "/static_item_modifier";
    public const string gameItemType = BASE_API_URL + "/game_item_type/";
    public const string staticItemTypeModifier = BASE_API_URL + "/static_item_type_modifier";
}


public class APIManager : MonoBehaviour {

    public static APIManager instance { get; private set; }

    public static string token { get; private set; }

    private Database database;

    private APIManager() {}

    public void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        // StartCoroutine(GetStaticGameItems());
    }

    public IEnumerator Login(string username, string password, System.Action callback) {

		string url = APIRoute.authorize;
		
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("username", username);
		data.Add("password", password);
		
        UnityWebRequest www = PreparePOSTRequest(url, data);
		

        yield return www.SendWebRequest();
        
        JsonData response = HandleResponse(www);

        Debug.Log("Authentication successful!");

        if (callback != null)
        {
            APIManager.token = response["token"].ToString();
            callback();
        }
    }

    public IEnumerator GetUser(System.Action<User> callback) {
        UnityWebRequest www = PrepareGETRequest(APIRoute.user);
        
        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        JsonData userJson = data[0];
        
        callback(User.Parse(userJson));
    }

    public IEnumerator GetCharacter(int id, System.Action<Character> callback) {
        UnityWebRequest www = PrepareGETRequest(APIRoute.character + "/" + id.ToString());

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(Character.Parse(data));
    }

    public IEnumerator GetCharacter(string url, System.Action<Character> callback) {
        UnityWebRequest www = PrepareGETRequest(url);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(Character.Parse(data));
    }

    public IEnumerator GetInventory(int id, System.Action<Inventory> callback)
    {
        UnityWebRequest www = PrepareGETRequest(APIRoute.inventory + "/" + id.ToString());

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(Inventory.Parse(data));
    }

    public IEnumerator GetInventory(string url, System.Action<Inventory> callback)
    {
        UnityWebRequest www = PrepareGETRequest(url);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(Inventory.Parse(data));
    }

    // public IEnumerator GetInventoryItems(int inventoryId, System.Action<InventoryItem[]> callback) 
    // {

    // }

    // public IEnumerator GetInventoryItem(int itemId, System.Action<InventoryItem> callback)
    // {

    // }

    public IEnumerator GetStaticGameItems(System.Action<List<StaticGameItem>> callback) {
        UnityWebRequest www = PrepareGETRequest(APIRoute.staticGameItem);
        
        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        List<StaticGameItem> items = new List<StaticGameItem>();
        foreach (JsonData item in data) {
            items.Add(StaticGameItem.Parse(item));
        }
        callback(items);
    }

    UnityWebRequest PrepareGETRequest(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        if (APIManager.token != null)
            www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
        return www;
    }

    UnityWebRequest PreparePOSTRequest(string url, Dictionary<string, object> formData) {

        JsonData jsonData = JsonMapper.ToJson(formData);

        UnityWebRequest www = UnityWebRequest.Put(url, jsonData.ToString());
		www.method = "POST";
		www.SetRequestHeader ("Content-Type", "application/json");
        if (APIManager.token != null)
            www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
        return www;
    }

    JsonData HandleResponse(UnityWebRequest www) {
        if (www.isNetworkError || www.isHttpError)
            throw new System.Exception(www.error);

        Debug.Log(www.downloadHandler.text);
        
        JsonData data = JsonMapper.ToObject(www.downloadHandler.text);

        return data;
    }

}

