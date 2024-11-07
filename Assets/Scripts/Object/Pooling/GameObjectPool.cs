using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool<T> : MonoBehaviour, IGameObjectPool where T : PoolableGameObject, new()
{
    [SerializeField]
    private int initialSize;
    
    [SerializeField]
    [Tooltip("0 혹은 -1은 제한이 제한이 없음을 나타냅니다.")]
    private int maximumSize;

    [SerializeField]
    private GameObject[] poolTarget;

    private List<T> pool = new List<T>();
    private int currentPoolSize;

    public int CurrentPoolSize
    {
        get => currentPoolSize;
        private set
        {
            currentPoolSize = value;
        }
    }

    private int countInUse;

    public int CountInUse
    {
        get => countInUse;
        private set
        {
            countInUse = value;
        }
    }

    private int countExcessConstructed;

    public int CountExcessConstructed
    {
        get => countExcessConstructed;
        private set
        {
            countExcessConstructed = value;
        }
    }

    public int CountAvailable => pool.Count;

    private void Awake()
    {
        if (initialSize > maximumSize)
            throw new ArgumentOutOfRangeException(nameof(initialSize), "Initial size must be less than or equal to maximum size.");
        
        for (int i = 0; i < initialSize; i++)
            pool.Add(create(i / (initialSize / poolTarget.Length)));

        CurrentPoolSize = initialSize;
    }

    private T create(int targetIndex)
    {
        var o = CreateNewGameObject(targetIndex);
        
        o.SetPool(this);

        return o;
    }

    protected virtual T CreateNewGameObject(int targetIndex = 0)
    {
        var o = Instantiate(poolTarget[targetIndex], transform, true);
        o.SetActive(false);
        
        return o.GetComponent<T>();
    }

    public void Return(PoolableGameObject pooledGameObject)
    {
        if (pooledGameObject is not T typedGameObject)
            throw new ArgumentException("Invalid type", nameof(pooledGameObject));
        
        pooledGameObject.gameObject.SetActive(false);

        if (pooledGameObject.IsInUse)
        {
            pooledGameObject.Return();
            
            return;
        }

        if (CountAvailable >= maximumSize)
        {
            // 오브젝트를 반환할 수 없는 경우, 이렇게 하여 처리할 수 있도록 함.
            pooledGameObject.SetPool(null);

            // 이후 파괴 시도.
            Destroy(pooledGameObject.gameObject);
        }
        else
        {
            pool.Add(typedGameObject);
        }

        CountInUse--;
    }
    
    PoolableGameObject IGameObjectPool.Get(Action<PoolableGameObject> setupAction) => Get(setupAction);

    public T Get(Action<T> setupAction = null)
    {
        T obj;
        
        if ((obj = pool.LastOrDefault()) == null)
        {
            obj = create(0);

            if (maximumSize <= 0 || currentPoolSize < maximumSize)
            {
                CurrentPoolSize++;
                Debug.Assert(maximumSize <= 0 || CurrentPoolSize <= maximumSize);
            }
            else
                CountExcessConstructed++;

            obj.gameObject.SetActive(true);
        }
        else
        {
            pool.Remove(obj);
        }

        CountInUse++;

        obj.Assign();

        setupAction?.Invoke(obj);

        return obj;
    }

    public T Get<U>(Action<T> setupAction = null, int createIndexIfNotExists = 0)
        where U : T
    {
        var obj = pool.LastOrDefault(o => o.GetType() == typeof(U));

        if (obj == null)
        {
            obj = create(createIndexIfNotExists);

            if (maximumSize <= 0 || currentPoolSize < maximumSize)
            {
                CurrentPoolSize++;
                Debug.Assert(maximumSize <= 0 || CurrentPoolSize <= maximumSize);
            }
            else
                CountExcessConstructed++;

            obj.gameObject.SetActive(true);
        }
        else
        {
            pool.Remove(obj);
        }
        
        CountInUse++;

        obj.Assign();

        setupAction?.Invoke(obj);

        return obj;
    }
}