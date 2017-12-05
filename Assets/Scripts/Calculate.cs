using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Calculate : MonoBehaviour {

    public List<int> ListOfOperands = new List<int>();

    public int TargetResult;
    public GameObject[] _targetResultPrefab = new GameObject[10];

    private GameObject _spawnTargetResult;
    private int _oldTargetResult;

    public int OpA;
    public int OpB;

    void Awake()
    {
        
        _spawnTargetResult = GameObject.Find("Operators/Spawn Target Result");

        for(int i = 2; i < _targetResultPrefab.Length; i++)
        {
            _targetResultPrefab[i] = GameObject.Find("Operators/Spawn Target Result/"+i);
        }

        RandomTargetResult();
        GetAdditionOperand();
    }
    

    public void RandomTargetResult()
    {
        if(_targetResultPrefab[TargetResult] != null)
        {
            DeactivateOldResult();
        }
        TargetResult = Random.Range(2, 10); //Random.Range works with floats! the range(1, 9) is in float 1.000-8.999! so the integer range from (1, 9) is in fact (1, 8)
        ActivateNewResult();
        GetAdditionOperand();
    }

    void GetAdditionOperand()
    {
        int i = Random.Range(1, TargetResult); //Random.Range works with floats! the range(1, 9) is in float 1.000-8.999! so the integer range from (1, 9) is in fact (1, 8)
        OpA = i;
        OpB = TargetResult - i;
    }

    void ActivateNewResult()
    {
        _targetResultPrefab[TargetResult].SetActive(true);
    }

    public void DeactivateOldResult()
    {
        _targetResultPrefab[TargetResult].SetActive(false);
    }


}
