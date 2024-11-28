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

/// <summary>
/// 한 스테이지의 정보를 담는 요소입니다.
/// 적기에 대해 적용할 값을 포함하고 있습니다.
/// </summary>
[Serializable]
public class StageData : IDeepCloneable<StageData>
{
    /// <summary>
    /// 체력 가중치.
    /// </summary>
    public float HPWeight;
    
    /// <summary>
    /// 대미지 가중치.
    /// </summary>
    public float DamageWeight;
    
    /// <summary>
    /// 스테이지에 소환할 기체 설정.
    /// </summary>
    public AircraftSetting[] AircraftSetting;

    /// <summary>
    /// 최소 기제 생성 간격.
    /// </summary>
    public float MinSpawnInterval;
    
    /// <summary>
    /// 최대 기체 생성 간격.
    /// </summary>
    public float MaxSpawnInterval;

    /// <summary>
    /// 스테이지에 생성되는 총 기체 개수.
    /// </summary>
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

    /// <summary>
    /// 제거한 기체의 개수.
    /// </summary>
    public int DownedAircraftCount
    {
        get => downedAircraftCount;
        set
        {
            if (IsCleared)
                return;
            
            downedAircraftCount = value;
            
            if (IsCleared)
                OnClear?.Invoke(this);
        }
    }

    private int downedAircraftCount;

    /// <summary>
    /// 스테이지 클리어 여부.
    /// </summary>
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