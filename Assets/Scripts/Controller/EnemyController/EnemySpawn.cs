/****************************************************
    文件：EnemySpawn.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/16 18:13:05
    功能：敌人生成器
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Singleton<EnemySpawn>
{
    public int WaveNumber => waveNumber;
    public float WaveInterval => waveInterval;

    [SerializeField] private bool spawnEnemy = true;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject waveUI;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float waveInterval = 1f;
    [SerializeField] private int minEnemyCount = 4;
    [SerializeField] private int maxEnemyCount = 10;


    private List<GameObject> enemyList;
    private WaitForSeconds waitForSpawn;
    private WaitForSeconds waitForWave;
    private WaitUntil waitUntilEmpty;
    private int enemyCount;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitForSpawn = new WaitForSeconds(spawnInterval);
        waitForWave = new WaitForSeconds(waveInterval);
        waitUntilEmpty = new WaitUntil(() => enemyList.Count == 0);
    }

    private IEnumerator Start()
    {
        while (spawnEnemy)
        {
            yield return waitUntilEmpty;
            waveUI.SetActive(true);
            yield return waitForWave;
            waveUI.SetActive(false);
            StartCoroutine(SpawnCoroutine());
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        enemyCount = Mathf.Clamp(enemyCount, minEnemyCount + waveNumber / 3, maxEnemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
            yield return waitForSpawn;
        }

        waveNumber += 1;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}