using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnCrates : MonoBehaviour {
    
    private GameObject[] _spawnerArray = new GameObject[4];
    private GameObject[] _equationArray = new GameObject[2];
    
    public GameObject[] prefabOperandArray = new GameObject[20];

    private GameObject _showResult;

    private Calculate _Calculate;
    private DragAndDrop _DragAndDrop;

    private int _rndA;
    private int _rndB;

    private bool _lockSpawn1 = false;
    private bool _lockSpawn2 = false;
    private bool _lockSpawn3 = false;
    private bool _lockSpawn4 = false;

    //getters and setters
    public bool LockSpawn1
    {
        get { return _lockSpawn1; }
        set { _lockSpawn1 = value; }
    }
    public bool LockSpawn2
    {
        get { return _lockSpawn2; }
        set { _lockSpawn2 = value; }
    }
    public bool LockSpawn3
    {
        get { return _lockSpawn3; }
        set { _lockSpawn3 = value; }
    }
    public bool LockSpawn4
    {
        get { return _lockSpawn4; }
        set { _lockSpawn4 = value; }
    }


    void Awake()
    {
        _Calculate = GetComponent<Calculate>();
        _DragAndDrop = GetComponent<DragAndDrop>();

        //get the spawnpoints for the crates
        _spawnerArray[0] = GameObject.Find("Operators/Spawn1");
        _spawnerArray[1] = GameObject.Find("Operators/Spawn2");
        _spawnerArray[2] = GameObject.Find("Operators/Spawn3");
        _spawnerArray[3] = GameObject.Find("Operators/Spawn4");

        //get the equation position
        _equationArray[0] = GameObject.Find("Operators/EquationPositionLeft");
        _equationArray[1] = GameObject.Find("Operators/EquationPositionRight");

        //
        _showResult = GameObject.Find("Operators/ResultSpawn");

    }



    public void SpawnOperand()
    {
        RandomCaseSelection();

        GameObject Temporary_Operand_Handler;
        int operandToSpawn;

        int[] select = { _rndA, _rndB };
        for (int i = 0; i < 2; i++) //going thru 2 times for spawning operandA and operandB
        {

            if (i == 0)
            {
                operandToSpawn = _Calculate.OpA;
            }
            else
            {
                operandToSpawn = _Calculate.OpB;
            }


            switch (select[i])
            {
                case 1:
                    if (!_lockSpawn1)
                    {
                        Temporary_Operand_Handler = Instantiate(prefabOperandArray[operandToSpawn], _spawnerArray[0].transform.position, _spawnerArray[0].transform.rotation) as GameObject;
                        Temporary_Operand_Handler.transform.parent = _spawnerArray[0].transform;
                        _lockSpawn1 = true;
                    }
                    break;
                case 2:
                    if (!_lockSpawn2)
                    {
                        Temporary_Operand_Handler = Instantiate(prefabOperandArray[operandToSpawn], _spawnerArray[1].transform.position, _spawnerArray[1].transform.rotation) as GameObject;
                        Temporary_Operand_Handler.transform.parent = _spawnerArray[1].transform;
                        _lockSpawn2 = true;
                    }
                    break;
                case 3:
                    if (!_lockSpawn3)
                    {
                        Temporary_Operand_Handler = Instantiate(prefabOperandArray[operandToSpawn], _spawnerArray[2].transform.position, _spawnerArray[2].transform.rotation) as GameObject;
                        Temporary_Operand_Handler.transform.parent = _spawnerArray[2].transform;
                        _lockSpawn3 = true;
                    }
                    break;
                case 4:
                    if (!_lockSpawn4)
                    {
                        Temporary_Operand_Handler = Instantiate(prefabOperandArray[operandToSpawn], _spawnerArray[3].transform.position, _spawnerArray[3].transform.rotation) as GameObject;
                        Temporary_Operand_Handler.transform.parent = _spawnerArray[3].transform;
                        _lockSpawn4 = true;
                    }
                    break;
                default:
                    Debug.Log("no case");
                    break;
            }
        }
    }

    public void SpawnRandomCrates()
    {
        GameObject Temporary_Operand_Handler;

        if (!_lockSpawn1)
        {
            int pickOperand = Random.Range(1, 10);
            while(pickOperand == _Calculate.TargetResult)
            {
                pickOperand = Random.Range(1, 10);
            }
            Temporary_Operand_Handler = Instantiate(prefabOperandArray[pickOperand], _spawnerArray[0].transform.position, _spawnerArray[0].transform.rotation) as GameObject;
            Temporary_Operand_Handler.transform.parent = _spawnerArray[0].transform;
            _lockSpawn1 = true;
        }
        if(!_lockSpawn2)
        {
            int pickOperand = Random.Range(1, 10);
            while (pickOperand == _Calculate.TargetResult)
            {
                pickOperand = Random.Range(1, 10);
            }
            Temporary_Operand_Handler = Instantiate(prefabOperandArray[pickOperand], _spawnerArray[1].transform.position, _spawnerArray[1].transform.rotation) as GameObject;
            Temporary_Operand_Handler.transform.parent = _spawnerArray[1].transform;
            _lockSpawn2 = true;
        }
        if(!_lockSpawn3)
        {
            int pickOperand = Random.Range(1, 10);
            while (pickOperand == _Calculate.TargetResult)
            {
                pickOperand = Random.Range(1, 10);
            }
            Temporary_Operand_Handler = Instantiate(prefabOperandArray[pickOperand], _spawnerArray[2].transform.position, _spawnerArray[2].transform.rotation) as GameObject;
            Temporary_Operand_Handler.transform.parent = _spawnerArray[2].transform;
            _lockSpawn3 = true;
        }
        if(!_lockSpawn4)
        {
            int pickOperand = Random.Range(1, 10);
            while (pickOperand == _Calculate.TargetResult)
            {
                pickOperand = Random.Range(1, 10);
            }
            Temporary_Operand_Handler = Instantiate(prefabOperandArray[pickOperand], _spawnerArray[3].transform.position, _spawnerArray[3].transform.rotation) as GameObject;
            Temporary_Operand_Handler.transform.parent = _spawnerArray[3].transform;
            _lockSpawn4 = true;
        }


    }


    void RandomCaseSelection()
    {
        _rndA = Random.Range(1, 5);
        _rndB = Random.Range(1, 5);

        while (_rndA == _rndB)
        {
            _rndB = Random.Range(1, 5);
        }

    }

    public void CleanSpawn()
    {
        //reset the spawn
        if(_lockSpawn1) 
        {
            if(_spawnerArray[0].transform.childCount > 0) //avoid "child out of bounds error"
            {
                Destroy(_spawnerArray[0].transform.GetChild(0).gameObject);
            }
            _lockSpawn1 = false;
        }
        if(_lockSpawn2) 
        {
            if(_spawnerArray[1].transform.childCount > 0) //avoid "child out of bounds error"
            {
                Destroy(_spawnerArray[1].transform.GetChild(0).gameObject);
            }
            _lockSpawn2 = false;
        }
        if (_lockSpawn3)
        {
            if (_spawnerArray[2].transform.childCount > 0) //avoid "child out of bounds error"
            {
                Destroy(_spawnerArray[2].transform.GetChild(0).gameObject);
            }
            _lockSpawn3 = false;
        }
        if(_lockSpawn4)
        {
            if(_spawnerArray[3].transform.childCount > 0) //avoid "child out of bounds error"
            {
                Destroy(_spawnerArray[3].transform.GetChild(0).gameObject);
            }
            _lockSpawn4 = false;
        }
        
    }


    public void CleanEquationSpawn()
    {
        if (_equationArray[0].transform.childCount > 0)
        {
            Destroy(_equationArray[0].transform.GetChild(0).gameObject);
        }
        if (_equationArray[1].transform.childCount > 0)
        {
            Destroy(_equationArray[1].transform.GetChild(0).gameObject);
        }

        _DragAndDrop.equationPositionTrigger = 0;
    }

    public void SpawnResult()
    {
        GameObject Temporary_Show_Result;
        Temporary_Show_Result = Instantiate(prefabOperandArray[_Calculate.ListOfOperands.Sum()], _showResult.transform.position, _showResult.transform.rotation) as GameObject;
        Temporary_Show_Result.transform.parent = _showResult.transform;
    }

}
