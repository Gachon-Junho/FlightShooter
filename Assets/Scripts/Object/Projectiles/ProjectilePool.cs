/// <summary>
/// 발사체를 풀링하는 오브젝트 풀입니다.
/// </summary>
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
