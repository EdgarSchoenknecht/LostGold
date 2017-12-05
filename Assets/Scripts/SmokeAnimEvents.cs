using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAnimEvents : MonoBehaviour {

    private SpawnCrates _spawnCratesScript;
    
    void Awake()
    {
        _spawnCratesScript = (SpawnCrates)GameObject.Find("GameController").GetComponent(typeof(SpawnCrates));
    }

    void SpawnResult()
    {
        _spawnCratesScript.SpawnResult();
    }
}
