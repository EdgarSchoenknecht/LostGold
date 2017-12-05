using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //external scripts and gameobjects
    private GameObject _cannon;
    private ShootCannon _shootCannonScript;
    private GameObject _enemyController;
    private EnemyController _enemyControllerScript;
    private IngameButtonManager _ingameButtonManagerScript;
    private ResultSpawnAnimEvents _resultSpawnAnimEventsScript;

    //internal scripts of the GameController
    private Calculate _calculateScript;
    private SpawnCrates _spawnCratesScript;
    private DragAndDrop _dragandDropScript;
    private PlayerController _playerControllerScript;

    private float _wait = 0f;
    private bool _disableDragAndDrop = false;
    private bool _playIfOnce = true;
    private bool _playSoundOnce = true;
    private bool _playAnimOnce = true;

    //sound
    private AudioSource _audioSourceCalculate;
    private AudioClip[] _loadTooMuch;
    private AudioClip[] _loadTooLittle;

    //animation
    private Animator _animatorCannon;
    private Animator _animatorOperators;

    void Awake ()
    {
        //get the external gameobject and scripts
        _cannon = GameObject.Find("Cannon");
        _shootCannonScript = (ShootCannon)_cannon.GetComponent(typeof(ShootCannon));
        _enemyController = GameObject.Find("EnemyController");
        _enemyControllerScript = (EnemyController)_enemyController.GetComponent(typeof(EnemyController));
        _ingameButtonManagerScript = (IngameButtonManager)GameObject.Find("UI Button Controller").GetComponent(typeof(IngameButtonManager));
        _resultSpawnAnimEventsScript = (ResultSpawnAnimEvents)GameObject.Find("Operators/ResultSpawn").GetComponent(typeof(ResultSpawnAnimEvents));


        //get internal scripts of the GameController
        _calculateScript = GetComponent<Calculate>();
        _spawnCratesScript = GetComponent<SpawnCrates>();
        _dragandDropScript = GetComponent<DragAndDrop>();
        _playerControllerScript = GetComponent<PlayerController>();

        //sound
        _audioSourceCalculate = GameObject.Find("GameController/Sound/Calculate").GetComponent<AudioSource>();
        _loadTooMuch = GameObject.Find("GameController/Sound/Calculate/Result Too High").GetComponent<SoundClips>().AudioClipsArray;
        _loadTooLittle = GameObject.Find("GameController/Sound/Calculate/Result Too Low").GetComponent<SoundClips>().AudioClipsArray;

        //animation
        _animatorCannon = GameObject.Find("Cannon").GetComponent<Animator>();
        _animatorOperators = GameObject.Find("Operators").GetComponent<Animator>();
    }

    void Start()
    {
        StartTheGame();
        _playIfOnce = true;
    }
	
	
	void Update ()
    {
        //------------------Debug Input----------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shootCannonScript.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            RaycastHit hitInfo;
            GameObject blub = _dragandDropScript.ReturnClickedObject(out hitInfo);
            Debug.Log(blub);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            _playerControllerScript.PlayerHealth -= 1;
        }

        //---------------------End------------------------------------


        //-----------------Player Inputs------------------------------
        if (_ingameButtonManagerScript.GameIsPaused == false)
        {
            //mouse button down
            if (Input.GetMouseButtonDown(0) && !_disableDragAndDrop)
            {
                _dragandDropScript.ClickDragAndDropButton();
            }
            //mouse button up
            if (Input.GetMouseButtonUp(0))
            {
                _dragandDropScript.ReleaseDragAndDropButton();
            }

        }


        //----------------------End-------------------------------



        //Player correct
        if (_calculateScript.TargetResult == _calculateScript.ListOfOperands.Sum())
        {
            ResultIsCorrect();
        }

        //Game Over
        if(_playerControllerScript.PlayerHealth <= 0)
        {
            if (_playIfOnce)
            {
                _ingameButtonManagerScript.GameOver();
                _playIfOnce = false;
            }

        }

        //Player victorious
        if(_enemyControllerScript.PlayerVictorious == true)
        {

        }

        //load too much
        if (_calculateScript.ListOfOperands.Count() == 2 && _calculateScript.ListOfOperands.Sum() > _calculateScript.TargetResult)
        {
            if(_playSoundOnce)
            {
                SoundController.RandomizeSfx(.95f, 1.05f, _audioSourceCalculate, _loadTooMuch);
                _playSoundOnce = false;
            }
            
            ResultIsWrong();
        }

        //load too little
        if (_calculateScript.ListOfOperands.Count() == 2 && _calculateScript.ListOfOperands.Sum() < _calculateScript.TargetResult)
        {
            if (_playSoundOnce)
            {
                SoundController.RandomizeSfx(.95f, 1.05f, _audioSourceCalculate, _loadTooLittle);
                _playSoundOnce = false;
            }
            
            ResultIsWrong();
        }

    }

    void ResultIsCorrect()
    {
        _disableDragAndDrop = true;

        if(_playAnimOnce)
        {
            _spawnCratesScript.CleanSpawn();
            _calculateScript.DeactivateOldResult();
            _animatorOperators.SetBool("Show Result", true);
            _playAnimOnce = false;
        }

        RaycastHit hitInfo;
        if (_playIfOnce && Input.GetMouseButtonDown(0) && _dragandDropScript.ReturnClickedObject(out hitInfo).CompareTag("Canon") && _resultSpawnAnimEventsScript.CannonIsLoaded)
        {
            _shootCannonScript.Shoot();
            _playIfOnce = false;
            _resultSpawnAnimEventsScript.CannonIsLoaded = false;
        }
        
        if(!_playIfOnce)
        {
            Invoke("ResetAfterCorrectResult", 3);
        }
    }

    void ResetAfterCorrectResult()
    {
        //reset all values
        _calculateScript.ListOfOperands.Clear();
        _calculateScript.RandomTargetResult();

        //spawn new operands
        _spawnCratesScript.SpawnOperand();
        _spawnCratesScript.SpawnRandomCrates();

        _playIfOnce = true;
        _playAnimOnce = true;
        _disableDragAndDrop = false;
        _animatorOperators.SetBool("Show Result", false);
        CancelInvoke("ResetAfterCorrectResult");
    }

    void ResultIsWrong()
    {
        if(_playIfOnce)
        {
            _shootCannonScript.Misfire();
            _playIfOnce = false;
        }

        if (!_playIfOnce)
        {
            Invoke("ResetAfterWrongResult", 3);
        }
    }

    void ResetAfterWrongResult()
    {
        //reset all values
        _calculateScript.ListOfOperands.Clear();
        _calculateScript.RandomTargetResult();

        //delete all crates
        _spawnCratesScript.CleanSpawn();
        _spawnCratesScript.CleanEquationSpawn();

        //spawn new operands
        _spawnCratesScript.SpawnOperand();
        _spawnCratesScript.SpawnRandomCrates();
        _playIfOnce = true;
        _playSoundOnce = true;
        CancelInvoke("ResetAfterWrongResult");
    }

    void StartTheGame()
    {
        //spawn new operands
        _spawnCratesScript.SpawnOperand();
        _spawnCratesScript.SpawnRandomCrates();

        //spawn enemy
        _enemyControllerScript.SpawnEnemy();
    }

    void GameOver()
    {

        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }


    void Timer()
    {
        _wait += Time.deltaTime;
    }

}
