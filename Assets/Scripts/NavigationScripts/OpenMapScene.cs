using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapScene : MonoBehaviour {


    public void openMapScene(string scenename)
    {
            Application.LoadLevel(scenename);
    }
}
