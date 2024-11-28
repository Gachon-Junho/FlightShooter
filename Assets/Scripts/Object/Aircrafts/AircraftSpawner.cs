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
        // 무작위로 스폰지점과 기체 선택 
        var spwnIdx = Random.Range(0, SpawnPointSettings.Length);
        var index = Random.Range(0, stage.AircraftSetting.Length);

        if (stage.AircraftSetting[index].Amount == 0)
            return null;
        
        // 스폰 지점을 기준으로 임의값을 부여해 실제 스폰지점 설정
        var pos = new Vector2(Random.Range(SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.x - SpawnPointSettings[spwnIdx].SpawnRange.x, SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.x + SpawnPointSettings[spwnIdx].SpawnRange.x), Random.Range(SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.y - SpawnPointSettings[spwnIdx].SpawnRange.y, SpawnPointSettings[spwnIdx].SpawnPoint.transform.position.y + SpawnPointSettings[spwnIdx].SpawnRange.y));
        var aircraft = Instantiate(stage.AircraftSetting[index].AircraftInfo.TargetPrefab, pos, stage.AircraftSetting[index].AircraftInfo.TargetPrefab.transform.rotation).GetComponent<AttackableAircraft>();
        
        aircraft.Initialize(stage.AircraftSetting[index].AircraftInfo, stage);
        stage.AircraftSetting[index].Amount--;

        return aircraft.gameObject;
    }

    /// <summary>
    /// 일정 간격으로 오브젝트를 생성하도록하는 루프를 시작합니다.
    /// </summary>
    /// <remarks>
    /// 루프를 중단하려면 코루틴을 사용해 중지해야 합니다.
    /// </remarks>
    private IEnumerator startSpawnObjectLoop()
    {
        // TODO: while문을 사용해 코루틴 루프 구현.
        
        SpawnObject();

        yield return new WaitForSeconds(Random.Range(stage.MinSpawnInterval, stage.MaxSpawnInterval));
        
        // while문을 사용하면 되는데 잘 몰랐어서 이렇게 구현이 되었음.
        currentCoroutine = StartCoroutine(startSpawnObjectLoop());
    }

    /// <summary>
    /// 스폰지점에 대한 설정 값을 가진 객체입니다.
    /// </summary>
    [Serializable]
    public class SpawnPointSetting
    {
        public GameObject SpawnPoint;
        public Vector2 SpawnRange;
    }
}