using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private TMP_Dropdown aircrafts;

    [SerializeField] 
    private TMP_InputField stageNumber;

    [SerializeField] 
    private Button start;

    [SerializeField] 
    private AircraftContainer playerAircrafts;

    private int aircraftIndex;
    private int stageIndex;
    
    void Awake()
    {
        playerAircrafts.Aircrafts.ForEach(a => aircrafts.options.Add(new TMP_Dropdown.OptionData(a.Name)));
        
        aircrafts.onValueChanged.AddListener(i => aircraftIndex = i);
        stageNumber.onEndEdit.AddListener(v =>
        {
            stageIndex = int.Parse(v) - 1;
            stageNumber.text = Math.Clamp(stageIndex + 1, 1, 40).ToString();
        });
        start.onClick.AddListener(loadScene); 
    }

    private void loadScene()
    {
        PlayerPrefs.SetString(nameof(aircraftIndex), aircrafts.options[aircraftIndex].text);
        PlayerPrefs.SetInt(nameof(stageIndex), stageIndex);
        
        this.LoadSceneAsync("GameplayScene");
    }
}
