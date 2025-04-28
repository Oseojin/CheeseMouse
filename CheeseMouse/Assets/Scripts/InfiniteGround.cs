using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public Transform target; // �� (Mouse)
    public float tileSize = 20f; // Plane ũ�� (�⺻ 10x10�̸� 10)

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

        // X�� �̵�
        if (Mathf.Abs(delta.x) >= tileSize / 2f)
        {
            float moveX = Mathf.Floor(delta.x / (tileSize / 2f)) * (tileSize / 2f);
            transform.position += new Vector3(moveX, 0, 0);
            lastTargetPosition += new Vector3(moveX, 0, 0);
        }

        // Z�� �̵�
        if (Mathf.Abs(delta.z) >= tileSize / 2f)
        {
            float moveZ = Mathf.Floor(delta.z / (tileSize / 2f)) * (tileSize / 2f);
            transform.position += new Vector3(0, 0, moveZ);
            lastTargetPosition += new Vector3(0, 0, moveZ);
        }
    }
}
