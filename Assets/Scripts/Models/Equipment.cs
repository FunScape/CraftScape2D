using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class Equipment : ScriptableObject {

    public int Id;
    public string Url;
    public GameItem Ring;
    public GameItem Neck;
    public GameItem Head;
    public GameItem Chest;
    public GameItem Weapon;
    public GameItem Back;
    public GameItem Hands;
    public GameItem Feet;
    public GameItem Legs;

    public static Equipment CreateInstance()
    {
        Equipment equipment = ScriptableObject.CreateInstance("Equipment") as Equipment;
        equipment.Init();
        return equipment;
    }

    void Init()
    {
        Id = -1;
    }

    public static Equipment Parse(JsonData data)
    {
        Equipment equipment = Equipment.CreateInstance();
        equipment.Id = (int) data["id"];
        equipment.Ring = GameItem.Parse(data["ring"]);
        equipment.Neck = GameItem.Parse(data["neck"]);
        equipment.Head = GameItem.Parse(data["head"]);
        equipment.Chest = GameItem.Parse(data["chest"]);
        equipment.Weapon = GameItem.Parse(data["weapon"]);
        equipment.Back = GameItem.Parse(data["back"]);
        equipment.Hands = GameItem.Parse(data["hands"]);
        equipment.Feet = GameItem.Parse(data["feet"]);
        equipment.Legs = GameItem.Parse(data["legs"]);
        return equipment;
    }

    public string ToJson()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("ring", Ring.ToJson());
        data.Add("neck", Neck.ToJson());
        data.Add("head", Head.ToJson());
        data.Add("chest", Chest.ToJson());
        data.Add("weapon", Weapon.ToJson());
        data.Add("back", Back.ToJson());
        data.Add("hands", Hands.ToJson());
        data.Add("feet", Feet.ToJson());
        data.Add("legs", Legs.ToJson());
        return JsonMapper.ToJson(data).ToString();
    }

}