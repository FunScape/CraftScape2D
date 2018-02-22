using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSkillTrees : MonoBehaviour {

    //Need to generalize this once the principle behind it is confirmed to work
    public SkillTreeCollection metalworkingSkillTree;

	// Use this for initialization
	void Start () {
        metalworkingSkillTree = new SkillTreeCollection();
        metalworkingSkillTree.subtrees.Add(new SkillTree(2));
        
        Debug.Log("Start finished", gameObject);
	}

    // Update is called once per frame
    void Update()
    {

    }
}
