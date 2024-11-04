using UnityEngine;

namespace Object.Projectiles
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "Aircraft/Projectile Info")]
    public class ProjectileInfo : ScriptableObject
    {
        public float Speed => speed;
        public int Damage => damage;
        
        [SerializeField] 
        private float speed;
        
        [SerializeField] 
        private int damage;
    }
}