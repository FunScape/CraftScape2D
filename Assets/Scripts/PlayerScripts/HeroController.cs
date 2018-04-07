using System.Collections;
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
        StartCoroutine(manager.GetUser((user) => {
            this.user = user;
            Debug.Log("Found user!");
            StartCoroutine(manager.GetCharacter(user.characterUrls[0], (character) => {
                Debug.Log("Found Character!");
                this.character = character;
                StartCoroutine(manager.GetInventory(character.inventoryUrls[0], (inventory) => {
                    Debug.Log("Found inventory!");
                    // this.inventories[0] = inventory;
                    GetComponent<HeroInventoryController>().inventory = inventory;
                    GetComponent<HeroInventoryController>().SetupInventory();
                    StartCoroutine(manager.GetStaticGameItems((staticItems) => {
                        GetComponent<HeroInventoryController>().inventory.database = new Database(this.inventories[0], staticItems);
                    }));
                }));
            }));
        }));
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

