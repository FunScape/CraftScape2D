using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;

public class APIRoute : Object {
    public const string BASE_URL = "http://localhost:8000/";
    public const string authorize = BASE_URL + "/api/authorize/";
    public const string user = BASE_URL + "/user/";
    public const string character = BASE_URL + "/character/";
    public const string staticGameItem = BASE_URL + "/static_game_item/";
    public const string gameItem = BASE_URL + "/game_item/";
    public const string skill = BASE_URL + "/skill/";
    public const string skillDependency = BASE_URL + "/skill_dependency/";
    public const string characterSkill = BASE_URL + "/character_skill/";
    public const string gameItemModifier = BASE_URL + "/game_item_modifier/";
    public const string itemModifier = BASE_URL + "/item_modifier/";
    public const string staticItemModifier = BASE_URL + "/static_item_modifier";
    public const string gameItemType = BASE_URL + "/game_item_type/";
    public const string staticItemTypeModifier = BASE_URL + "/static_item_type_modifier";
}


public class APIManager : MonoBehaviour {

    public static APIManager instance { get; private set; }

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
        StartCoroutine(GetStaticGameItemsAsync());
    }

    public IEnumerator GetStaticGameItemsAsync() {
        UnityWebRequest request = UnityWebRequest.Get(APIRoute.staticGameItem);
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        } else {
            JsonData data = JsonMapper.ToJson(request.downloadHandler.text);
            database = new Database(data);
            database.Save();
        }
    }

}

