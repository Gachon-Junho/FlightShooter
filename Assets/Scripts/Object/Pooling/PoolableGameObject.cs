using System;
using JetBrains.Annotations;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    /// <summary>
    /// 풀링된 이 오브젝트가 현재 사용 중인지의 여부.
    /// </summary>
    public bool IsInUse { get; private set; }

    /// <summary>
    /// 이 오브젝트가 현재 풀에 의해 관리되는지의 여부.
    /// </summary>
    public bool IsInPool => pool != null;

    [CanBeNull] 
    private IGameObjectPool pool;

    /// <summary>
    /// 준비 호출이 예약된 호출로 실행될 수 있도록 보장하기 위해 드로어블을 존재하는 상태로 유지하는 플래그.
    /// </summary>
    private bool waitingForPrepare;

    /// <summary>
    /// 이 오브젝트를 수동으로 풀로 반환합니다.
    /// </summary>
    public void Return()
    {
        if (!IsInUse)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} was already returned");

        IsInUse = false;
        
        FreeAfterUse();
        
        // 풀과 연관되지 않았거나 그 밖에 경우엔 의도적으로 예외를 호출하지 않음.
        // 특별한 처리 없이 풀링된 시나리오 밖에서 풀링된 오브젝트 사용을 지원함.
        pool?.Return(this);
        waitingForPrepare = false;
    }
    
    /// <summary>
    /// 이 오브젝트를 새로 사용할 때 초기화를 수행합니다.
    /// 이는 첫번 째 업데이트 프레임에 예약되어 있으며, 여기에 도달하지 않으면 실행되지 않습니다.
    /// </summary>
    protected virtual void PrepareForUse()
    {
    }
    
    /// <summary>
    /// 이 오브젝트를 반환하기 전에 필요한 정리 작업을 수행합니다.
    /// <see cref="PrepareForUse"/>가 호출되었는지 여부에 관계없이 호출됩니다.
    /// </summary>
    protected virtual void FreeAfterUse()
    {
    }

    /// <summary>
    /// 이 오브젝트를 풀과 연결된 풀을 설정합니다.
    /// </summary>
    /// <param name="pool">대상 풀 또는 모든 풀에서 연결을 해제하려면 null을 지정합니다.</param>
    /// <exception cref="InvalidOperationException">이 오브젝트가 사용 중이거나 이미 다른 풀에 있는 경우 발생합니다.</exception>
    internal void SetPool([CanBeNull] IGameObjectPool pool)
    {
        if (IsInUse)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} is still in use");

        if (pool != null && this.pool != null)
            throw new InvalidOperationException($"This {nameof(PoolableGameObject)} is already in a pool");

        this.pool = pool;
    }

    /// <summary>
    /// 이 오브젝트를 사용하기 위해 할당합니다.
    /// </summary>
    /// <exception cref="InvalidOperationException">이 오브젝트가 아직 사용 중이면 발생합니다.</exception>
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