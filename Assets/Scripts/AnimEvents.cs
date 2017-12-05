using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    private EnemyController _enemyControllerScript;

    void Awake()
    {
        _enemyControllerScript = (EnemyController)GameObject.Find("EnemyController").GetComponent(typeof(EnemyController));
    }

    public void AnimEventTentacle()
    {
        Enemy temp = (Enemy)_enemyControllerScript.SpawnedEnemy.GetComponent(typeof(Enemy));
        temp.AnimEventAttackThePlayer();
    }



}
