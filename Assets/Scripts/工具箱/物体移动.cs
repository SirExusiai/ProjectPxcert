// MoveObjectAction.cs
using UnityEngine;
using System.Collections;

public class MoveObjectAction : MonoBehaviour
{
    [Header("动画设置")]
    public Transform targetTransform; // 物体要移动到的目标位置
    public float duration = 2.0f;     // 完成移动需要的时间（秒）

    [Header("完成后的行为")]
    public bool disableOnComplete = true; // 移动完成后是否禁用（消失）

    public void Execute()
    {
        if (targetTransform != null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.LogError("物体 " + gameObject.name + " 的目标位置未设置！");
        }
    }

    private IEnumerator MoveCoroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetTransform.position;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // 计算当前时间占总时长的百分比
            float t = timeElapsed / duration;
            // 使用 Lerp (线性插值) 来计算当前帧应该在的位置
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            
            // 累加经过的时间
            timeElapsed += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }

        // 确保动画结束后，物体精确地在目标位置
        transform.position = endPosition;
        
        // 根据设置决定是否禁用物体
        if (disableOnComplete)
        {
            gameObject.SetActive(false);
        }
    }
}