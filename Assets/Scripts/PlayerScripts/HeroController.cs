﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    [SerializeField]
    float walkSpeed = 5f;

    Rigidbody2D body;

    User user;
    Character character;
    Inventory[] inventories = new Inventory[5];

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InitializeCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void InitializeCharacter() {
        GameObject APIManager = GameObject.FindGameObjectWithTag("APIManager");
        APIManager manager = APIManager.GetComponent<APIManager>();
        Debug.Log("Loading user...");
        StartCoroutine(manager.GetUser((user) => {
            this.user = user;

            Debug.Log("Loading character...");
            StartCoroutine(manager.GetCharacter(user.characterUrls[0], (character) => {
                this.character = character;
                
                Debug.Log("Loading character inventory...");
                StartCoroutine(manager.GetInventory(character.inventoryUrls[0], (inventory) => {
                    GetComponent<HeroInventoryController>().inventory = inventory;
                    GetComponent<HeroInventoryController>().SetupInventory();
                    GameObject.Find("LoginForm").GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);

					Debug.Log("Loading character equipment...");
					StartCoroutine(manager.GetEquipment(character, (equipment) => {
						character.equipment = equipment;
						GameObject player = GameObject.FindWithTag("Player");
						EquipmentController eController = player.GetComponent<EquipmentController>();
						eController.equipment = equipment;

						Debug.Log("Loading static items...");
						StartCoroutine(manager.GetStaticGameItems((staticItems) => {
							GameItemDatabase.instance.gameItems = staticItems;
						}));
					}));
                }));
            }));
        }));

        // Get static game items
        
    }

    private void Move()
    {
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        float posX = 0f;
        float posY = 0f;

        if (moveRight)
        {
           posX = walkSpeed * Time.deltaTime;
           transform.localScale.Set(-1f, 1f, transform.localScale.z);
           
        }
        else if (moveLeft)
        {
           posX = -walkSpeed * Time.deltaTime;
           transform.localScale.Set(1f, 1f, transform.localScale.z);
        }

        if (moveUp)
        {
           posY = walkSpeed * Time.deltaTime;
        }
        else if (moveDown)
        {
           posY = -walkSpeed * Time.deltaTime;
        }

        Vector2 position = new Vector2(posX, posY);
        
        position = new Vector2(position.x + transform.position.x, position.y + transform.position.y);
        
        body.MovePosition(position);
    }

}

