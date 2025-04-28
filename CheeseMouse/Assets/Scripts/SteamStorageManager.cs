using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectedCheese
{
    public string cheeseName;
    public string rarity;
    public string tasteType;
    public bool wasRotten;
    public System.DateTime eatenAt;
}

public class SteamStorageManager : MonoBehaviour
{
    public static SteamStorageManager Instance;

    private List<CollectedCheese> collectedCheeses = new List<CollectedCheese>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �Ѿ�� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecordCheese(string name, string rarity, string tasteType, bool isRotten)
    {
        CollectedCheese newRecord = new CollectedCheese
        {
            cheeseName = name,
            rarity = rarity,
            tasteType = tasteType,
            wasRotten = isRotten,
            eatenAt = System.DateTime.Now
        };

        collectedCheeses.Add(newRecord);

        Debug.Log($"[SteamStorage] {name} ġ� �����߽��ϴ�! (�����?: {isRotten})");
    }

    public List<CollectedCheese> GetCollectedCheeses()
    {
        return collectedCheeses;
    }
}
