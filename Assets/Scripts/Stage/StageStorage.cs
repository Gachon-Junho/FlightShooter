using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageStorage", menuName = "Stage/Stage Storage", order = 0)]
public class StageStorage : ScriptableObject
{
    public StageData[] StageData => stageData;

    [SerializeField]
    private StageData[] stageData;
}

[Serializable]
public class StageData
{
    public float HPWeight;
    public float DamageWeight;
    public AircraftSetting[] AircraftSetting;
}

[Serializable]
 public class AircraftSetting
 {
     public EnemyAircraft Aircraft;
     public int Amount;
 }