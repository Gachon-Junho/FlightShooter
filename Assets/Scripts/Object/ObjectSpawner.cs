using System;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField] 
    public GameObject SpawnTarget;

    public abstract GameObject SpawnObject(Action<GameObject> setupAction = null);
}