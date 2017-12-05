using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorsAnimEvents : MonoBehaviour
{

    public GameObject SmokePFB;

    private GameObject _smokeSpawn;

    private SpawnCrates _spawnCratesScript;

    

    void Awake()
    {
        _smokeSpawn = GameObject.Find("Operators/SmokeSpawn");
        _spawnCratesScript = (SpawnCrates)GameObject.Find("GameController").GetComponent(typeof(SpawnCrates));
    }

    void SpawnSmoke()
    {
        _spawnCratesScript.CleanEquationSpawn();
        GameObject Temporary_Particle_Handler;
        Temporary_Particle_Handler = Instantiate(SmokePFB, _smokeSpawn.transform.position, _smokeSpawn.transform.rotation) as GameObject;
        Destroy(Temporary_Particle_Handler, 3.0f);
    }


    

}
