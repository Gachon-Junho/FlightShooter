using UnityEngine;

public abstract class Aircraft : MonoBehaviour
{
    public abstract string Name { get; }
    public abstract double Speed { get; }
    public abstract int HP { get; }

    public virtual void MoveTo(Vector3 newPosition, double duration = 0)
    {
        
    }

    public void MoveToOffset(Vector3 offset, double duration = 0) => MoveTo(transform.position + offset, duration);
}