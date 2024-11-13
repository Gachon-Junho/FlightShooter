using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayScene : MonoBehaviour
{
    [SerializeField]
    private AircraftContainer aircrafts;

    [SerializeField]
    private StageStorage stages;
    
    private void Awake()
    {
        var aircraftName = PlayerPrefs.GetString(MainMenuScene.PLAYER_AIRCRAFT);
        var stageIndex = PlayerPrefs.GetInt(MainMenuScene.STAGE_INDEX);

        var aircraft = aircrafts.Aircrafts.FirstOrDefault(a => a.Name == aircraftName)!;
        var stage = stages.StageData[stageIndex];

        // var cameraSize = new Vector3(Camera.main!.aspect * Camera.main!.orthographicSize, Camera.main!.orthographicSize);
        // var pos = Camera.main!.ScreenToViewportPoint(new Vector3(cameraSize.x * 0.5f, cameraSize.y * -0.8f, 0)) + new Vector3(0, 0, 10);

        var obj = Instantiate(aircraft.TargetPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerAircraft>();
        obj.Initialize(aircraft);
    }
}
