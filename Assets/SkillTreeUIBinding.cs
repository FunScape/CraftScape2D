using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUIBinding : MonoBehaviour {

    public SkillBtnManagerBehavior btnManage;

    public List<GameObject> UITreeNodes;

    // Use this for initialization
    void Start () {
        btnManage = GameObject.Find("SkillBtnManager").GetComponent<SkillBtnManagerBehavior>();
		
	}
	
	// Update is called once per frame
	void Update () {

        //Makes a list of UI elements that can be accessed with the same index as the SkillNodes that they are associated with
		if(!btnManage.isBound)
        {
            UITreeNodes = new List<GameObject> { };
            int limit = gameObject.transform.childCount;

            for(int i = 0; i < limit; i++)
            {
                UITreeNodes.Add(gameObject.transform.GetChild(i).gameObject);
            }

            btnManage.isBound = true;
        }
	}
}
