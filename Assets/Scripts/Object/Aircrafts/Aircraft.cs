using System.Collections;
using UnityEngine;

public abstract class Aircraft : MonoBehaviour, IMovableObject, IHasHitPoint
{
    public string Name { get; private set; }
    public float Speed { get; private set; }
    public HitPoint HP { get; private set; }

    public Coroutine CurrentTransformCoroutine { get; protected set; }
    
    protected Vector3 MoveDirection;

    public virtual void Initialize(AircraftInfo info, StageData stage = null)
    {
        Name = info.Name;
        Speed = info.Speed;
        HP = stage == null ? info.HP : new HitPoint(info.HP.MaxHP * stage.HPWeight);
    }

    private void Update()
    {
        // MoveTo()를 쓰려했지만 초반에 멘탈나가서 그냥 이렇게 해결함
        if (MoveDirection != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + MoveDirection, Speed * Time.deltaTime);
        }
    }

    public virtual IEnumerator MoveTo(Vector3 newPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        if (!transform.position.Equals(newPosition))
        {
            yield return null;
            CurrentTransformCoroutine = StartCoroutine(MoveTo(newPosition, speed));
        }            
    }

    public IEnumerator MoveToOffset(Vector3 offset, float speed) => MoveTo(transform.position + offset, speed);
}