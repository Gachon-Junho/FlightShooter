using UnityEngine;
using UnityEngine.Pool;

public interface IPoolableGameObject
{
    IObjectPool<GameObject> Pool { get; set; }

    void Release();
}