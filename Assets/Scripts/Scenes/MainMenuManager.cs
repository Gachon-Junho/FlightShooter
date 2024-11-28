using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
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

    public const string PLAYER_AIRCRAFT = "PlayerAircraft";
    public const string STAGE_INDEX = "StageIndex";
    
    void Awake()
    {
        // 플레이어가 사용가능한 기체를 드롭다운에 추가
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
        PlayerPrefs.SetString(PLAYER_AIRCRAFT, aircrafts.options[aircraftIndex].text);
        PlayerPrefs.SetInt(STAGE_INDEX, stageIndex);
        
        this.LoadSceneAsync("GameplayScene");
    }
}
