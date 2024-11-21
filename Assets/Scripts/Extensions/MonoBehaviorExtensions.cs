using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MonoBehaviorExtensions
{
    public static void LoadSceneAsync(this MonoBehaviour mono, string sceneName)
    {
        mono.StartCoroutine(loadScene());

        IEnumerator loadScene()
        {
            var loadingTask = SceneManager.LoadSceneAsync(sceneName);

            while (!loadingTask.isDone)
                yield return null;
        }
    }

    public static Coroutine StartDelayedCoroutine(this MonoBehaviour mono, IEnumerator routine, float timeUntilStart)
    {
        var cor = mono.StartCoroutine(startDelayed());
        
        IEnumerator startDelayed()
        {
            yield return new WaitForSeconds(timeUntilStart);

            cor = mono.StartCoroutine(routine);
        }

        return cor;
    }
}