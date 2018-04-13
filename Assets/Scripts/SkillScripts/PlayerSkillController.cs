using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour {

    public GameObject skillMenuPanelPrefab;

    public GameObject skillMenuPanel;

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

        showSkillMenu = false;

        GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");

        skillMenuPanel = Instantiate(skillMenuPanelPrefab, new Vector3(cameraWidth * 2, 0f, 0f), Quaternion.identity, mainCanvas.transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
            ToggleSkillMenu();

	}

    void ToggleSkillMenu()
    {
        showSkillMenu = !showSkillMenu;

        if (showSkillMenu)
            skillMenuPanel.transform.localPosition = new Vector3(-cameraWidth / 3, 0f, 0f);
        else
            skillMenuPanel.transform.localPosition = new Vector3(-cameraWidth * 2, 0f, 0f);

        return;
    }
}
