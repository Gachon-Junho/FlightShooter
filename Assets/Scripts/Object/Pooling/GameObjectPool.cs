using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class GameObjectPool<T> : MonoBehaviour
    where T : MonoBehaviour, IPoolableGameObject
{
    public IObjectPool<GameObject> Pool { get; private set; }
    
    public const int DEFAULT_CAPACITY = 20;
    public const int MAX_SIZE = 100;

    [SerializeField] 
    public GameObject Prefab;

    protected void Awake()
    {
        Pool = new ObjectPool<GameObject>(createPooledItem, onGetFromPool, onReleaseToPool, onDestroyPoolObject, true, DEFAULT_CAPACITY, MAX_SIZE);
    }

    protected virtual GameObject createPooledItem()
    {
        var go = Instantiate(Prefab, gameObject.transform, true);
        go.GetComponent<T>().Pool = Pool;

        return go;
    }
    
    // Todo: 재사용하기 전에 초기상태처럼 되돌려야 할 수 있음.
    private void onGetFromPool(GameObject poolGO) => poolGO.SetActive(true);
    
    private void onReleaseToPool(GameObject poolGO) => poolGO.SetActive(false);
    
    private void onDestroyPoolObject(GameObject poolGO) => Destroy(poolGO);
}