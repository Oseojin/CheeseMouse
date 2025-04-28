using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (쥐)
    public Vector3 offset = new Vector3(0, 15, -10);
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
