using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtnManagerBehavior : MonoBehaviour {

    //The name of the object that contains the sript InitializeSkillTrees.cs
    private const string INITIALIZER = "Initializer";

    //Tracks whether or not SkillTreeUIBinding has updated
    public bool isBound = false;

    //Tracks whether or not UnlockRelevantSkills has updated
    public bool isBindingLibrary = false;

    //Allows references to variables from InitializeSkillTrees.cs
    InitializeSkillTrees skillTreeLibrary;
    //Contains the buttons used to toggle which skill tree is being viewed
    public List<Button> skillBtnList;

    //Contains a list of the names of the SkillTreeView objects
    public List<string> skillTreeDisplayRectNames = new List<string> { "MetalworkingSkillTreeView", "MagicSkillTreeView", "CookingSkillTreeView", "TailoringSkillTreeView", "EngineeringSkillTreeView" };

    //Holds the index of the skill tree being displayed; index references both the button and the SkillTreeCollection
    public int skillTreeSelected = 0;

	// Use this for initialization
	void Start () {
        skillTreeLibrary = GameObject.Find(INITIALIZER).GetComponent<InitializeSkillTrees>();

        int limit = gameObject.transform.childCount;
        skillBtnList = new List<Button>(gameObject.transform.GetComponents<Button>());

        foreach(Button btn in skillBtnList)
        {
            btn.onClick.AddListener(setActiveSkillTree);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setActiveSkillTree()
    {
        skillTreeSelected = skillBtnList.FindIndex(x => x == gameObject);

        //This part finds all RectTransform components, and sets the content of the Scroll View to the correct skillTreeView
        List<RectTransform> rectList = new List<RectTransform>(FindObjectsOfType<RectTransform>());
        GameObject.Find("Scroll View").GetComponent<ScrollRect>().content = rectList.Find(x => x.name == skillTreeDisplayRectNames[skillTreeSelected]);
        skillTreeLibrary.activeSkillTree = skillTreeLibrary.skillTreeCollectionsList[skillTreeSelected];

        isBound = false;
        isBindingLibrary = false;
    }
}
