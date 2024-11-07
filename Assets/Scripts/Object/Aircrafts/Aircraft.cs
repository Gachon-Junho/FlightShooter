using System;
using System.Collections;
using UnityEngine;

public abstract class Aircraft : MonoBehaviour, IMovableObject, IHasHitPoint
{
    public string Name { get; private set; }
    public float Speed { get; private set; }
    public HitPoint HP { get; private set; }

    public IEnumerator CurrentTransformCoroutine { get; protected set; }

    public virtual void Initialize(AircraftInfo info, StageData stage = null)
    {
        Name = info.Name;
        Speed = info.Speed;
        HP = stage == null ? info.HP : new HitPoint(info.HP.MaxHP * stage.HPWeight);
    }

    public virtual IEnumerator MoveTo(Vector3 newPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        if (!transform.position.Equals(newPosition))
        {
            yield return new WaitForFixedUpdate();
            CurrentTransformCoroutine = MoveTo(newPosition, speed);
        }
    }

    public IEnumerator MoveToOffset(Vector3 offset, float speed) => MoveTo(transform.position + offset, speed);
}