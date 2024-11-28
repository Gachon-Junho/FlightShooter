using System;

public interface IGameObjectPool
{
    /// <summary>
    /// 이 풀에서 오브젝트를 가져옵니다.
    /// </summary>
    /// <param name="setupAction">가져온 직후 이 오브젝트에서 수행할 선택적인 작업입니다. 일반적으로 오브젝트를 사용 가능한 상태로 준비하는데 사용합니다.</param>
    /// <returns>오브젝트.</returns>
    PoolableGameObject Get(Action<PoolableGameObject> setupAction = null);
    
    /// <summary>
    /// 사용 후 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="pooledGameObject">반환할 오브젝트. 이 풀안에 속해야 함.</param>
    void Return(PoolableGameObject pooledGameObject);
}