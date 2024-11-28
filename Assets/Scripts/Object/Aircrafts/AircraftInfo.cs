using UnityEngine;

/// <summary>
/// 기체의 정보를 담는 요소입니다.
/// 기체에 관한 필수적인 속성을 포함하고 있습니다.
/// </summary>
public abstract class AircraftInfo : ScriptableObject, IDeepCloneable<AircraftInfo>
{
    public string Name => aircraftName;
    public float Speed => speed;
    public HitPoint HP => hitPoint ??= new HitPoint(hp);

    public GameObject TargetPrefab => targetPrefab;
    
    [SerializeField]
    private string aircraftName;
    
    [SerializeField]
    private float speed;
    
    [SerializeField]
    private float hp;

    [SerializeField] 
    private GameObject targetPrefab;

    private HitPoint hitPoint;

    public AircraftInfo(string aircraftName, float speed, float hp, GameObject targetPrefab)
    {
        this.aircraftName = aircraftName;
        this.speed = speed;
        this.hp = hp;
        this.targetPrefab = targetPrefab;
    }

    public abstract AircraftInfo DeepClone();
}