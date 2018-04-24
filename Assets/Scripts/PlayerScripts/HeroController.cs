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

        // Init character without database interaction.
        if (PlayerPrefs.GetInt("IsLocalPlayer") == 1) {

			Inventory inventory = Inventory.CreateInstance ();

			// Setup inventory with no starting items
			GetComponent<HeroInventoryController>().inventory = inventory;

            GetComponent<HeroInventoryController>().SetupInventory();

            // Move login form off screen
            GameObject.Find("LoginForm").GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);

			// Still need to get static game items...
			Debug.Log("Loading static items...");
			StartCoroutine(manager.GetStaticGameItems((staticItems) => {
				GameItemDatabase.instance.gameItems = staticItems;
			}));

        } else {
			
            Debug.Log("Loading user...");
            StartCoroutine(manager.GetUser((user) => {
                this.user = user;

                Debug.Log("Loading character...");
                StartCoroutine(manager.GetCharacter(user.characterUrls[0], (character) => {
                    this.character = character;
                    
                    Debug.Log("Loading character inventory...");
                    StartCoroutine(manager.GetInventory(character.inventoryUrls[0], (inventory) => {

                        Debug.Log("Loading character equipment...");
                        StartCoroutine(manager.GetEquipment(character, (equipment) => {
                            character.equipment = equipment;
                            GameObject player = GameObject.FindWithTag("Player");
                            EquipmentController eController = player.GetComponent<EquipmentController>();
                            eController.equipment = equipment;

                            // Get static game items
                            Debug.Log("Loading static items...");
                            StartCoroutine(manager.GetStaticGameItems((staticItems) => {
								GameItemDatabase.instance.gameItems = staticItems;

								// Add inventory and do initial setup
								GetComponent<HeroInventoryController>().inventory = inventory;
								GetComponent<HeroInventoryController>().SetupInventory();

                                //Get character skills
                                Debug.Log("Loading character skills...");
                                StartCoroutine(manager.GetCharacterSkills((recipes) => {

                                    //Find PlayerRecipeBookController
                                    PlayerRecipeBookController recipeBookController = player.GetComponent<PlayerRecipeBookController>();

                                    //Load recipes
                                    recipeBookController.recipeBook.Load();

                                    //Connect inventory to recipe book controller and layout recipe book.
                                    recipeBookController.FindInventory();

                                    //Layout recipe book
                                    recipeBookController.LayoutRecipeBook();

                                    //Get all skills
                                    Debug.Log("Loading all skills...");
                                    StartCoroutine(manager.GetAllSkills((skills) => {
                                        PlayerSkillController skillController = player.GetComponent<PlayerSkillController>();

                                        foreach (Recipe skill in skills)
                                        {
                                            SkillNode node = new SkillNode(skill);
                                            if (recipeBookController.SearchRecipeBook(skill.id))
                                                node.recipe.canCraft = true;
                                            skillController.skillTrees[skill.skillType].skills.Add(node);
                                        }

                                        //Get skill dependencies
                                        Debug.Log("Loading skill dependencies...");
                                        StartCoroutine(manager.GetSkillDependencies((dependencies) =>
                                        {//dependencies holds three lists: A list of ints, holding parent skill ids, a list of ints, holding child skill ids, and a list of characters, representing unions or intersections.
                                            for (int i = 0; i < dependencies[0].Count; i++)
                                            {
                                                SkillNode parentNode = skillController.FindSkillNodeById((int)dependencies[0][i]);
                                                SkillNode childNode = skillController.FindSkillNodeById((int)dependencies[1][i]);
                                                SkillNode dependencyNode;
                                                char dependencyType = (char)dependencies[2][i];
                                                int depNodeId = 0; //-1 if the dependency is a union, -2 if the dependency is an intersection

                                                if (dependencyType == 'U')
                                                    depNodeId = -1;
                                                if (dependencyType == 'I')
                                                    depNodeId = -2;
                                                
                                                //If the dependency node has already been created, it will be in childNode's dependency list, and it will be the only node of its type there.
                                                dependencyNode = childNode.FindDependencyById(depNodeId);
                                                if (dependencyNode == null)
                                                {
                                                    dependencyNode = new SkillNode(dependencyType);
                                                    skillController.skillTrees[childNode.recipe.skillType].skills.Add(dependencyNode);
                                                    childNode.dependencies.Add(dependencyNode);
                                                    dependencyNode.children.Add(childNode);
                                                }

                                                parentNode.children.Add(dependencyNode);
                                                dependencyNode.dependencies.Add(parentNode);
                                            }

                                            //Now that the dependencies are all established, iterate over all the nodes and eliminate unnecessary union and intersection nodes (e.g., union/intersection nodes with only one dependency.
                                            foreach (string key in skillController.skillTrees.Keys)
                                            {
                                                List<SkillNode> skillList = skillController.skillTrees[key].skills;
                                                List<SkillNode> nodesToBeDeleted = new List<SkillNode>();

                                                foreach (SkillNode node in skillList)
                                                {
                                                    if (node.getId() < 0)
                                                    {
                                                        if (node.dependencies.Count == 1)
                                                        {//Make connections between nodes on either side, remove connections to this node, and delete this node.
                                                            SkillNode depNode = node.dependencies[0];
                                                            depNode.children.Remove(node);
                                                            foreach (SkillNode childNode in node.children)
                                                            {
                                                                depNode.children.Add(childNode);
                                                                childNode.dependencies.Add(depNode);

                                                                childNode.dependencies.Remove(node);
                                                            }

                                                            nodesToBeDeleted.Add(node);
                                                        }
                                                    }
                                                }

                                                foreach (SkillNode node in nodesToBeDeleted)
                                                {
                                                    skillList.Remove(node);
                                                }
                                            }

                                            foreach (string key in skillController.skillTrees.Keys)
                                            {
                                                skillController.skillTrees[key].topologicalSort();
                                            }

                                            skillController.LayoutSkillMenu();

                                            // Move login form off screen
                                            GameObject.Find("LoginForm").GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);
                                        }));
                                    }));
                                }));
                            }));
                        }));
                    }));
                }));
            }));

        }


        
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

