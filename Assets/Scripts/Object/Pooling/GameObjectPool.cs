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

    /// <summary>
    /// 풀링 할 오브젝트들.
    /// </summary>
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

    /// <summary>
    /// 풀에 사용하거나 추가할 새로운 오브젝트를 생성합니다.
    /// </summary>
    /// <param name="targetPrefab">생성할 오브젝트의 프리펩.</param>
    protected virtual T CreateNewGameObject(GameObject targetPrefab = null)
    {
        var o = Instantiate(targetPrefab, transform, true);
        
        o.SetActive(false);
        
        return o.GetComponent<T>();
    }

    /// <summary>
    /// 사용 후 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="pooledGameObject">반환할 오브젝트. 이 풀안에 속해야 함.</param>
    public void Return(PoolableGameObject pooledGameObject)
    {
        if (pooledGameObject is not T typedGameObject)
            throw new ArgumentException("Invalid type", nameof(pooledGameObject));
        
        pooledGameObject.gameObject.SetActive(false);

        if (pooledGameObject.IsInUse)
        {
            // 반환 작업이 오브젝트에서 이루어지지 않은 경우, 일관된 동작을 보장하기 위해 호출함.
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
    
    /// <summary>
    /// 이 풀에 의해 관리됨을 보장하도록 오브젝트를 준비합니다.
    /// </summary>
    /// <param name="obj">이 풀에서 사용 할 오브젝트. null로 지정되어 있으면 <paramref name="createPrefabIfNotExists"/>에 지정된 새 오브젝트를 만듭니다.</param>
    /// <param name="createPrefabIfNotExists">필요시 새로 만들 오브젝트의 프리펩.</param>
    /// <returns>할당이 완료된 오브젝트.</returns>
    private T setupGameObjectForUse(T obj, GameObject createPrefabIfNotExists = null)
    {
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

        return obj;
    }
    
    PoolableGameObject IGameObjectPool.Get(Action<PoolableGameObject> setupAction) => Get(setupAction);

    /// <summary>
    /// 이 풀에서 오브젝트를 가져옵니다.
    /// </summary>
    /// <param name="setupAction">가져온 직후 이 오브젝트에서 수행할 선택적인 작업입니다. 일반적으로 오브젝트를 사용 가능한 상태로 준비하는데 사용합니다.</param>
    /// <returns>오브젝트.</returns>
    public T Get(Action<T> setupAction = null)
    {
        T obj = setupGameObjectForUse(pool.LastOrDefault(), poolTarget[Random.Range(0, poolTarget.Length)]);

        setupAction?.Invoke(obj);

        return obj;
    }

    public U Get<U>(Action<U> setupAction = null, GameObject createPrefabIfNotExists = null)
        where U : T
    {
        var obj = setupGameObjectForUse(pool.LastOrDefault(o => o.GetType() == typeof(U)), createPrefabIfNotExists) as U;

        setupAction?.Invoke(obj);

        return obj;
    }

    public T Get(Func<T, bool> predicate, Action<T> setupAction = null, GameObject createPrefabIfNotExists = null)
    {
        var obj = setupGameObjectForUse(pool.Where(predicate).LastOrDefault(), createPrefabIfNotExists);

        setupAction?.Invoke(obj);

        return obj;
    }
}