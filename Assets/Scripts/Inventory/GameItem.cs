using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem {

	private Sprite _sprite;
	public Sprite Sprite { get { return _sprite; } }

	private int _id;
	public int Id { get { return _id; } }

	private int _maxStackSize;
	public int MaxStackSize { get { return _maxStackSize; } }

	private string _title;
	public string Title { get { return _title; } }

	private string _description;
	public string Description { get { return _description; } }

	private float _value;
    public float Value { get{return _value;} }

	private float _power;
	public float Power { get {return _power;} }

	private float _defense;
	public float Defense { get { return _defense; } }

	private float _vitality;
	public float Vitality {get{return _vitality;}}

	private float _healAmount = 0f;
	public float HealAmount {get{return _healAmount;}}

	private List<string> _types = new List<string>();
	public List<string> Types {get{return _types;}}

	public GameItem(int id, Sprite sprite, string title, string description, 
	float value, int maxStackSize, float power, float defense, float vitality,
	float healAmount, List<string> types)
	{
		_id = id;
		_sprite = sprite;
		_title = title;
		_description = description;
		_maxStackSize = maxStackSize;
        _value = value;
		_power = power;
		_defense = defense;
		_vitality = vitality;
		_healAmount = healAmount;
		_types = types;
	}

    
}


