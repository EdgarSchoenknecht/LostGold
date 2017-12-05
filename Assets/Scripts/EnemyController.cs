using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public string WhatShouldSpawn;
    public GameObject MonsterPrefab;
    public GameObject EelPrefab;
    public GameObject KrakenPrefab;
    public IngameButtonManager IngameButtonManagerScript;

    private bool _enemyIsAlive = false;
    private bool _playerVictorious;
    public GameObject SpawnedEnemy;
    private float _wait;
    private Vector3 _monsterSpawn;
    private Vector3 _eelSpawn;
    private Vector3 _krakenSpawn;
    private Vector3 _enemySpawnRotation;
    private int _counter;


    public bool EnemyIsAlive
    {
        get { return _enemyIsAlive; }
        set { _enemyIsAlive = value; }
    }
    public bool PlayerVictorious
    {
        get { return _playerVictorious; }
        set { _playerVictorious = value; }
    }

    void Awake()
    {
        _monsterSpawn = GameObject.Find("EnemyController/MonsterWaypoints/MonsterSpawn").GetComponent<Transform>().position;
        _eelSpawn = GameObject.Find("EnemyController/MonsterWaypoints/EelSpawn").GetComponent<Transform>().position;
        _krakenSpawn = GameObject.Find("EnemyController/MonsterWaypoints/KrakenSpawn").GetComponent<Transform>().position;

    }

    void Update()
    {
        ////spawn monster endless after a specific time
        //if (SpawnedEnemy == null && _enemyIsAlive)
        //{

        //    Timer();
        //    if (_wait >= 3)
        //    {
        //        _enemyIsAlive = false;
        //        SpawnEnemy();
        //        _wait = 0;
        //    }
        //}

        if(_playerVictorious && _counter == 0)
        {
            for (_counter = 0; _counter < 1; _counter++)
            {
                IngameButtonManagerScript.Victory();
            }
        }
    }

    public void SpawnEnemy()
    {
        if(WhatShouldSpawn.Equals("Monster"))
        {
            GameObject Temporary_Enemy_Handler;
            Temporary_Enemy_Handler = Instantiate(MonsterPrefab, _monsterSpawn, gameObject.transform.rotation) as GameObject;
            Temporary_Enemy_Handler.transform.Rotate(Vector3.down * 90);
            Temporary_Enemy_Handler.transform.parent = gameObject.transform;
            SpawnedEnemy = Temporary_Enemy_Handler;
        }
        else if(WhatShouldSpawn.Equals("Eel"))
        {
            GameObject Temporary_Enemy_Handler;
            Temporary_Enemy_Handler = Instantiate(EelPrefab, _eelSpawn, gameObject.transform.rotation) as GameObject;
            Temporary_Enemy_Handler.transform.Rotate(Vector3.down * 180);
            Temporary_Enemy_Handler.transform.parent = gameObject.transform;
            SpawnedEnemy = Temporary_Enemy_Handler;
        }
        else if (WhatShouldSpawn.Equals("Kraken"))
        {
            GameObject Temporary_Enemy_Handler;
            Temporary_Enemy_Handler = Instantiate(KrakenPrefab, _krakenSpawn, gameObject.transform.rotation) as GameObject;
            Temporary_Enemy_Handler.transform.Rotate(Vector3.down * 0);
            Temporary_Enemy_Handler.transform.parent = gameObject.transform;
            SpawnedEnemy = Temporary_Enemy_Handler;
        }

        _enemyIsAlive = true;
    }

    void Timer()
    {
        _wait += Time.deltaTime;
    }
}
