using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;
using System;

public class APIManager : MonoBehaviour {

	public static APIManager instance { get; private set; }

	public static string token { get; private set; }

	public APIRoutes routes;

	Database database;

	private APIManager() {}

	public void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		if (routes == null)
			routes = APIRoutes.CreateInstance ("APIRoutes") as APIRoutes;
	}

	public IEnumerator Login(string username, string password, System.Action<bool> callback) {

		string url = routes.authorize;

		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("username", username);
		data.Add("password", password);

		UnityWebRequest www = PreparePOSTRequest(url, data);


		yield return www.SendWebRequest();
		try {
			JsonData response = HandleResponse(www);

			Debug.Log("Authentication successful!");

			APIManager.token = response["token"].ToString();
			Debug.Log (string.Format("TOKEN: {0}", APIManager.token));
			callback(true);

		} catch (System.Exception e) {
			Debug.Log (e);
			callback (false);
		}
	}

	public IEnumerator GetUser(System.Action<User> callback) {
		UnityWebRequest www = PrepareGETRequest(routes.user);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		JsonData userJson = data[0];

		callback(User.Parse(userJson));
	}

	public IEnumerator GetCharacter(int id, System.Action<Character> callback) {
		UnityWebRequest www = PrepareGETRequest(routes.character + "/" + id.ToString());

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

    public IEnumerator UpdateCharacterPosition(Character character, float x, float y, System.Action<Character> callback)
    {
        Dictionary<string, object> formData = new Dictionary<string, object>();
        formData.Add("x_pos", x);
        formData.Add("y_pos", y);

        UnityWebRequest www = PreparePUTRequest(routes.character + "/" + character.Id + "/", formData);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(Character.Parse(data));
    }

	public IEnumerator GetInventory(int id, System.Action<Inventory> callback)
	{
		UnityWebRequest www = PrepareGETRequest(routes.inventory + "/" + id.ToString());

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(Inventory.Parse(data));
	}

	public IEnumerator GetInventory(string url, System.Action<Inventory> callback)
	{
		UnityWebRequest www = PrepareGETRequest(url);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		Inventory inventory = Inventory.Parse(data);

		callback(inventory);
	}

	public IEnumerator UpdateInventory(Inventory inventory, System.Action<Inventory> callback)
	{
		Dictionary<string, object> formData = new Dictionary<string, object>();
		formData.Add("position", inventory.Position);
		formData.Add("character", inventory.CharacterId);
		formData.Add("size", inventory.Size);

		UnityWebRequest www = PreparePUTRequest(routes.inventory + inventory.Id.ToString() + "/", formData);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(Inventory.Parse(data));
	}

	public IEnumerator UpdateGameItem(GameItem item, System.Action<GameItem> callback)
	{
		Dictionary<string, object> formData = item.ToDict();

		UnityWebRequest www = PreparePUTRequest(routes.gameItem + item.Id + "/", formData);

		yield return www.SendWebRequest();

		try {
			JsonData data = HandleResponse(www);
			callback(GameItem.Parse(data));
		} catch (System.Exception) {
			callback (null);
		}

	}

	/* 
	 * 
	 * Create Game Item
	 * 
	 * */
	public IEnumerator CreateGameItem(GameItem item, System.Action<GameItem> callback)
	{
		Dictionary<string, object> formData = item.ToDict();
		formData.Add("static_game_item", item.StaticGameItemId);

		UnityWebRequest www = PreparePOSTRequest(routes.gameItem, formData);

		yield return www.SendWebRequest();

		try {
			JsonData data = HandleResponse(www);
			callback(GameItem.Parse(data));
		} catch (System.Exception) {
			callback (null);
		}

	}

	public IEnumerator GetEquipment(Character character, System.Action<Equipment> callback)
	{
		if (character.equipmentUrl == null)
			throw new System.MissingMemberException("character has no equipment URL");

		UnityWebRequest www = PrepareGETRequest(character.equipmentUrl);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(Equipment.Parse(data));
	}

	public IEnumerator UpdateEquipment(Equipment equipment, System.Action<Equipment> callback)
	{
		Dictionary<string, object> formData = new Dictionary<string, object> ();
		formData.Add("id", equipment.Id);
		if (equipment.MainHand != null)
			formData.Add("main_hand", new Dictionary<string, object>() { { "uuid", equipment.MainHand.Uuid } });
		if (equipment.Ring != null)
			formData.Add("ring", new Dictionary<string, object>() { { "uuid", equipment.Ring.Uuid } });
		if (equipment.Neck != null)
			formData.Add("neck", new Dictionary<string, object>() { { "uuid", equipment.Neck.Uuid } });
		if (equipment.Head != null)
			formData.Add("head", new Dictionary<string, object>() { { "uuid", equipment.Head.Uuid } });
		if (equipment.Shoulders != null)
			formData.Add("shoulders", new Dictionary<string, object>() { { "uuid", equipment.Shoulders.Uuid } });
		if (equipment.Chest != null)
			formData.Add("chest", new Dictionary<string, object>() { { "uuid", equipment.Chest.Uuid } });
		if (equipment.Back != null)
			formData.Add("back", new Dictionary<string, object>() { { "uuid", equipment.Back.Uuid } });
		if (equipment.Hands != null)
			formData.Add("hands", new Dictionary<string, object>() { { "uuid", equipment.Hands.Uuid } });
		if (equipment.Feet != null)
			formData.Add("feet", new Dictionary<string, object>() { { "uuid", equipment.Feet.Uuid } });
		if (equipment.Legs != null)
			formData.Add("legs", new Dictionary<string, object>() { { "uuid", equipment.Legs.Uuid } });

		UnityWebRequest www = PreparePUTRequest(routes.equipment + "/" + equipment.Id.ToString() + "/", formData);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(Equipment.Parse(data));
	}

	public IEnumerator DeleteGameItem(GameItem item, System.Action callback)
	{
		UnityWebRequest www = PrepareDELETERequest (routes.gameItem + "/" + item.Id.ToString () + "/");

		yield return www.SendWebRequest ();

		if (www.isHttpError || www.isNetworkError) {
			throw new System.Exception (www.error);
		} else {
			callback ();
		}
	}

	public IEnumerator GetStaticGameItems(System.Action<List<StaticGameItem>> callback) {
		UnityWebRequest www = PrepareGETRequest(routes.staticGameItem);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		List<StaticGameItem> items = new List<StaticGameItem>();
		foreach (JsonData item in data) {
			items.Add(StaticGameItem.Parse(item));
		}
		callback(items);
	}

    public IEnumerator GetSkill(int skillId, System.Action<JsonData> callback)
    {
        UnityWebRequest www = PrepareGETRequest(routes.skill + skillId.ToString() + "/");

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        callback(data);
    }

    public IEnumerator GetCharacterSkills(System.Action<List<Recipe>> callback) {
        UnityWebRequest www = PrepareGETRequest(routes.characterSkill);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);
        
        List<Recipe> recipes = new List<Recipe>();

        foreach (JsonData characterSkill in data)
        {
            StartCoroutine(GetSkill((int)characterSkill["skill"], (skillData) =>
            {
                recipes.Add(Recipe.Parse(skillData, true));
            }));
        }

        /*foreach (JsonData characterSkill in data) {
            recipes.Add(Recipe.Parse(characterSkill["skill"], true));
        }*/
        callback(recipes);
    }

    public IEnumerator GetAllSkills(System.Action<List<Recipe>> callback) {
        UnityWebRequest www = PrepareGETRequest(routes.skill);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);

        List<Recipe> skills = new List<Recipe>();
        foreach (JsonData skill in data)
        {
            skills.Add(Recipe.Parse(skill, false));
        }
        callback(skills);
    }

    public IEnumerator GetSkillDependencies(System.Action<List<List<object>>> callback) {
        UnityWebRequest www = PrepareGETRequest(routes.skillDependency);

        yield return www.SendWebRequest();

        JsonData data = HandleResponse(www);
        
        //skillDependencies holds three lists: A list of ints, holding parent skill ids, a list of ints, holding child skill ids, and a list of characters, representing unions or intersections.
        List<List<object>> skillDependencies = new List<List<object>>();
        skillDependencies.Add(new List<object>());
        skillDependencies.Add(new List<object>());
        skillDependencies.Add(new List<object>());

        foreach (JsonData dependency in data)
        {
            skillDependencies[0].Add((int)dependency["parent_skill"]);
            skillDependencies[1].Add((int)dependency["child_skill"]);
            skillDependencies[2].Add(((string)dependency["dependency_type"])[0]); //Since I can't convert between a single-character string and a char, I access the first character in the string and use that.
        }

        callback(skillDependencies);
    }

    public IEnumerator CreateCharacterSkill(Character character, Recipe skill)
    {
        Dictionary<string, object> formData = new Dictionary<string, object>
        {
            { "character", character.Id },
            { "skill", skill.id }

        };
        
        UnityWebRequest www = PreparePOSTRequest(routes.characterSkill, formData);

        yield return www.SendWebRequest();
    }

    public IEnumerator UpdateCharacterExperience(Character character)
    {
        Dictionary<string, object> formData = new Dictionary<string, object>
        {
            { "id", character.Id },
            { "experience", character.experience }
        };
        
        UnityWebRequest www = PreparePUTRequest(routes.character, formData);

        yield return www.SendWebRequest();
    }

    public IEnumerator UpdateCharacter(Character character)
    {
        Dictionary<string, object> data = character.ToDict();
        UnityWebRequest www = PreparePUTRequest(routes.character, data);

        yield return www.SendWebRequest();
    }

    UnityWebRequest PrepareGETRequest(string url) {
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		return www;
	}

	public UnityWebRequest PreparePOSTRequest(string url, Dictionary<string, object> formData) {
		JsonData jsonData = JsonMapper.ToJson(formData);
		return PreparePOSTRequest(url, jsonData.ToString());
	}

	public UnityWebRequest PreparePOSTRequest(string url, string json) {

		UnityWebRequest www = UnityWebRequest.Put(url, json);
		www.method = "POST";
		www.SetRequestHeader ("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	public UnityWebRequest PreparePUTRequest(string url, string json)
	{
		UnityWebRequest www = UnityWebRequest.Put(url, json);
		www.SetRequestHeader("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	public UnityWebRequest PreparePUTRequest(string url, Dictionary<string, object> formData)
	{
		JsonData jsonData = JsonMapper.ToJson(formData);
		return PreparePUTRequest(url, jsonData.ToString());
	}

	public UnityWebRequest PrepareDELETERequest(string url)
	{
		UnityWebRequest www = UnityWebRequest.Delete (url);
		www.SetRequestHeader("Content-Type", "application/json; charset=utf8");
		www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	JsonData HandleResponse(UnityWebRequest www) {
		if (www.isNetworkError || www.isHttpError) {
			throw new System.Exception (www.downloadHandler.text);
		}

		JsonData data = JsonMapper.ToObject(www.downloadHandler.text);

		return data;
	}

}

