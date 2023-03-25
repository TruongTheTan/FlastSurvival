using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseCharacterEventHandler : MonoBehaviour
{
    private Color characterPanelColor;
    private Color characterStatPanelColor;


    private Image characterPanelImage;
    private Image characterStatPanelImage;



    private void Start()
    {
        // John is selected by default
        ChangeCharacterPanelColor("JohnPanel", "JohnStatPanel");
    }


    public void ChooseJohnButtonClick()
    {
        DataPreserve.characterSelectedNumber = 1;

        string[] charaterPanelNames = new string[] { "AriahPanel", "StevePanel" };
        string[] charaterStatPanelNames = new string[] { "AriahStatPanel", "SteveStatPanel" };

        ChangeCharacterPanelColor("JohnPanel", "JohnStatPanel");
        ResetOtherPanelColorToDefault(charaterPanelNames, charaterStatPanelNames);

    }



    public void ChooseAriahButtonClick()
    {
        DataPreserve.characterSelectedNumber = 2;

        string[] charaterPanelNames = new string[] { "StevePanel", "JohnPanel" };
        string[] charaterStatPanelNames = new string[] { "JohnStatPanel", "SteveStatPanel" };

        ChangeCharacterPanelColor("AriahPanel", "AriahStatPanel");
        ResetOtherPanelColorToDefault(charaterPanelNames, charaterStatPanelNames);
    }



    public void ChooseSteveButtonClick()
    {
        DataPreserve.characterSelectedNumber = 3;

        string[] charaterPanelNames = new string[] { "AriahPanel", "JohnPanel" };
        string[] charaterStatPanelNames = new string[] { "JohnStatPanel", "AriahStatPanel" };

        ChangeCharacterPanelColor("StevePanel", "SteveStatPanel");
        ResetOtherPanelColorToDefault(charaterPanelNames, charaterStatPanelNames);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("ScenePlayGame");
    }




    /// <summary>
    /// A sub-function, use to change character panel color when click choose button
    /// </summary>

    private void ChangeCharacterPanelColor(string characterPanelName, string characterStatPanelName)
    {
        // Find Panel's Image component
        characterPanelImage = GameObject.Find(characterPanelName).GetComponent<Image>();
        characterStatPanelImage = GameObject.Find(characterStatPanelName).GetComponent<Image>();


        // Convert Hex code to Color
        ColorUtility.TryParseHtmlString("#CFFF00", out characterPanelColor);
        ColorUtility.TryParseHtmlString("#0C06FF", out characterStatPanelColor);


        // Change color
        characterPanelImage.color = characterPanelColor;
        characterStatPanelImage.color = characterStatPanelColor;
    }



    /// <summary>
    /// A sub-function, reset other panel to their default color
    /// </summary>
    private void ResetOtherPanelColorToDefault(string[] characterPanelName, string[] characterStatPanelName)
    {
        foreach (string panelName in characterPanelName)
        {
            characterPanelImage = GameObject.Find(panelName).GetComponent<Image>();
            characterPanelImage.color = new Color(1, 1, 1, 0.3921569f);
        }

        foreach (string statPanelName in characterStatPanelName)
        {
            characterStatPanelImage = GameObject.Find(statPanelName).GetComponent<Image>();
            characterStatPanelImage.color = new Color(0.04579253f, 0.02264148f, 1, 0.3921569f);
        }


    }
}
