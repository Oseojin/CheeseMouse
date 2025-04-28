using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    [Header("치즈 스폰 설정")]
    public GameObject cheesePrefab;
    public int maxCheeseCount = 100;
    public float spawnRadius = 10f; // 쥐 주변 몇 미터 안에서 스폰

    private GameObject mouse;

    private List<GameObject> spawnedCheeses = new List<GameObject>();
    private List<Cheese> cheesePool = new List<Cheese>(); // ⬅️ 치즈 종류들

    private void Start()
    {
        InitializeCheesePool();
        mouse = GameObject.FindWithTag("Player"); // 쥐에 Tag "Player" 달아줘야 함
        StartCoroutine(SpawnCheeseLoop());
    }

    private IEnumerator SpawnCheeseLoop()
    {
        yield return new WaitForSeconds(1f); // 초기 대기

        while (true)
        {
            SpawnCheese();
            float nextSpawnTime = Random.Range(1f, 5f); // 랜덤 1~5초
            yield return new WaitForSeconds(nextSpawnTime);
        }
    }


    void InitializeCheesePool()
    {
        cheesePool.Add(new Cheese
        {
            cheeseName = "브리 치즈",
            rarity = "Common",
            spawnChance = 60f,
            rotTime = 20f,
            size = new Vector3(0.3f, 0.3f, 0.3f),
            tasteType = "Sweet"
        });

        cheesePool.Add(new Cheese
        {
            cheeseName = "체다 치즈",
            rarity = "Common",
            spawnChance = 20f,
            rotTime = 25f,
            size = new Vector3(0.35f, 0.35f, 0.35f),
            tasteType = "Salty"
        });

        cheesePool.Add(new Cheese
        {
            cheeseName = "고르곤졸라",
            rarity = "Rare",
            spawnChance = 10f,
            rotTime = 30f,
            size = new Vector3(0.4f, 0.4f, 0.4f),
            tasteType = "Bitter"
        });

        cheesePool.Add(new Cheese
        {
            cheeseName = "블루 치즈",
            rarity = "Epic",
            spawnChance = 7f,
            rotTime = 40f,
            size = new Vector3(0.45f, 0.45f, 0.45f),
            tasteType = "Savory"
        });

        cheesePool.Add(new Cheese
        {
            cheeseName = "캐비아 치즈",
            rarity = "Legendary",
            spawnChance = 3f,
            rotTime = 50f,
            size = new Vector3(0.5f, 0.5f, 0.5f),
            tasteType = "Umami"
        });
    }

    void SpawnCheese()
    {
        if (mouse == null || spawnedCheeses.Count >= maxCheeseCount) return;

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(
            mouse.transform.position.x + randomCircle.x,
            0.5f,
            mouse.transform.position.z + randomCircle.y
        );

        GameObject cheese = Instantiate(cheesePrefab, spawnPos, Quaternion.identity);

        CheeseBehavior behavior = cheese.AddComponent<CheeseBehavior>();
        behavior.Init(RandomCheeseData());

        spawnedCheeses.Add(cheese);
    }

    Cheese RandomCheeseData()
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var cheese in cheesePool)
        {
            cumulative += cheese.spawnChance;
            if (roll <= cumulative)
            {
                return cheese;
            }
        }

        // 만약 오류로 다 실패하면 가장 일반 치즈 반환
        return cheesePool[0];
    }

    public void RemoveCheese(GameObject cheese)
    {
        if (spawnedCheeses.Contains(cheese))
        {
            spawnedCheeses.Remove(cheese);
        }
    }

}
