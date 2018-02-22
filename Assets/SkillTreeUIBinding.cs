using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUIBinding : MonoBehaviour {

    public bool isBound = false;

    public List<GameObject> UITreeNodes;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Makes a list of UI elements that can be accessed with the same index as the SkillNodes that they are associated with
		if(!isBound)
        {
            UITreeNodes = new List<GameObject> { };
            int limit = gameObject.transform.childCount;

            for(int i = 0; i < limit; i++)
            {
                UITreeNodes.Add(gameObject.transform.GetChild(i).gameObject);
            }

            isBound = true;
        }
	}
}
