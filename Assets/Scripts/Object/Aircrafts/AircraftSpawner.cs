using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AircraftSpawner : ObjectSpawner
{
    public Vector2 SpawnRange;

    private StageData stage => GameplayManager.Current.Stage;

    private Coroutine currentCoroutine;

    private void Start()
    {
        currentCoroutine = this.StartDelayedCoroutine(startSpawnObjectLoop(), 1);
    }

    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        var index = Random.Range(0, stage.AircraftSetting.Length);

        if (stage.AircraftSetting[index].Amount == 0)
            return null;
        
        var pos = new Vector2(Random.Range(transform.position.x - SpawnRange.x, transform.position.x + SpawnRange.x), Random.Range(transform.position.y - SpawnRange.y, transform.position.y + SpawnRange.y));
        var obj = Instantiate(stage.AircraftSetting[index].AircraftInfo.TargetPrefab, pos, Quaternion.identity);
        
        stage.AircraftSetting[index].Amount--;

        return obj;
    }

    private IEnumerator startSpawnObjectLoop()
    {
        SpawnObject();

        yield return new WaitForSeconds(Random.Range(stage.MinSpawnInterval, stage.MaxSpawnInterval));

        currentCoroutine = StartCoroutine(startSpawnObjectLoop());
    }
}