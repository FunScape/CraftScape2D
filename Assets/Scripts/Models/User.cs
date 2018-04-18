using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class User : ScriptableObject {

    public int id { get; private set; }
    public string username { get; private set; }
    public List<string> characterUrls { get; private set; }

    void Init(int id, string username, List<string> urls)
    {
        this.id = id;
        this.username = username;
        this.characterUrls = urls;
    }

    void Init(int id, string username, string[] urls)
    {
        this.id = id;
        this.username = username;
        this.characterUrls = new List<string>();
        foreach (string url in urls)
            this.characterUrls.Add(url);
    }

    public static User Parse(JsonData data)
    {
        int id = (int)data["id"];
        string username = data["username"].ToString();
        List<string> characterUrls = new List<string>();
        foreach (JsonData url in data["characters"]) {
            characterUrls.Add(url.ToString());
        }

        User user = ScriptableObject.CreateInstance("User") as User;
		user.Init(id, username, characterUrls);
		return user;
    }

}