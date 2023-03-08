using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEventHandler : MonoBehaviour
{
    [SerializeField]
    private Sprite _john;

    [SerializeField]
    private Sprite _ariah;

    [SerializeField]
    private Sprite _steve;

    private GameObject _upgradePanelReference;
    private GameObject _displayCharacterReference;
    private TextMeshProUGUI _currentMaxHP;
    private TextMeshProUGUI _currentMaxSpeed;

    private readonly string _currentMaxHPPrefix = "Current HP: ";
    private readonly string _currentMaxSpeedPrefix = "Current Speed: ";

    private void Awake()
    {
        _upgradePanelReference = GameObject.Find("UpgradePanel");
        _displayCharacterReference = GameObject.Find("CharacterDisplay");
        switch (DataPreserve.characterSelectedNumber)
        {
            case 1: _displayCharacterReference.GetComponent<Image>().sprite = _john; break;
            case 2: _displayCharacterReference.GetComponent<Image>().sprite = _ariah; break;
            case 3: _displayCharacterReference.GetComponent<Image>().sprite = _steve; break;
        }
        _currentMaxHP = GameObject.Find("HealthCurrent").GetComponent<TextMeshProUGUI>();
        //Get player current max HP and set here

        _currentMaxSpeed = GameObject.Find("SpeedCurrent").GetComponent<TextMeshProUGUI>();
        //Get player current max speed and set here

        //_upgradePanelReference.SetActive(false);
    }

    public void Upgrade()
    {
        if (_upgradePanelReference == null)
        {
            _upgradePanelReference = GameObject.Find("UpgradePanel");
        }
        _upgradePanelReference.SetActive(true);

        Time.timeScale = 0;
    }

    public void UpgradeHealthClick()
    {
        Debug.Log("Health + 10");
        _upgradePanelReference.SetActive(false);
    }

    public void UpgradeSpeedClick()
    {
        Debug.Log("Speed + 0.5");
        _upgradePanelReference.SetActive(false);
    }

    public void UpgradeBackClick()
    {
        _upgradePanelReference.SetActive(false);
    }
}