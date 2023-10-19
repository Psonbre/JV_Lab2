using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text placeHolderText;

    void Start()
    { 
    
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (playerNameText.text.Length < 3)
            {
                inputField.Select();
                inputField.text = "";
                placeHolderText.text = "Le nom doit avoir au moins 3 caractères";
            }
            else
            {
                GameManager.Instance.SetPlayerName(playerNameText.text);
                GameManager.Instance.StartNextlevel(0.2f);
            }
        }
    }
}