using UnityEngine;

public interface IFollowingObject
{
    /// <summary>
    /// 지정된 오브젝트를 향해 따라갑니다.
    /// </summary>
    /// <param name="obj">따라갈 대상 오브젝트.</param>
    /// <param name="speed">따라가는 속도.</param>
    void FollowTo(GameObject obj, float speed);
}