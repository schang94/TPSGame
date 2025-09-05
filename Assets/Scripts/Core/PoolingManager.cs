using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_Instance;
    public GameObject enemyPrefab;
    private List<GameObject> enemyList = new List<GameObject>();
    private int maxEnemy = 5;


    public GameObject playerPrefab;
    private List<GameObject> playerList = new List<GameObject>();
    private int maxPlayer = 5;
    
    void Awake()
    {
        if (p_Instance == null)
            p_Instance = this;
        else if (p_Instance != this)
            Destroy(gameObject);

        StartCoroutine(CreatePlayer());
        StartCoroutine(CreateEnemy());
    }

    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(1f);

        GameObject obj = new GameObject("EnemyObjects") ;

        for (int i = 0; i < maxEnemy; i++)
        {
            var enemy = Instantiate(enemyPrefab, obj.transform);
            enemy.name = $"{i + 1} enemy";
            enemy.SetActive(false);
            
            enemyList.Add(enemy);
        }
    }

    public GameObject GetEenemy()
    {
        foreach (var enemy in enemyList)
        {
            if (!enemy.activeSelf)
            {
                return enemy;
            }
        }
        return null;
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject obj = new GameObject("PlayerObjects");

        for (int i = 0; i < maxPlayer; i++)
        {
            var player = Instantiate(playerPrefab, obj.transform);
            player.name = $"{i + 1} player";
            player.SetActive(false);

            playerList.Add(player);
        }
    }

    public GameObject GetPlayer()
    {
        foreach (var player in playerList)
        {
            if (!player.activeSelf)
            {
                return player;
            }
        }
        return null;
    }
}
