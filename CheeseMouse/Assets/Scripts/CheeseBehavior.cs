using UnityEngine;

public class CheeseBehavior : MonoBehaviour
{
    public Cheese data;
    private float rotTimer;
    private bool isRotten = false;
    private float rottenDestroyTimer = 10f; // 🍃 썩은 후 삭제까지 남은 시간 (10초)

    public void Init(Cheese cheeseData)
    {
        data = new Cheese
        {
            cheeseName = cheeseData.cheeseName,
            rarity = cheeseData.rarity,
            spawnChance = cheeseData.spawnChance,
            rotTime = cheeseData.rotTime,
            size = cheeseData.size,
            tasteType = cheeseData.tasteType,
            isRotten = false
        };

        rotTimer = data.rotTime;
        transform.localScale = data.size;
        GetComponent<Renderer>().material.color = Color.yellow; // 기본 색
    }

    private void Update()
    {
        if (data == null) return;

        if (!isRotten)
        {
            rotTimer -= Time.deltaTime;
            if (rotTimer <= 0f)
            {
                BecomeRotten();
            }
        }
        else
        {
            rottenDestroyTimer -= Time.deltaTime;
            if (rottenDestroyTimer <= 0f)
            {
                RemoveSelf();
            }
        }
    }

    void BecomeRotten()
    {
        data.isRotten = true;
        isRotten = true;
        GetComponent<Renderer>().material.color = Color.green; // 썩은 색
    }

    void RemoveSelf()
    {
        CheeseManager cheeseManager = FindFirstObjectByType<CheeseManager>();
        if (cheeseManager != null)
        {
            cheeseManager.RemoveCheese(gameObject);
        }
        Destroy(gameObject);
    }
}
