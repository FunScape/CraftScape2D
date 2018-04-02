using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationBehaviour : MonoBehaviour
{

    Animator leftArmAnimator;
    Animator rightArmAnimator;
    Animator legAnimator;
    SpriteRenderer leftArmRenderer;
    SpriteRenderer rightArmRenderer;

    Color clearColor
    {
        get
        {
            Color color = Color.white;
            color.a = 0; // change alpha value to zero
            return color;
        }
    }


    void Start()
    {
        Animator[] playerAnimators = gameObject.GetComponentsInChildren<Animator>();
        SpriteRenderer[] playerSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer item in playerSprites)
        {

            if (item.gameObject.name == "LeftArm")
            {
                leftArmRenderer = item;
            }
            else if (item.gameObject.name == "RightArm")
            {
                rightArmRenderer = item;
            }

        }
        foreach (Animator item in playerAnimators)
        {
            if (item.gameObject.name == "Legs")
            {
                legAnimator = item;
            }
            else if (item.gameObject.name == "LeftArm")
            {
                leftArmAnimator = item;
            }
            else if (item.gameObject.name == "RightArm")
            {
                rightArmAnimator = item;
            }

        }

    }

    void Update()
    {
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        if (moveUp && !moveRight && !moveLeft)
        {
            leftArmAnimator.SetTrigger("WalkUp");
            rightArmAnimator.SetTrigger("WalkUp");
            legAnimator.SetTrigger("LegsWalkUp");
            //anim.SetTrigger("WalkUp");
        }
        else if (moveDown && !moveRight && !moveLeft)
        {
            leftArmAnimator.SetTrigger("WalkDown");
            rightArmAnimator.SetTrigger("WalkDown");
            legAnimator.SetTrigger("LegsWalkDown");

            //anim.SetTrigger("WalkDown");
        }
        else if (moveRight)
        {

            GameObject leftArm = leftArmAnimator.gameObject;
            SpriteRenderer leftSpriteRenderer = leftArm.GetComponent<SpriteRenderer>();

            leftSpriteRenderer.color = clearColor;

            rightArmAnimator.SetTrigger("WalkSide");
            legAnimator.SetTrigger("LegsWalkSide");
            //anim.SetTrigger("WalkSide");
            GetComponent<NetworkPlayerAnimationBehaviour>().CmdOnFlipX(true);
        }
        else if (moveLeft)
        {
            leftArmAnimator.SetTrigger("WalkSide");
            rightArmAnimator.SetTrigger("Dissapear");
            legAnimator.SetTrigger("LegsWalkSide");
            //anim.SetTrigger("WalkSide");
            GetComponent<NetworkPlayerAnimationBehaviour>().CmdOnFlipX(false);
        }
        else
        {
            leftArmAnimator.SetTrigger("IdleDown");
            rightArmAnimator.SetTrigger("IdleDown");
            legAnimator.SetTrigger("IdleDown");
            //anim.SetTrigger("Idle");
        }
    }


}
