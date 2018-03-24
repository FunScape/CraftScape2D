using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class DatabaseAPI : MonoBehaviour {
	
	void Start() {
		string axeUrl = "https://opengameart.org/sites/default/files/axe2.png";
		StartCoroutine(GetTexture(axeUrl, GetComponent<SpriteRenderer>()));
	}

	public IEnumerator GetTexture(string url, SpriteRenderer renderer) {
		Debug.Log ("Starting HTTP request");

		Texture2D texture;
		texture = new Texture2D (1, 1);

		using (WWW www = new WWW(url))
		{
			yield return www;
			www.LoadImageIntoTexture (texture);
			Sprite sprite = Sprite.Create (texture, 
				new Rect(0, 0, texture.width, texture.height), 
				Vector2.one
			);
			renderer.sprite = sprite;
		}

	}
	
}
