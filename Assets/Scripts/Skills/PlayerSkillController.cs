using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour {

    public GameObject skillMenuPanelPrefab;

    public GameObject skillMenuPanel;

    public GameObject skillNodePefab;

    public Dictionary<string, SkillTree> skillTrees;

    SkillTree selectedTree;

    SkillNode selectedSkill;

    bool showSkillMenu;

    float cameraHeight;
    float cameraWidth;

    protected const string treePanelName = "TreePanel";
    protected const string selectionPanelName = "SelectionPanel";
    protected const string selectionImageName = "SelectionImage";
    protected const string selectionTextName = "SelectionText";
    protected const string selectionTextBackgroundName = "SelectionTextBackground";
    protected const string ingredientsPanelName = "IngredientsPanel";
    protected const string selectionButtonName = "SelectionButton";

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

        //Do more.
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
                int maxTier = -1;

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

        for (int tierIndex = 0; tierIndex < displayTiers.Count; tierIndex++)
        {
            List<SkillNode> tier = displayTiers[tierIndex];

            for (int skillIndex = 0; skillIndex < tier.Count; skillIndex++)
            {
                SkillNode skill = tier[skillIndex];

                GameObject treeScrollPanel = skillMenuPanel.transform.Find(treePanelName).gameObject;

                float xOffset = 50f + tierIndex * 50f;

                float middleNodeIndex = (float)(tier.Count) / 2f - .5f;
                float yOffset = (middleNodeIndex - (float)skillIndex) * 75;

                Vector3 offsetVector = new Vector3(xOffset, yOffset);

                GameObject skillNode = Instantiate(
                    skillNodePefab,
                    Vector3.zero,
                    Quaternion.identity,
                    treeScrollPanel.transform);

                skillNode.transform.Translate(offsetVector);

                Image nodeImage = skillNode.GetComponent<Image>();
                nodeImage.sprite = skill.getSprite();
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
}
