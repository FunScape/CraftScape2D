using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour {

    public GameObject skillMenuPanelPrefab;

    public GameObject skillMenuPanel;

    public GameObject skillNodePrefab;

    public GameObject ingredientSlotPrefab;

    public Dictionary<string, SkillTree> skillTrees;

    SkillTree selectedTree;

    SkillNode selectedSkill;

    bool showSkillMenu;

    float cameraHeight;
    float cameraWidth;

    protected const string treeScrollViewName = "Scroll View";
    protected const string treeScrollViewPortName = "Viewport";
    protected const string treePanelName = "TreePanel";
    protected const string selectionPanelName = "SelectionPanel";
    protected const string selectionImageName = "SelectionImage";
    protected const string selectionTextName = "SelectionText";
    protected const string selectionTextBackgroundName = "SelectionTextBackground";
    protected const string ingredientsPanelName = "IngredientsPanel";
    protected const string selectionButtonName = "SelectionButton";
    protected const string ingredientImageName = "IngredientImage";
    protected const string ingredientTextName = "IngredientCount";

    protected const string spritePath = "Sprites/";
    protected const string unlockedSpriteName = "unlocked";
    protected const string lockedSpriteName = "locked";

    // Use this for initialization
    void Start () {
        cameraHeight = Camera.main.pixelHeight;
        cameraWidth = Camera.main.pixelWidth;

        showSkillMenu = true;

        GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");
        
        skillMenuPanel = Instantiate(skillMenuPanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, mainCanvas.transform);
        ToggleSkillMenu();

        skillTrees = new Dictionary<string, SkillTree>();
        skillTrees.Add("Blacksmithing", new SkillTree(1));
        selectedTree = skillTrees["Blacksmithing"];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
            ToggleSkillMenu();
	}

    public void LayoutSkillMenu()
    {
        selectedSkill = selectedTree.skills[0];

        LayoutTreePanel();
        LayoutSelectedSkill();
    }

    public void LayoutSelectedSkill()
    {
        GameObject selectionPanel = skillMenuPanel.transform.Find(selectionPanelName).gameObject;
        GameObject selectionImage = selectionPanel.transform.Find(selectionImageName).gameObject;
        GameObject selectionText = selectionPanel.transform.Find(selectionTextName).gameObject;
        GameObject selectionButton = selectionPanel.transform.Find(selectionButtonName).gameObject;

        selectionText.GetComponent<Text>().text = selectedSkill.getDescription();
        selectionImage.GetComponent<Image>().sprite = selectedSkill.getSprite();

        ClearIngredients();

        if (selectedSkill.getId() >= 0)
        {
            LayoutIngredients();
        }

        if (selectedSkill.getUnlocked())
            selectionButton.GetComponent<Image>().sprite = (Sprite)Resources.Load(spritePath + unlockedSpriteName, typeof(Sprite));
        else
        {
            selectionButton.GetComponent<Image>().sprite = (Sprite)Resources.Load(spritePath + lockedSpriteName, typeof(Sprite));
            //selectionButton.GetComponent<Button>().onClick = UnlockSkill();
        }
    }

    public void ClearIngredients()
    {
        GameObject selectionPanel = skillMenuPanel.transform.Find(selectionPanelName).gameObject;
        GameObject ingredientsContainer = selectionPanel.transform.Find(ingredientsPanelName).gameObject;

        foreach (Transform slotTransform in ingredientsContainer.transform)
        {
            Destroy(slotTransform.gameObject);
        }
    }

    public void LayoutIngredients()
    {
        GameObject selectionPanel = skillMenuPanel.transform.Find(selectionPanelName).gameObject;
        GameObject ingredientsContainer = selectionPanel.transform.Find(ingredientsPanelName).gameObject;
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>().inventory;

        foreach (RecipeRequirement ingredient in selectedSkill.recipe.ingredients)
        {
            GameObject slot = Instantiate(
                ingredientSlotPrefab,
                Vector3.zero,
                Quaternion.identity,
                ingredientsContainer.transform
            );

            Image slotImage = slot.transform.Find(ingredientImageName).GetComponent<Image>();
            slotImage.sprite = ingredient.ingredient.sprite;

            Text slotText = slot.transform.Find(ingredientTextName).GetComponent<Text>();
            int currentCount = inventory.CheckQuantity(ingredient.ingredient.Id);
            int requiredCount = ingredient.quantity;
            slotText.text = currentCount.ToString() + "/" + requiredCount.ToString();

            if (currentCount >= requiredCount)
                slotText.color = Color.green;
            else
                slotText.color = Color.red;
        }
    }

    public void LayoutTreePanel()
    {
        List<List<SkillNode>> displayTiers = new List<List<SkillNode>>();

        foreach (SkillNode node in selectedTree.skills)
        {
            if (node.dependencies.Count == 0)
            {
                node.setTier(0);
            }
            else
            {
                int maxTier = -1; //The highest tier among any of the node's dependencies.

                foreach (SkillNode parent in node.dependencies)
                {
                    if (parent.getTier() > maxTier)
                        maxTier = parent.getTier();
                }

                node.setTier(maxTier + 1);
            }

            if (node.getTier() + 1 > displayTiers.Count)
            {
                List<SkillNode> newTier = new List<SkillNode>();
                newTier.Add(node);

                displayTiers.Add(newTier);
            }
            else
            {
                displayTiers[node.getTier()].Add(node);
            }
        }
        // At this point, the displayTiers list contains one list of nodes per "tier". A tier is comprised of nodes whose parents are contained in previous tiers.
        
        GameObject treeScrollRect = skillMenuPanel.transform.Find(treeScrollViewName).gameObject;
        GameObject treeScrollViewPort = treeScrollRect.transform.Find(treeScrollViewPortName).gameObject;
        GameObject treeScrollPanel = treeScrollViewPort.transform.Find(treePanelName).gameObject;

        //Dynamically size the treeScrollPanel to be big enough to hold the tree.
        RectTransform treePanelRectTransform = treeScrollPanel.GetComponent<RectTransform>();

        int largestTierSize = 0;

        foreach (List<SkillNode> tier in displayTiers)
        {
            if (tier.Count > largestTierSize)
                largestTierSize = tier.Count;
        }

        treePanelRectTransform.sizeDelta = new Vector2(displayTiers.Count * 75f + 50f, largestTierSize * 75f + 50f);

        for (int tierIndex = 0; tierIndex < displayTiers.Count; tierIndex++)
        {
            List<SkillNode> tier = displayTiers[tierIndex];

            for (int skillIndex = 0; skillIndex < tier.Count; skillIndex++)
            {
                SkillNode skill = tier[skillIndex];

                float xOffset = 50f + tierIndex * 75f;

                float middleNodeIndex = (tier.Count) / 2f - .5f;
                float yOffset = (middleNodeIndex - skillIndex) * 75;

                Vector3 offsetVector = new Vector3(xOffset, yOffset);

                GameObject skillNode = Instantiate(
                    skillNodePrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    treeScrollPanel.transform);

                skillNode.transform.Translate(offsetVector);

                Image nodeImage = skillNode.GetComponent<Image>();
                nodeImage.sprite = skill.getSprite();

                Button nodeButton = skillNode.GetComponent<Button>();
                nodeButton.onClick.AddListener(delegate { SelectSkill(skill); });
            }
        }
    }

    void ToggleSkillMenu()
    {
        showSkillMenu = !showSkillMenu;

        if (showSkillMenu)
            skillMenuPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
        else
            skillMenuPanel.transform.localPosition = new Vector3(-cameraWidth * 2, 0f, 0f);

        return;
    }

    public SkillNode FindSkillNodeById(int skillId)
    {
        foreach (string key in skillTrees.Keys)
        {
            SkillTree tree = skillTrees[key];

            foreach (SkillNode node in tree.skills)
            {
                if (node.getId() == skillId)
                    return node;
            }
        }

        return null;
    }

    public void SelectSkill(SkillNode skill)
    {
        selectedSkill = skill;
        LayoutSelectedSkill();
    }
}
