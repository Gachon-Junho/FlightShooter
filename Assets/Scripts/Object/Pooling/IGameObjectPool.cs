using System;

public interface IGameObjectPool
{
    PoolableGameObject Get(Action<PoolableGameObject> setupAction = null);
    void Return(PoolableGameObject pooledGameObject);
}