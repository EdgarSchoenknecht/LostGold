using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public int ChanceToWait;
    public float WaitTime;
    public float MonsterMoveSpeed;

    private Vector3[] _monsterWaypoints = new Vector3[3];
    private Vector3 _monsterAttackNode;

    private int[] _node1OrNode2 = new int[10];
    private int[] _node0OrNode1 = new int[10];

    private string _currentPosition = "Node1";

    private bool _isMoving = false;

    private int pickedNode;

    private float _wait;

    public bool isMoving
    {
        get { return _isMoving; }
    }

    void Awake()
    {
        _monsterAttackNode = GameObject.Find("EnemyController/MonsterWaypoints/AttackNode").GetComponent<Transform>().position;
        for (int i = 0; i< _monsterWaypoints.Length; i++)
        {
            _monsterWaypoints[i] = GameObject.Find("EnemyController/MonsterWaypoints/Node" + i).GetComponent<Transform>().position;
        }

        
    }

    void Start()
    {
        //fill _node0OrNode1[] with numbers
        for(int i = 0; i < ChanceToWait; i++)
        {
            _node0OrNode1[i] = 1;
        }
        for(int i = ChanceToWait; i < _node0OrNode1.Length; i++)
        {
            _node0OrNode1[i] = 0;
        }

        //fill _node1OrNode2[] with numbers
        for (int i = 0; i < ChanceToWait; i++)
        {
            _node1OrNode2[i] = 1;
        }
        for (int i = ChanceToWait; i < _node1OrNode2.Length; i++)
        {
            _node1OrNode2[i] = 2;
        }
    }


    public void ChooseWhichWaypointToGo()
    {
        if (_currentPosition.Equals("Node0"))
        {
            if (!isMoving)
            {
                pickedNode = _node1OrNode2[Random.Range(0, 10)];
            }

            if (pickedNode == 1)
            {
                MoveToNode1();
            }
            if (pickedNode == 2)
            {
                MoveToNode2();
            }
        }
        if (_currentPosition.Equals("Node2"))
        {
            if (!isMoving)
            {
                pickedNode = _node0OrNode1[Random.Range(0, 10)];
            }

            if (pickedNode == 0)
            {
                MoveToNode0();
            }
            if (pickedNode == 1)
            {
                MoveToNode1();
            }
        }

        if (_currentPosition.Equals("Node1"))
        {
            Timer();
            if(_wait >= WaitTime)
            {
                if (!isMoving)
                {
                    pickedNode = Random.Range(1, 3); //here choose between 0 or 2 (in this case 1 = 0 )
                }

                if (pickedNode == 1)
                {
                    MoveToNode0();
                }
                if (pickedNode == 2)
                {
                    MoveToNode2();
                }
            }
            
        }


    }

    void MoveToNode0()
    {
        transform.position = Vector3.MoveTowards(transform.position, _monsterWaypoints[0], MonsterMoveSpeed * Time.deltaTime);
        _isMoving = true;

        if(transform.position == _monsterWaypoints[0])
        {
            _currentPosition = "Node0";
            _isMoving = false;
            _wait = 0;              //reset timer for wait in node 1
        }
    }

    void MoveToNode1()
    {
        transform.position = Vector3.MoveTowards(transform.position, _monsterWaypoints[1], MonsterMoveSpeed * Time.deltaTime);
        _isMoving = true;

        if (transform.position == _monsterWaypoints[1])
        {
            _currentPosition = "Node1";
            _isMoving = false;
        }
    }

    void MoveToNode2()
    {
        transform.position = Vector3.MoveTowards(transform.position, _monsterWaypoints[2], MonsterMoveSpeed * Time.deltaTime);
        _isMoving = true;

        if (transform.position == _monsterWaypoints[2])
        {
            _currentPosition = "Node2";
            _isMoving = false;
            _wait = 0;              //reset timer for wait in node 1
        }
    }

    public void MoveToAttackNode()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _monsterAttackNode.z);
    }

    public void MoveBackToNode1()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _monsterWaypoints[1].z);
    }


    void Timer()
    {
        _wait += Time.deltaTime;
    }


}
