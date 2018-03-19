using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		// DrawMousePositionGUI();
	}

	void DrawMousePositionGUI()
	{
		Vector3 position = new Vector3();
		Camera  camera = Camera.main;
		Vector2 mousePos = new Vector2();

		// Get the mouse position from Event.current.
		// Note that the y position from Event.current is inverted.
		mousePos.x = Event.current.mousePosition.x;
		mousePos.y = camera.pixelHeight - Event.current.mousePosition.y;

		position = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));

		GUIStyle style = new GUIStyle();
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(1, 1, Color.black);
		texture.Apply();
		style.normal.background = texture;

		GUILayout.BeginArea(new Rect(10, 150, 230, 317-265), "", style);
		GUILayout.Label("Mouse position: " + mousePos);
		GUILayout.Label("World position: " + position.ToString("F1"));
		GUILayout.EndArea();
	}

}
