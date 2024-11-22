using System.Linq;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public StageData Stage { get; private set; }
    
    public PlayerAircraft Player { get; private set; }
    
    [SerializeField]
    private AircraftContainer aircrafts;

    [SerializeField]
    private StageStorage stages;

    protected override void Awake()
    {
        base.Awake();

        var aircraftName = PlayerPrefs.GetString(MainMenuManager.PLAYER_AIRCRAFT);
        var stageIndex = PlayerPrefs.GetInt(MainMenuManager.STAGE_INDEX);
        var aircraft = aircrafts.Aircrafts.FirstOrDefault(a => a.Name == aircraftName)!;
        
        Stage = stages.StageData[stageIndex].DeepClone();
        Stage.OnClear += _ => EndGame(GameResultState.Success);

        Player = Instantiate(aircraft.TargetPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerAircraft>();
        Player.Initialize(aircraft);
    }

    public void EndGame(GameResultState result)
    {
        Debug.Log(result);
    }

    public enum GameResultState
    {
        Success,
        Fail
    }
}
