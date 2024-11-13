using System;
using System.Collections;
using UnityEngine;

public abstract class Aircraft : MonoBehaviour, IMovableObject, IHasHitPoint
{
    public string Name { get; private set; }
    public float Speed { get; private set; }
    public HitPoint HP { get; private set; }

    public IEnumerator CurrentTransformCoroutine { get; protected set; }
    
    protected Vector3 MoveDirection;

    public virtual void Initialize(AircraftInfo info, StageData stage = null)
    {
        Name = info.Name;
        Speed = info.Speed;
        HP = stage == null ? info.HP : new HitPoint(info.HP.MaxHP * stage.HPWeight);
    }

    private void Update()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + MoveDirection, Speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 쓰읍... 작동이 안되는데 이유를 몰라요.
    /// </summary>
    /// <param name="newPosition">이동할 위치</param>
    /// <param name="speed">속도</param>
    /// <returns>현재 수행 중인 트랜스폼 코루틴.</returns>
    public virtual IEnumerator MoveTo(Vector3 newPosition, float speed)
    {
        Debug.Log("MoveTo");
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        if (!transform.position.Equals(newPosition))
        {
            yield return new WaitForFixedUpdate();
            CurrentTransformCoroutine = MoveTo(newPosition, speed);
        }
    }

    public IEnumerator MoveToOffset(Vector3 offset, float speed) => MoveTo(transform.position + offset, speed);
}