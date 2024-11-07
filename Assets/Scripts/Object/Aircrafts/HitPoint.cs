using System;
using UnityEngine;

public class HitPoint
{
    public float MaxHP { get; }
    public float CurrentHP { get; private set; }

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