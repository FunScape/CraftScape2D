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
    public GameItem Chest { get { return chest; } set { chest = value; Dirty = true; } }
	public GameItem MainHand { get { return mainHand; } set { mainHand = value; Dirty = true; } }
    public GameItem Back { get { return back; } set { back = value; Dirty = true; } }
    public GameItem Hands { get { return hands; } set { hands = value; Dirty = true; } }
    public GameItem Feet { get { return feet; } set { feet = value; Dirty = true; } }
    public GameItem Legs { get { return legs; } set { legs = value; Dirty = true; } }

    private GameItem ring;
    private GameItem neck;
    private GameItem head;
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
        equipment.Chest = GameItem.Parse(data["chest"]);
		equipment.MainHand = GameItem.Parse(data["main_hand"]);
        equipment.Back = GameItem.Parse(data["back"]);
        equipment.Hands = GameItem.Parse(data["hands"]);
        equipment.Feet = GameItem.Parse(data["feet"]);
        equipment.Legs = GameItem.Parse(data["legs"]);
        return equipment;
    }

    public string ToJson()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        if (Ring != null)
            data.Add("ring", Ring.Id);
        if (Neck != null)
            data.Add("neck", Neck.Id);
        if (Head != null)
            data.Add("head", Head.Id);
        if (Chest != null)
            data.Add("chest", Chest.Id);
        if (MainHand != null)
		    data.Add("main_hand", MainHand.Id);
        if (Back != null)
            data.Add("back", Back.Id);
        if (Hands != null)
            data.Add("hands", Hands.Id);
        if (Feet != null)
            data.Add("feet", Feet.Id);
        if (Legs != null)
            data.Add("legs", Legs.Id);
        return JsonMapper.ToJson(data).ToString();
    }

    public void Save()
    {
        APIManager manager = GameObject.FindWithTag("APIManager").GetComponent<APIManager>();
        EquipmentController controller = GameObject.FindWithTag("Player").GetComponent<EquipmentController>();

        controller.StartCoroutine(manager.UpdateEquipment(this, (equipment) => {
            ring = equipment.ring;
            neck = equipment.neck;
            head = equipment.head;
            chest = equipment.chest;
            mainHand = equipment.mainHand;
            back = equipment.back;
            hands = equipment.hands;
            feet = equipment.feet;
            legs = equipment.legs;
        }));

    }

}