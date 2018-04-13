using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;

[CreateAssetMenu()]
public class APIRoutes : ScriptableObject {
	// public string BASE_API_URL = "https://foostats.com/api";
	public string baseURL = "localhost:8000";
	public string authorize { get { return baseURL + "/api/authorize/"; } }
	public string user { get { return baseURL + "/api/user/"; } }
	public string character { get { return baseURL + "/api/character/"; } }
	public string inventory { get { return baseURL + "/api/inventory/"; } }
	public string staticGameItem { get { return baseURL + "/api/static_game_item/"; } }
	public string gameItem { get { return baseURL + "/api/game_item/"; } }
	public string skill { get { return baseURL + "/api/skill/"; } }
	public string skillDependency { get { return baseURL + "/api/skill_dependency/"; } }
	public string characterSkill { get { return baseURL + "/api/character_skill/"; } }
	public string gameItemModifier { get { return baseURL + "/api/game_item_modifier/"; } }
	public string itemModifier { get { return baseURL + "/api/item_modifier/"; } }
	public string staticItemModifier { get { return baseURL + "/api/static_item_modifier"; } }
	public string gameItemType { get { return baseURL + "/api/game_item_type/"; } }
	public string staticItemTypeModifier { get { return baseURL + "/api/static_item_type_modifier"; } }
	public string equipment { get { return baseURL + "/api/equipment"; } }
}


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

		callback(Inventory.Parse(data));
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
		Dictionary<string, object> formData = new Dictionary<string, object>();
		formData.Add("inventory", item.InventoryId);
		formData.Add("inventory_position", item.Position);
		formData.Add("stack_size", item.StackSize);
		formData.Add("created_by", item.CreatedById);
		formData.Add("static_game_item", item.StaticGameItemId);

		UnityWebRequest www = PreparePUTRequest(routes.gameItem + item.Id + "/", formData);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(GameItem.Parse(data));
	}

	public IEnumerator CreateGameItem(GameItem item, System.Action<GameItem> callback)
	{
		Dictionary<string, object> formData = new Dictionary<string, object>();
		formData.Add("inventory", item.InventoryId);
		formData.Add("inventory_position", item.Position);
		formData.Add("stack_size", item.StackSize);
		formData.Add("created_by", item.CreatedById);
		formData.Add("static_game_item", item.staticGameItem.Id);

		UnityWebRequest www = PreparePOSTRequest(routes.gameItem, formData);

		yield return www.SendWebRequest();

		JsonData data = HandleResponse(www);

		callback(GameItem.Parse(data));
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
		UnityWebRequest www = PreparePUTRequest(routes.equipment + "/" + equipment.Id.ToString() + "/", equipment.ToJson());

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
			//			callback (false);
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

	UnityWebRequest PrepareGETRequest(string url) {
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		return www;
	}

	UnityWebRequest PreparePOSTRequest(string url, Dictionary<string, object> formData) {
		JsonData jsonData = JsonMapper.ToJson(formData);
		return PreparePOSTRequest(url, jsonData.ToString());
	}

	UnityWebRequest PreparePOSTRequest(string url, string json) {

		UnityWebRequest www = UnityWebRequest.Put(url, json);
		www.method = "POST";
		www.SetRequestHeader ("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	UnityWebRequest PreparePUTRequest(string url, string json)
	{
		UnityWebRequest www = UnityWebRequest.Put(url, json);
		www.SetRequestHeader("Content-Type", "application/json");
		if (APIManager.token != null)
			www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	UnityWebRequest PreparePUTRequest(string url, Dictionary<string, object> formData)
	{
		JsonData jsonData = JsonMapper.ToJson(formData);
		return PreparePUTRequest(url, jsonData.ToString());
	}

	UnityWebRequest PrepareDELETERequest(string url)
	{
		UnityWebRequest www = UnityWebRequest.Delete (url);
		www.SetRequestHeader ("Content-Type", "application/json; charset=utf8");
		www.SetRequestHeader("Authorization", string.Format("Token {0}", APIManager.token));
		www.chunkedTransfer = false;
		return www;
	}

	JsonData HandleResponse(UnityWebRequest www) {
		if (www.isNetworkError || www.isHttpError)
			throw new System.Exception(www.error);

		// Debug.Log(www.downloadHandler.text);

		JsonData data = JsonMapper.ToObject(www.downloadHandler.text);

		return data;
	}

}

