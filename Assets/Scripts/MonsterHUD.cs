using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHUD : MonoBehaviour {

    public Sprite[] HealthSprites;

    private Image _healthHUD;
    private Enemy _EnemyScript;
    private EnemyController _enemyControllerScript;
    private GameObject _attackBarHUD;

    void Awake()
    {
        _enemyControllerScript = GameObject.Find("EnemyController").GetComponent<EnemyController>();
        _healthHUD = GameObject.Find("Canvas/Unpaused/Monster Health/Image").GetComponent<Image>();
        _attackBarHUD = GameObject.Find("Canvas/Unpaused/Monster Attack Bar");
    }

    void Start()
    {
        _EnemyScript = _enemyControllerScript.SpawnedEnemy.GetComponent<Enemy>();
        _healthHUD.enabled = true;
        _attackBarHUD.SetActive(true);
    }
    
    void Update()
    {
        if(_enemyControllerScript.EnemyIsAlive == true)
        {
            if (_EnemyScript.EnemyHealth <= 0)
            {
                _healthHUD.enabled = false;
                _attackBarHUD.SetActive(false);
            }

            _healthHUD.sprite = HealthSprites[_EnemyScript.EnemyHealth];
        }

    }




}
