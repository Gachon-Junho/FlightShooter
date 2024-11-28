using System;

public class HitPoint
{
    /// <summary>
    /// 최대 체력.
    /// </summary>
    public float MaxHP { get; }
    
    /// <summary>
    /// 현재 체력.
    /// </summary>
    public float CurrentHP { get; private set; }

    /// <summary>
    /// 체력을 완전히 소모 했을 때 실행됩니다.
    /// </summary>
    public event Action OnDead;

    public HitPoint(float maxHP, float? currentHP = null)
    {
        MaxHP = maxHP;
        CurrentHP = currentHP.HasValue ? currentHP.Value : maxHP;
    }

    public void IncreaseHP(float amount) => CurrentHP = Math.Clamp(CurrentHP + amount, 0, MaxHP);

    public void DecreaseHP(float amount)
    {
        CurrentHP = Math.Clamp(CurrentHP - amount, 0, CurrentHP);
        
        if (CurrentHP == 0)
            OnDead?.Invoke();
    }
}