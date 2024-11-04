
using System.Collections;
using UnityEngine;

public interface IMovableObject
{
    IEnumerator MoveTo(Vector3 newPosition, float speed);
}
