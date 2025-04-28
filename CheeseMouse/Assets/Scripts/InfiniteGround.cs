using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public Transform target; // 쥐 (Mouse)
    public float tileSize = 20f; // Plane 크기 (기본 10x10이면 10)

    private Vector3 lastTargetPosition;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("InfiniteGround: Target not assigned!");
            enabled = false;
            return;
        }
        lastTargetPosition = target.position;
    }

    private void Update()
    {
        Vector3 delta = target.position - lastTargetPosition;

        // X축 이동
        if (Mathf.Abs(delta.x) >= tileSize / 2f)
        {
            float moveX = Mathf.Floor(delta.x / (tileSize / 2f)) * (tileSize / 2f);
            transform.position += new Vector3(moveX, 0, 0);
            lastTargetPosition += new Vector3(moveX, 0, 0);
        }

        // Z축 이동
        if (Mathf.Abs(delta.z) >= tileSize / 2f)
        {
            float moveZ = Mathf.Floor(delta.z / (tileSize / 2f)) * (tileSize / 2f);
            transform.position += new Vector3(0, 0, moveZ);
            lastTargetPosition += new Vector3(0, 0, moveZ);
        }
    }
}
