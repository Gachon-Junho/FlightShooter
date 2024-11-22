using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AircraftSpawner : ObjectSpawner
{
    public SpawnPointSetting[] SpawnPointSettings;

    private StageData stage => GameplayManager.Current.Stage;

    private Coroutine currentCoroutine;

    private void Start()
    {
        currentCoroutine = this.StartDelayedCoroutine(startSpawnObjectLoop(), 1);
    }

    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        var spwnIdx = Random.Range(0, SpawnPointSettings.Length);
        var index = Random.Range(0, stage.AircraftSetting.Length);

        if (stage.AircraftSetting[index].Amount == 0)
            return null;
        
        var pos = new Vector2(Random.Range(SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.x - SpawnPointSettings[spwnIdx].SpawnRange.x, SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.x + SpawnPointSettings[spwnIdx].SpawnRange.x), Random.Range(SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.y - SpawnPointSettings[spwnIdx].SpawnRange.y, SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.y + SpawnPointSettings[spwnIdx].SpawnRange.y));
        var aircraft = Instantiate(stage.AircraftSetting[index].AircraftInfo.TargetPrefab, pos, stage.AircraftSetting[index].AircraftInfo.TargetPrefab.transform.rotation).GetComponent<AttackableAircraft>();
        
        aircraft.Initialize(stage.AircraftSetting[index].AircraftInfo, stage);
        stage.AircraftSetting[index].Amount--;

        return aircraft.gameObject;
    }

    private IEnumerator startSpawnObjectLoop()
    {
        SpawnObject();

        yield return new WaitForSeconds(Random.Range(stage.MinSpawnInterval, stage.MaxSpawnInterval));

        currentCoroutine = StartCoroutine(startSpawnObjectLoop());
    }

    [Serializable]
    public class SpawnPointSetting
    {
        public GameObject SpawnPoint;
        public Vector2 SpawnRange;
    }
}