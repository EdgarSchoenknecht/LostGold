using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int EnemyHealth;
    public int EnemyDealDamage;

    private GameObject _gameController;
    public GameObject KrakenTentaclePrefab;
    private PlayerController _playerControllerScript;
    private EnemyController _enemyControllerScript;
    private Vector3 _moveToSpawnPosition;
    private Pathfinder _pathfinderScript;
    private Animator _animator;
    private Vector3 _krakenTentacleSpawn;

    private Vector3 _lastPos;
    private Vector3 _offset;
    private float _threshold = 0.0f;

    private bool _enemyIsSpawned = false;
    private bool _enemyIsAttacking = false;

    public float TimeUntilAttack;
    private float _attackTime;
    private Slider _attackBarSlider;

    //sound
    private AudioSource _audioSource;
    private AudioClip[] _audioClipsArrayGetHit;
    private AudioSource _audioSourcePlayer;
    private AudioClip[] _audioClipsArrayPlayerGetHit;

    //variables for EnemyIsDead()
    private int _iHelp = 1;
    private float _iOld = 0;

    public bool EnemyIsSpawned
    {
        get { return _enemyIsSpawned; }
    }

    void Awake ()
    {
        _animator = GetComponent<Animator>();
        _pathfinderScript = GetComponent<Pathfinder>();
        _gameController = GameObject.Find("GameController");
        _playerControllerScript = (PlayerController)_gameController.GetComponent(typeof(PlayerController));
        _moveToSpawnPosition = GameObject.Find("EnemyController/MonsterWaypoints/Node1").GetComponent<Transform>().position;
        _attackBarSlider = GameObject.Find("Canvas/Unpaused/Monster Attack Bar/Slider").GetComponent<Slider>();
        _enemyControllerScript = (EnemyController)GameObject.Find("EnemyController").GetComponent(typeof(EnemyController));
        _krakenTentacleSpawn = GameObject.Find("EnemyController/MonsterWaypoints/Kraken Tentacle Spawn").GetComponent<Transform>().position;

        //sound
        _audioSource = GameObject.Find("GameController/Sound/Monster").GetComponent<AudioSource>();
        _audioClipsArrayGetHit = GameObject.Find("GameController/Sound/Monster/Get Hit").GetComponent<SoundClips>().AudioClipsArray;
        _audioSourcePlayer = GameObject.Find("GameController/Sound/Player").GetComponent<AudioSource>();
        _audioClipsArrayPlayerGetHit = GameObject.Find("GameController/Sound/Player/Get Hit").GetComponent<SoundClips>().AudioClipsArray;
    }
	
    void Start()
    {
        _lastPos = transform.position;
    }

	void Update ()
    {
        //controls for the monster -------------------------------------------
        if(_enemyControllerScript.WhatShouldSpawn.Equals("Monster"))
        {
            //monster is dead
            if (EnemyHealth <= 0)
            {
                EnemyIsDead();
                _enemyControllerScript.PlayerVictorious = true;
            }

            //check if monster is walking (used for animation)
            if (_enemyIsSpawned && (_enemyIsAttacking == false) && EnemyHealth > 0)
            {
                _offset = transform.position - _lastPos;
                if (_offset.x > _threshold)
                {
                    _lastPos = transform.position;
                    _animator.SetBool("Swims Left", false);
                    _animator.SetBool("Swims Right", true);

                }
                else if (_offset.x < _threshold)
                {
                    _lastPos = transform.position;
                    _animator.SetBool("Swims Right", false);
                    _animator.SetBool("Swims Left", true);
                }
                else
                {
                    _animator.SetBool("Swims Right", false);
                    _animator.SetBool("Swims Left", false);
                }
            }

            //monster begins with his movement
            if (_enemyIsSpawned && !_enemyIsAttacking && EnemyHealth > 0)
            {
                _pathfinderScript.ChooseWhichWaypointToGo();
            }

            //monster move to start position after it spawned
            if (!_enemyIsSpawned && !_enemyIsAttacking && EnemyHealth > 0)
            {
                MoveToSpawnPosition();
            }

            //monster attack
            if (_enemyIsSpawned && EnemyHealth > 0)
            {
                if (_attackTime < TimeUntilAttack)
                {
                    StartAttackTimer();
                    FillAttackBar();
                }
                else if (_attackTime >= TimeUntilAttack)
                {
                    _animator.SetBool("isAttacking", true);
                    _enemyIsAttacking = true;
                    _animator.SetBool("Attacks", true);
                }
            }
        }


        //controls for the eel ---------------------------------------
        if (_enemyControllerScript.WhatShouldSpawn.Equals("Eel"))
        {
            //monster is dead
            if (EnemyHealth <= 0)
            {
                EnemyIsDead();
                _enemyControllerScript.PlayerVictorious = true;
            }

            //monster attack
            if (_enemyIsSpawned && EnemyHealth > 0)
            {
                if (_attackTime < TimeUntilAttack)
                {
                    StartAttackTimer();
                    FillAttackBar();
                }
                else if (_attackTime >= TimeUntilAttack)
                {
                    _enemyIsAttacking = true;
                    _animator.SetBool("Attacks", true);
                }
            }

        }

        //controls for the kraken ---------------------------------------
        if (_enemyControllerScript.WhatShouldSpawn.Equals("Kraken"))
        {
            //monster is dead
            if (EnemyHealth <= 0)
            {
                EnemyIsDead();
                _enemyControllerScript.PlayerVictorious = true;
            }

            //monster attack
            if (_enemyIsSpawned && EnemyHealth > 0)
            {
                if (_attackTime < TimeUntilAttack)
                {
                    StartAttackTimer();
                    FillAttackBar();
                }
                else if (_attackTime >= TimeUntilAttack)
                {
                    _enemyIsAttacking = true;
                    _animator.SetBool("Attacks", true);
                }
            }

        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannonball") && (EnemyHealth > 0) && (_enemyIsAttacking == false))
        {
            _animator.SetTrigger("Get Hit");
            SoundController.RandomizeSfx(.95f, 1.05f, _audioSource, _audioClipsArrayGetHit);

            EnemyHealth -= _playerControllerScript.PlayerDealDamage;
            Debug.Log("Enemy HP: "+EnemyHealth);
        }
        if (other.CompareTag("KrakenShield") && (EnemyHealth > 0) && (_enemyIsAttacking == false))
        {
            SoundController.RandomizeSfx(.95f, 1.05f, _audioSource, _audioClipsArrayGetHit);
            Debug.Log("shield gets hit");
        }

    }

    void MoveToSpawnPosition()
    {
        if (transform.position != _moveToSpawnPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveToSpawnPosition, 6 * Time.deltaTime);
            _animator.SetBool("Swims Up", true);

            if (transform.position == _moveToSpawnPosition)
            {
                _enemyIsSpawned = true;
                _animator.SetBool("Swims Up", false);
            }
        }
    }

    void EnemyIsDead()
    {
        _animator.SetTrigger("Die");
        Destroy(gameObject, 6.0f);

        //disable all hitboxes so it cant be hit anymore
        Collider[] hitBoxes = gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < hitBoxes.Length; i++)
        {
            hitBoxes[i].enabled = false;
        }
        

        //enemy float left after he dies
        Vector3 vec3Left = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, vec3Left, 3 * Time.deltaTime);

        //enemy floats up and down after he dies
        if (_iHelp >= 0 && _iHelp < 100)
        {
            _iHelp++;
        }
        else if (_iHelp >= 100)
        {
            _iHelp = -1;
        }
        else if(_iHelp <= -1 && _iHelp > -100)
        {
            _iHelp--;
        }
        else if (_iHelp <= -100)
        {
            _iHelp = 1;
        }
        
        Vector3 vec3UpAndDown = new Vector3(transform.position.x, transform.position.y + (((float)_iHelp / 100)-_iOld), transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, vec3UpAndDown, 3 * Time.deltaTime);
        _iOld = ((float)_iHelp / 100);

        //reset attack bar
        _attackBarSlider.value = 0;

    }

    void StartAttackTimer()
    {
        _attackTime += Time.deltaTime;
    }

    void FillAttackBar()
    {
        _attackBarSlider.maxValue = TimeUntilAttack;
        _attackBarSlider.value = _attackTime;
    }

    //Monster anim events-------------------------------
    public void AnimEventMoveMonsterToAttackNode()
    {
        _pathfinderScript.MoveToAttackNode();
    }

    public void AnimEventMoveMonsterBackToNode1()
    {
        _pathfinderScript.MoveBackToNode1();
    }
    public void AnimEventMonsterAttackEnds()
    {
        print("Attack ends");
        _animator.SetBool("Attacks", false);
        _attackTime = 0f;
        _enemyIsAttacking = false;
        _animator.SetBool("isAttacking", false);

    }
    //eel anim events-------------------------------
    public void AnimEventEelSpawned()
    {
        _enemyIsSpawned = true;
    }
    public void AnimEventEelAttackEnds()
    {
        _animator.SetBool("Attacks", false);
        _attackTime = 0f;
        _enemyIsAttacking = false;
    }
    //kraken anim events-------------------------------
    public void AnimEventKrakenSpawned()
    {
        _enemyIsSpawned = true;
    }
    public void AnimEventKrakenAttack()
    {
        GameObject Temporary_Spawn_Handler;
        Temporary_Spawn_Handler = Instantiate(KrakenTentaclePrefab, _krakenTentacleSpawn, gameObject.transform.rotation) as GameObject;
        Temporary_Spawn_Handler.transform.Rotate(Vector3.down * 0);
        Temporary_Spawn_Handler.transform.parent = gameObject.transform;
        Destroy(Temporary_Spawn_Handler, 3.0f);
    }
    public void AnimEventKrakenAttackEnds()
    {
        _animator.SetBool("Attacks", false);
        _attackTime = 0f;
        _enemyIsAttacking = false;
    }
    //anim events------------------------------------
    public void AnimEventAttackThePlayer()
    {
        SoundController.RandomizeSfx(.95f, 1.05f, _audioSourcePlayer, _audioClipsArrayPlayerGetHit);
        _playerControllerScript.PlayerHealth -= EnemyDealDamage;
    }
}
