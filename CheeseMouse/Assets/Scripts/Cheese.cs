using UnityEngine;

[System.Serializable]
public class Cheese
{
    public string cheeseName;   // 치즈 이름
    public string rarity;       // 희귀도 (Common, Rare, etc)
    public float spawnChance;   // 등장 확률
    public float rotTime;       // 썩는데 걸리는 시간 (초)
    public Vector3 size;        // 치즈 크기
    public string tasteType;    // 맛 종류 (Sweet, Salty 등)
    public bool isRotten = false; // 썩었는지 여부
}
