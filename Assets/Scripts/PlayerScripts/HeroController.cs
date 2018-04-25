using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{

    [SerializeField]
    float walkSpeed = 5f;

    Rigidbody2D body;

    public User user;
    public Character character;
    Inventory[] inventories = new Inventory[5];

    Slider progressBar;
    Text progressBarText;
    GameObject loginContainer;

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

    private void OnApplicationQuit()
    {
        UpdateCharacterPosition();
    }

    void InitializeCharacter() {
		
		GameObject APIManager = GameObject.FindGameObjectWithTag("APIManager");
		APIManager manager = APIManager.GetComponent<APIManager>();
        loginContainer = GameObject.FindGameObjectWithTag("LoginContainer");

        // Init character without database interaction.
        if (PlayerPrefs.GetInt("IsLocalPlayer") == 1) {

            GetComponent<HeroInventoryController>().SetupInventory();
            GetComponent<EquipmentController>().equipment = Equipment.CreateInstance();

            // Move login form off screen
            loginContainer.GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);

			// Still need to get static game items...
			Debug.Log("Loading static items...");
			StartCoroutine(manager.GetStaticGameItems((staticItems) => {
				GameItemDatabase.instance.gameItems = staticItems;
			}));

        } else {

            progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
            progressBarText = GameObject.Find("ProgressBarText").GetComponent<Text>();

            int callbackCount = 6;

            progressBar.value = 1.0f / callbackCount;
            progressBarText.text = "Loading user...";

            StartCoroutine(manager.GetUser((user) => {
                this.user = user;

                progressBar.value = 2.0f / callbackCount;
                progressBarText.text = "Loading character...";

                StartCoroutine(manager.GetCharacter(user.characterUrls[0], (character) => {
                    this.character = character;

                    if (character.Position != null)
                        transform.position = new Vector3(character.Position.x, character.Position.y, 0);

                    progressBar.value = 3f / callbackCount;
                    progressBarText.text = "Loading inventory...";

                    StartCoroutine(manager.GetInventory(character.inventoryUrls[0], (inventory) => {

                        // Add inventory and do initial setup
                        GetComponent<HeroInventoryController>().SetupInventory(inventory);

                        progressBar.value = 4f / callbackCount;
                        progressBarText.text = "Loading equipment...";

                        StartCoroutine(manager.GetEquipment(character, (equipment) => {
                            character.equipment = equipment;
                            GameObject player = GameManager.GetLocalPlayer();
                            EquipmentController eController = player.GetComponent<EquipmentController>();
                            eController.equipment = equipment;

                            progressBar.value = 5f / callbackCount;
                            progressBarText.text = "Loading static game items...";

                            // Get static game items
                            Debug.Log("Loading static items...");
                            StartCoroutine(manager.GetStaticGameItems((staticItems) => {
								GameItemDatabase.instance.gameItems = staticItems;

                                progressBar.value = 1f;
                                progressBarText.text = "done!";

                                // Move login form off screen
                                loginContainer.GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);
                            }));
                        }));
                    }));
                }));
            }));

        }


        
    }

    // Updates the characters position in the database
    public void UpdateCharacterPosition()
    {
        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;

        Character character = GetComponent<HeroController>().character;

        APIManager manager = GameObject.FindWithTag("APIManager").GetComponent<APIManager>();

        StartCoroutine(manager.UpdateCharacterPosition(character, posX, posY, (c) => {
            Debug.LogFormat("Updated {0}'s position", character.Name);
            character.Position = c.Position;
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

