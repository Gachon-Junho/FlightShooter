public class ProjectilePool : GameObjectPool<Projectile>, ISingleton<ProjectilePool>
{
    public static ProjectilePool Current
    {
        get => current;
        set
        {
            if (current == null)
                current = value;
        }
    }

    private static ProjectilePool current;

    protected override void Awake()
    {
        base.Awake();

        current = this;
    }
}
