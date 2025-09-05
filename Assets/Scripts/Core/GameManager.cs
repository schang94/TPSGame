using Cinemachine;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int killCount = 0;
    public Text killtxt;
    public PatrolPoint patrolPoint;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        KillLoad();
        StartCoroutine(spwanPlayer());
        StartCoroutine(spawnEnemy());
    }

    public void KillLoad()
    {
        killCount = PlayerPrefs.GetInt("KILL", 0);
        killtxt.text = $"KILL : {killCount:000}";
    }
    public void KillUpdate()
    {
        killCount++;
        killtxt.text = $"KILL : {killCount:000}";
        PlayerPrefs.SetInt("KILL", killCount);
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            var enemy = PoolingManager.p_Instance.GetEenemy();
            if (enemy == null) break;
            var enemyCtrl = enemy.GetComponent<AIController>();
            enemyCtrl.curWayPointIdx = Random.Range(0, patrolPoint.WayPointCount());
            enemy.transform.position = patrolPoint.GetWayPoint(enemyCtrl.curWayPointIdx);
            enemy.SetActive(true);
        }
    }
    IEnumerator spwanPlayer()
    {
        yield return new WaitForSeconds(2f);
        var player = PoolingManager.p_Instance.GetPlayer();
        player.transform.position = new Vector3(-50, 0, -20);
        player.SetActive(true);
    }
}
