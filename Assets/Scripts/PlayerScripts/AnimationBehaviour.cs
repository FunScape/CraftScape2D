using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBehaviour : MonoBehaviour {
    

    Color clearColor { get { Color color = Color.white; color.a = 0; return color; } }


	void Start () {

    }

    void Update () {
		bool moveRight = Input.GetKey(KeyCode.D);
		bool moveLeft = Input.GetKey(KeyCode.A);
		bool moveUp = Input.GetKey(KeyCode.W);
		bool moveDown = Input.GetKey(KeyCode.S);

		if (moveUp && !moveRight && !moveLeft) 
        {
            
        } 
        else if (moveDown && !moveRight && !moveLeft) 
        {
            
		} 
        else if (moveRight) {

		} 
        else if (moveLeft) 
        {

        } 
        else
        {

		}
	}


}
