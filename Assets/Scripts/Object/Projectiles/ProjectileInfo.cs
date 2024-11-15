using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Aircraft/Projectile Info")]
public class ProjectileInfo : ScriptableObject
{
    public float Speed => speed;
    public float Damage => damage;

    public GameObject TargetPrefab => targetPrefab;
    
    [SerializeField] 
    private float speed;
    
    [SerializeField] 
    private int damage;

    [SerializeField] 
    private GameObject targetPrefab;

    public ProjectileInfo(float speed, int damage, GameObject targetPrefab)
    {
        this.speed = speed;
        this.damage = damage;
        this.targetPrefab = targetPrefab;
    }
}
