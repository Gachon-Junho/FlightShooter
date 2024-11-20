using System;
using System.Linq;
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

    public bool IsCleared => AircraftSetting.Where(a => a.Amount != 0) == null;
}

[Serializable]
 public class AircraftSetting
 {
     public AttackableAircraftInfo AircraftInfo;
     public int Amount;
 }