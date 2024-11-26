using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : Singleton<GameplayManager>
{
    public StageData Stage { get; private set; }
    
    public PlayerAircraft Player { get; private set; }
    
    [SerializeField]
    private AircraftContainer playerAircrafts;

    [SerializeField]
    private StageStorage stages;

    [SerializeField] private GameObject gameResultOverlay;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button nextStage;
    [SerializeField] private TMP_Dropdown aircrafts;
    [SerializeField] private TMP_Text result;
    
    private int aircraftIndex;
    private int stageIndex;

    protected override void Awake()
    {
        base.Awake();

        var aircraftName = PlayerPrefs.GetString(MainMenuManager.PLAYER_AIRCRAFT);
        stageIndex = PlayerPrefs.GetInt(MainMenuManager.STAGE_INDEX);
        
        var aircraft = playerAircrafts.Aircrafts.FirstOrDefault(a => a.Name == aircraftName)!;
        
        Stage = stages.StageData[stageIndex].DeepClone();
        Stage.OnClear += _ => EndGame(GameResultState.Success);

        initializeUI();
        
        Player = Instantiate(aircraft.TargetPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerAircraft>();
        Player.Initialize(aircraft);
    }

    private void initializeUI()
    {
        playerAircrafts.Aircrafts.ForEach(a => aircrafts.options.Add(new TMP_Dropdown.OptionData(a.Name)));
                
        aircrafts.onValueChanged.AddListener(i => aircraftIndex = i);
        mainMenu.onClick.AddListener(() => this.LoadSceneAsync("MainMenuScene"));
        nextStage.onClick.AddListener(loadScene);

        if (stageIndex + 1 >= stages.StageData.Length)
            nextStage.enabled = false;
        else
            stageIndex++;
    }

    public void EndGame(GameResultState result)
    {
        this.result.text = result == GameResultState.Success ? "Clear!" : "Fail";
        
        gameResultOverlay.SetActive(true);
        
        Debug.Log(result);
    }
    
    private void loadScene()
    {
        PlayerPrefs.SetString(MainMenuManager.PLAYER_AIRCRAFT, aircrafts.options[aircraftIndex].text);
        PlayerPrefs.SetInt(MainMenuManager.STAGE_INDEX, stageIndex);
        
        this.LoadSceneAsync("GameplayScene");
    }

    public enum GameResultState
    {
        Success,
        Fail
    }
}
