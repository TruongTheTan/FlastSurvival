using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    private GameObject mainMenuPanel;
    private GameObject confirmationPanel;

    private void Awake()
    {
        mainMenuPanel = GameObject.Find("MainMenuPanel");
        confirmationPanel = GameObject.Find("ConfirmationPanel");
        confirmationPanel.SetActive(false);
    }

    public void QuitButtonClick()
    {
        if (mainMenuPanel != null && confirmationPanel != null)
        {
            mainMenuPanel.SetActive(false);
            confirmationPanel.SetActive(true);
        }
    }
}