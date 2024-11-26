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
public class StageData : IDeepCloneable<StageData>
{
    public float HPWeight;
    public float DamageWeight;
    public AircraftSetting[] AircraftSetting;

    public float MinSpawnInterval;
    public float MaxSpawnInterval;

    public readonly int TotalAircraftCount;

    public StageData(float hpWeight, float damageWeight, AircraftSetting[] aircraftSetting, float minSpawnInterval, float maxSpawnInterval)
    {
        HPWeight = hpWeight;
        DamageWeight = damageWeight;
        AircraftSetting = aircraftSetting;
        MinSpawnInterval = minSpawnInterval;
        MaxSpawnInterval = maxSpawnInterval;
        
        TotalAircraftCount  = AircraftSetting.Sum(a => a.Amount);
    }

    public int DownedAircraftCount
    {
        get => downedAircraftCount;
        set
        {
            downedAircraftCount = value;
            
            if (IsCleared)
                OnClear?.Invoke(this);
        }
    }

    private int downedAircraftCount;

    public bool IsCleared => DownedAircraftCount >= TotalAircraftCount;

    public Action<StageData> OnClear;
    
    public StageData DeepClone()
    {
        return new StageData(HPWeight, DamageWeight, AircraftSetting.Select(a => a.DeepClone()).ToArray(), MinSpawnInterval, MaxSpawnInterval);
    }
}

[Serializable]
public class AircraftSetting : IDeepCloneable<AircraftSetting>
 {
     public AttackableAircraftInfo AircraftInfo;
     public int Amount;


     public AircraftSetting DeepClone()
     {
         return new AircraftSetting
         {
             AircraftInfo = ((IDeepCloneable<AttackableAircraftInfo>)AircraftInfo).DeepClone(),
             Amount = Amount
         };
     }
 }