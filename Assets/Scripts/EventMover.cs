using UnityEngine;
using System.Collections;

public class EventMover : MonoBehaviour
{
    [Header("动画设置")]
    public Transform targetPosition; // 物体要移动到的目标位置
    public float moveSpeed = 10f;    // 移动速度

    // 这个公共方法将由“事件触发器”来调用
    public void StartEvent()
    {
        // 确保目标位置已设置
        if (targetPosition != null)
        {
            StartCoroutine(MoveAndDisappear());
        }
        else
        {
            Debug.LogError("物体 " + gameObject.name + " 的目标位置未设置！");
        }
    }

    private IEnumerator MoveAndDisappear()
    {
        // 当物体与目标位置的距离大于一个很小的值时，就继续移动
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.01f)
        {
            // 使用 MoveTowards 以恒定速度移动
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            // 等待下一帧
            yield return null; 
        }

        // 确保最终位置精确
        transform.position = targetPosition.position;
        
        // 移动完成后，等待一小会儿（可选，效果更好）
        yield return new WaitForSeconds(0.2f);

        // 消失
        gameObject.SetActive(false);
    }
}