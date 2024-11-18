using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public int CurrentPoolSize { get; private set; }

    public int CountInUse { get; private set; }

    public int CountExcessConstructed { get; private set; }

    public int CountAvailable => pool.Count;

    protected virtual void Awake()
    {
        if (initialSize > maximumSize)
            throw new ArgumentOutOfRangeException(nameof(initialSize), "Initial size must be less than or equal to maximum size.");
        
        for (int i = 0; i < initialSize; i++)
            pool.Add(create(poolTarget[i / (initialSize / poolTarget.Length)]));

        CurrentPoolSize = initialSize;
    }

    private T create(GameObject targetPrefab = null)
    {
        var o = CreateNewGameObject(targetPrefab);
        
        o.SetPool(this);

        return o;
    }

    protected virtual T CreateNewGameObject(GameObject targetPrefab = null)
    {
        var o = Instantiate(targetPrefab, transform, true);
        
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
            obj = create(poolTarget[Random.Range(0, poolTarget.Length)]);

            if (maximumSize <= 0 || CurrentPoolSize < maximumSize)
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

    public U Get<U>(Action<U> setupAction = null, GameObject createPrefabIfNotExists = null)
        where U : T
    {
        var obj = pool.LastOrDefault(o => o.GetType() == typeof(U)) as U;

        if (obj == null)
        {
            obj = create(createPrefabIfNotExists) as U;

            if (maximumSize <= 0 || CurrentPoolSize < maximumSize)
            {
                CurrentPoolSize++;
                Debug.Assert(maximumSize <= 0 || CurrentPoolSize <= maximumSize);
            }
            else
            {
                CountExcessConstructed++;
            }
        }
        else
        {
            pool.Remove(obj);
        }
        
        obj.gameObject.SetActive(true);
        CountInUse++;

        obj.Assign();

        setupAction?.Invoke(obj);

        return obj;
    }

    public T Get(Func<T, bool> predicate, Action<T> setupAction = null, GameObject createPrefabIfNotExists = null)
    {
        var obj = pool.Where(predicate).FirstOrDefault();
        
        if (obj == null)
        {
            obj = create(createPrefabIfNotExists);

            if (maximumSize <= 0 || CurrentPoolSize < maximumSize)
            {
                CurrentPoolSize++;
                Debug.Assert(maximumSize <= 0 || CurrentPoolSize <= maximumSize);
            }
            else
            {
                CountExcessConstructed++;
            }
        }
        else
        {
            pool.Remove(obj);
        }
        
        obj.gameObject.SetActive(true);
        CountInUse++;

        obj.Assign();

        setupAction?.Invoke(obj);

        return obj;
    }
}