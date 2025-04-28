using UnityEngine;

[System.Serializable]
public class Cheese
{
    public string cheeseName;   // ġ�� �̸�
    public string rarity;       // ��͵� (Common, Rare, etc)
    public float spawnChance;   // ���� Ȯ��
    public float rotTime;       // ��µ� �ɸ��� �ð� (��)
    public Vector3 size;        // ġ�� ũ��
    public string tasteType;    // �� ���� (Sweet, Salty ��)
    public bool isRotten = false; // ������� ����
}
