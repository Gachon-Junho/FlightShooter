using UnityEngine;

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