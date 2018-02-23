using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSkillTrees : MonoBehaviour {

    //The number of different varieties of skills in the game
    private const int NUM_SKILL_TYPES = 5;

    //Contains all skillTreCollections within the game
    public List<SkillTreeCollection> skillTreeCollectionsList;

    //References the skillTree currently rendered
    public SkillTreeCollection activeSkillTree;

	// Use this for initialization
	void Start () {

        for(int i = 0; i < NUM_SKILL_TYPES; i++)
        {
            skillTreeCollectionsList.Add(new SkillTreeCollection());
            skillTreeCollectionsList[i].subtrees.Add(new SkillTree(2));
        }

        activeSkillTree = skillTreeCollectionsList[0];
        
        Debug.Log("Start finished", gameObject);
	}

    // Update is called once per frame
    void Update()
    {

    }
}
