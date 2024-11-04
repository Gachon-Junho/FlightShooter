using System;
using UnityEngine;

public class HitPoint
{
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }

    public event Action OnDead;

    public HitPoint(int maxHP, int? currentHP = null)
    {
        MaxHP = maxHP;
        CurrentHP = currentHP.HasValue ? currentHP.Value : maxHP;
    }

    public void IncreaseHP(int amount) => CurrentHP = Math.Clamp(CurrentHP + amount, 0, MaxHP);

    public void DecreaseHP(int amount)
    {
        CurrentHP = Math.Clamp(CurrentHP - amount, 0, CurrentHP);
        
        if (CurrentHP == 0)
            OnDead?.Invoke();
    }
}