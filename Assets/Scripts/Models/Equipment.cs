using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class Equipment : ScriptableObject {

	public bool Dirty;

    public int Id;
    public string Url;
    public GameItem Ring { get { return ring; } set { ring = value; Dirty = true; } }
    public GameItem Neck { get { return neck; } set { neck = value; Dirty = true; } }
    public GameItem Head { get { return head; } set { head = value; Dirty = true; } }
    public GameItem Shoulders { get { return shoulders; } set { shoulders = value; Dirty = true; } }
    public GameItem Chest { get { return chest; } set { chest = value; Dirty = true; } }
	public GameItem MainHand { get { return mainHand; } set { mainHand = value; Dirty = true; } }
    public GameItem Back { get { return back; } set { back = value; Dirty = true; } }
    public GameItem Hands { get { return hands; } set { hands = value; Dirty = true; } }
    public GameItem Feet { get { return feet; } set { feet = value; Dirty = true; } }
    public GameItem Legs { get { return legs; } set { legs = value; Dirty = true; } }

    private GameItem ring;
    private GameItem neck;
    private GameItem head;
    private GameItem shoulders;
    private GameItem chest;
    private GameItem mainHand;
    private GameItem back;
    private GameItem hands;
    private GameItem feet;
    private GameItem legs;

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
        equipment.Shoulders = GameItem.Parse(data["shoulders"]);
        equipment.Chest = GameItem.Parse(data["chest"]);
		equipment.MainHand = GameItem.Parse(data["main_hand"]);
        equipment.Back = GameItem.Parse(data["back"]);
        equipment.Hands = GameItem.Parse(data["hands"]);
        equipment.Feet = GameItem.Parse(data["feet"]);
        equipment.Legs = GameItem.Parse(data["legs"]);
        return equipment;
    }

    public JsonData ToJson()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        if (Ring != null)
            data.Add("ring", Ring.ToDict());
        if (Neck != null)
            data.Add("neck", Neck.ToDict());
        if (Head != null)
            data.Add("head", Head.ToDict());
        if (Shoulders != null)
            data.Add("shoulders", Shoulders.ToDict());
        if (Chest != null)
            data.Add("chest", Chest.ToDict());
        if (MainHand != null)
		    data.Add("main_hand", MainHand.ToDict());
        if (Back != null)
            data.Add("back", Back.ToDict());
        if (Hands != null)
            data.Add("hands", Hands.ToDict());
        if (Feet != null)
            data.Add("feet", Feet.ToDict());
        if (Legs != null)
            data.Add("legs", Legs.ToDict());
        return JsonMapper.ToJson(data);
    }

    public void Save()
    {
        if (PlayerPrefs.GetInt("IsLocalPlayer") == 1)
            return;

        APIManager manager = GameObject.FindWithTag("APIManager").GetComponent<APIManager>();
//        EquipmentController controller = GameManager.GetLocalPlayer().GetComponent<EquipmentController>();

        manager.StartCoroutine(manager.UpdateEquipment(this, (equipment) => {
            ring = equipment.ring;
            neck = equipment.neck;
            head = equipment.head;
            shoulders = equipment.shoulders;
            chest = equipment.chest;
            mainHand = equipment.mainHand;
            back = equipment.back;
            hands = equipment.hands;
            feet = equipment.feet;
            legs = equipment.legs;
        }));

    }

}