using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    public GameObject SpawnTarget => spawnTarget;

    [SerializeField] 
    private GameObject spawnTarget;

    public abstract GameObject SpawnObject();
}