using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AircraftSpawner : ObjectSpawner
{
    public Vector2 SpawnRange;

    private StageData stage => GameplayManager.Current.Stage;
    
    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        var index = Random.Range(0, stage.AircraftSetting.Length);
        var pos = new Vector2(Random.Range(transform.position.x - SpawnRange.x, transform.position.x + SpawnRange.x), Random.Range(transform.position.y - SpawnRange.y, transform.position.y + SpawnRange.y));
        var obj = Instantiate(stage.AircraftSetting[index].AircraftInfo.TargetPrefab, pos, Quaternion.identity);
        
        stage.AircraftSetting[index].Amount--;

        return obj;
    }
}