using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Login : MonoBehaviour
{


    public GameObject username;
    public GameObject password;
    private string Username;
    private string Password;
    private string[] Lines;
    private string DecryptedPassword;
    // Use this for initialization






    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
        }

        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Username != "" && Password != "")
            {
                LoginButton();
            }

        }

    }

    public void LoginButton()
    {
        bool PW = false;
        bool UN = false;

        if (Username != "")
        {
            if (System.IO.File.Exists(@"C:\Users\scdro\Desktop\UnityTestFolder\" + Username + ".txt"))
            {
                UN = true;
                Lines = System.IO.File.ReadAllLines(@"C:\Users\scdro\Desktop\UnityTestFolder\" + Username + ".txt");
            }

            else
            {
                Debug.LogWarning("Username Is Taken");
            }

        }
        else
        {
            Debug.LogWarning("The username Field is Empty");
        }

        if (Password != "")
        {

            if (System.IO.File.Exists(@"C:\Users\scdro\Desktop\UnityTestFolder\" + Username + ".txt"))
            {
                int i = 1;
                foreach (char c in Lines[2]) //this is where you can change lines of where the file is written
                {

                    i++;
                    char Decrypted = (char)(c / i);
                    DecryptedPassword += Decrypted.ToString();

                }
                if (Password == DecryptedPassword)
                {
                    PW = true;
                }
                else
                {
                    Debug.LogWarning("Password Incorrect");
                }
            }

            else
            {
                Debug.LogWarning("Password Empty");
            }
            if(UN == true && PW == true)
            {
                username.GetComponent<InputField>().text = "";
                password.GetComponent<InputField>().text = "";

                print("Login Succeeded");
                Application.LoadLevel("MainScene"); //this is where you can change 
                                                    //which scene starts when login is clicked
                                                    //Application.LoadLevel("StartupScene");

            }
        }
    }
}
