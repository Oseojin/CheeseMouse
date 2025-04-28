using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private enum MouseState { Idle, MoveToCheese, Eating, Full, Sick }
    private MouseState currentState = MouseState.Idle;

    [Header("쥐 설정")]
    public float moveSpeed = 1.2f;
    public float eatingTime = 5f;
    public float fullDuration = 10f;
    public float fullnessThreshold = 100f; // 포만감 최댓값
    public float fullnessDecreaseRate = 10f; // 초당 포만감 감소량

    private float currentEatingTime;
    private float currentFullTime;
    private float currentFullness;

    private Transform targetCheese;

    [Header("배탈 설정")]
    public float sickDuration = 8f; // 배탈 지속 시간

    private float currentSickTime;
    private bool lastCheeseWasRotten = false;

    void Update()
    {
        switch (currentState)
        {
            case MouseState.Idle:
                FindCheese();
                break;
            case MouseState.MoveToCheese:
                MoveToCheese();
                break;
            case MouseState.Eating:
                EatingCheese();
                break;
            case MouseState.Full:
                FullRest();
                break;
            case MouseState.Sick:
                SickRest();
                break;
        }

        UpdateFullness();
    }

    void SickRest()
    {
        currentSickTime -= Time.deltaTime;
        if (currentSickTime <= 0f)
        {
            currentState = MouseState.Idle;
        }
    }


    void FindCheese()
    {
        GameObject[] cheeses = GameObject.FindGameObjectsWithTag("Cheese");

        if (cheeses.Length == 0) return;

        // 랜덤 치즈 선택
        GameObject chosen = cheeses[Random.Range(0, cheeses.Length)];
        targetCheese = chosen.transform;
        currentState = MouseState.MoveToCheese;
    }

    void MoveToCheese()
    {
        if (targetCheese == null)
        {
            currentState = MouseState.Idle;
            return;
        }

        Vector3 targetPos = new Vector3(targetCheese.position.x, transform.position.y, targetCheese.position.z);
        Vector3 direction = (targetPos - transform.position).normalized;

        if (direction.magnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f); // 자연스럽게 회전
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            currentEatingTime = eatingTime;
            currentState = MouseState.Eating;
        }
    }


    void EatingCheese()
    {
        if (targetCheese == null)
        {
            currentState = MouseState.Idle;
            return;
        }

        currentEatingTime -= Time.deltaTime;
        if (currentEatingTime <= 0f)
        {
            CheeseBehavior cheeseBehavior = targetCheese.GetComponent<CheeseBehavior>();
            if (cheeseBehavior != null)
            {
                SteamStorageManager.Instance.RecordCheese(
                    cheeseBehavior.data.cheeseName,
                    cheeseBehavior.data.rarity,
                    cheeseBehavior.data.tasteType,
                    cheeseBehavior.data.isRotten
                );

                // 🍴 썩은 치즈 먹었는지 기록
                lastCheeseWasRotten = cheeseBehavior.data.isRotten;
            }

            // 🍽️ CheeseManager에서 제거
            CheeseManager cheeseManager = FindFirstObjectByType<CheeseManager>();
            if (cheeseManager != null)
            {
                cheeseManager.RemoveCheese(targetCheese.gameObject);
            }

            Destroy(targetCheese.gameObject);
            targetCheese = null;

            if (lastCheeseWasRotten)
            {
                // 🤢 썩은 치즈 먹었으면 Sick 상태로
                currentSickTime = sickDuration;
                currentState = MouseState.Sick;
            }
            else if (currentFullness >= fullnessThreshold)
            {
                currentState = MouseState.Full;
                currentFullTime = fullDuration;
            }
            else
            {
                currentState = MouseState.Idle;
            }
        }
    }

    void FullRest()
    {
        if (targetCheese == null)
        {
            WanderRandomly();
        }

        currentFullTime -= Time.deltaTime;
        if (currentFullTime <= 0f)
        {
            currentState = MouseState.Idle;
        }
    }

    void WanderRandomly()
    {
        if (targetCheese == null)
        {
            // 이동 방향 랜덤 설정
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            ).normalized;

            if (randomDirection.magnitude > 0.01f)
            {
                Quaternion toRotation = Quaternion.LookRotation(randomDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 2f); // 천천히 회전
            }

            transform.Translate(Vector3.forward * moveSpeed * 0.5f * Time.deltaTime);
        }
    }



    void UpdateFullness()
    {
        if (currentFullness > 0)
        {
            currentFullness -= fullnessDecreaseRate * Time.deltaTime;
            if (currentFullness < 0)
                currentFullness = 0;
        }
    }
}
