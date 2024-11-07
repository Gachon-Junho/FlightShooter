using System;
using JetBrains.Annotations;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    public bool IsInUse { get; private set; }

    public bool IsInPool => pool != null;

    [CanBeNull] 
    private IGameObjectPool pool;

    private bool waitingForPrepare;

    public void Return()
    {
        if (!IsInUse)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} was already returned");

        IsInUse = false;
        
        FreeAfterUse();
        
        pool?.Return(this);
        waitingForPrepare = false;
    }
    
    protected virtual void PrepareForUse()
    {
    }
    
    protected virtual void FreeAfterUse()
    {
    }

    internal void SetPool([CanBeNull] IGameObjectPool pool)
    {
        if (IsInUse)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} is still in use");

        if (pool != null && this.pool != null)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} is already in a pool");

        this.pool = pool;
    }

    internal void Assign()
    {
        if (IsInUse)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} is already in use");

        IsInUse = true;
        waitingForPrepare = true;
    }

    protected virtual void Update()
    {
        if (waitingForPrepare)
        {
            PrepareForUse();
            waitingForPrepare = false;
        }
    }
}