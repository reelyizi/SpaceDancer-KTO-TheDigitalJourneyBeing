using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject bossHead, bossLeftHand, bossRightHand;
    [SerializeField] private List<Transform> bubbleHoles = new List<Transform>();
    [SerializeField] private GameObject bossBubble;
    [Range(1, 30)] [SerializeField] float minBubbleSpawnRate, maxBubbleSpawnRate;
    private List<float> spawnRate;
    private float timer = 0;
    [SerializeField] private float bossHeadHealth, bossLeftHandHealth, bossRightHandHealth;

    [SerializeField] private GameObject scoreText;
    [SerializeField] private int bossHeadScore, bossHandScore;

    #region Properties

    public float BossHeadHealth
    {
        get { return bossHeadHealth; }
        set
        {
            bossHeadHealth -= value;
            if (bossHeadHealth <= 0)
            {
                bossHeadHealth = 0;
            }
        }
    }
    public float BossLeftHandHealth
    {
        get { return bossLeftHandHealth; }
        set
        {
            bossLeftHandHealth -= value;
            if (bossLeftHandHealth <= 0)
            {
                bossLeftHandHealth = 0;
            }
        }
    }
    public float BossRightHandHealth
    {
        get { return bossRightHandHealth; }
        set
        {
            bossRightHandHealth -= value;
            if (bossRightHandHealth <= 0)
            {
                bossRightHandHealth = 0;
            }
        }
    }

    #endregion

    void Start()
    {
        SetSpawnRate(1.5f);
    }

    void Update()
    {
        CheckHealthStatus();
        timer += Time.deltaTime;
        for (int i = 0; i < spawnRate.Count; i++)
        {
            if (timer >= spawnRate[i])
            {
                spawnRate.RemoveAt(i);
                GameObject obj = Instantiate(bossBubble, bubbleHoles[i].position, Quaternion.identity, GameObject.Find("Bubble").transform);
                obj.GetComponent<Bubble>().applyForce = true;
                //spawnRate[i] = Random.Range(minBubbleSpawnRate, maxBubbleSpawnRate);
            }
        }
        if (timer >= maxBubbleSpawnRate)
        {
            SetSpawnRate(0);
            timer = 0f;
        }
    }

    private void CheckHealthStatus()
    {
        if (bossLeftHandHealth == 0 && bossLeftHand.GetComponent<BoxCollider2D>().enabled)
        {
            bossLeftHand.GetComponent<BoxCollider2D>().enabled = false;
            SetScore(bossHandScore);
        }
        if (bossRightHandHealth == 0 && bossRightHand.GetComponent<BoxCollider2D>().enabled)
        {
            bossRightHand.GetComponent<BoxCollider2D>().enabled = false;
            SetScore(bossHandScore);
        }
        if (bossHeadHealth == 0 && bossHead.GetComponent<BoxCollider2D>().enabled)
        {
            Debug.Log("A");
            bossHead.GetComponent<BoxCollider2D>().enabled = false;
            SetScore(bossHeadScore);
        }
    }

    private void SetSpawnRate(float additionalTime)
    {
        spawnRate = new List<float>();
        for (int i = 0; i < bubbleHoles.Count; i++)
        {
            spawnRate.Add(Random.Range(minBubbleSpawnRate, maxBubbleSpawnRate) + additionalTime);
        }
    }

    private void SetScore(int score)
    {
        GameManager.score += score;
        GameObject obj = Instantiate(scoreText, Camera.main.WorldToScreenPoint(Vector3.zero), Quaternion.identity, GameObject.Find("Holder").transform);
        obj.GetComponent<BubbleScoreText>().SetText(score);
    }
}
