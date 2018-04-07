using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;
using LitJson;
using UnityEngine.EventSystems;


public class LoginForm : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;
    public Toggle rememberToggle;
    public GameObject networkManager;
	public GameObject apiManager;

	void Start() {

		if (PlayerPrefs.GetInt("rememberMe") == 1) {
			rememberToggle.isOn = true;	
			LoadCredentials();
		} else {
			rememberToggle.isOn = false;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();

			if (next != null)
			{
				InputField field = next.GetComponent<InputField> ();
				if (field != null)
				{
					field.OnPointerClick (new PointerEventData (EventSystem.current));
				}
				EventSystem.current.SetSelectedGameObject (next.gameObject, new BaseEventData (EventSystem.current));
			}

		}

		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
			OnClickLogin ();
		}
	}

    void LoadCredentials() {
        if (PlayerPrefs.HasKey("username")) {
            usernameField.text = PlayerPrefs.GetString("username");
        }

        if (PlayerPrefs.HasKey("password")) {
            passwordField.text = PlayerPrefs.GetString("password");
        }
    }

    void SaveCredentials() {
        PlayerPrefs.SetString("username", usernameField.text);
        PlayerPrefs.SetString("password", passwordField.text);
		PlayerPrefs.SetInt ("rememberMe", 1);
    }

    public void OnClickLogin() {
        
		APIManager manager = apiManager.GetComponent<APIManager>();
        StartCoroutine(manager.Login(usernameField.text, passwordField.text, () => {

			if (rememberToggle.isOn)
				SaveCredentials();

			// networkManager.SetActive(true);
			GetComponent<RectTransform>().localPosition = new Vector3(10000, 10000, 0f);
			networkManager.GetComponent<CSNetworkManager>().StartHost();
		}));
		
    }

    public void OnClickRememberToggle(bool value) {
        if (value) {
            SaveCredentials();
        } else {
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("password");
			PlayerPrefs.DeleteKey ("rememberMe");
        }
        LoadCredentials();
    }

    

	public IEnumerator GetTexture(string url, SpriteRenderer renderer) {

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
