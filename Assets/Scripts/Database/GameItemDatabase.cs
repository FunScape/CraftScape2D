using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System.Linq;

public class GameItemDatabase : System.Object {

    private GameItemDatabase() {
        APIManager manager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();
        manager.GetStaticGameItems((items) => {
            gameItems = items;
        });
    }

    private static GameItemDatabase _instance;

    public static GameItemDatabase instance {
        get {
            if (GameItemDatabase._instance == null) {
                _instance = new GameItemDatabase();
            }
            return _instance;
        }
    }

    public List<StaticGameItem> gameItems = new List<StaticGameItem>();

    public StaticGameItem GetItem(int id)
    {
        foreach (StaticGameItem item in gameItems)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }

    public StaticGameItem GetItem(string name)
    {
        foreach (StaticGameItem item in gameItems)
        {
            if (item.Name == name)
                return item;
        }
        return null;
    }

}