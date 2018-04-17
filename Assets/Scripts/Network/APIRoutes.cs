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
	// public string baseURL = "https://foostats.com/api";
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