using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IngameButtonManager : MonoBehaviour {

    //audio
    public AudioMixer audioMixer;
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    //ui
    private Canvas _canvasPaused;
    private Canvas _canvasUnpaused;
    private Canvas _canvasPausePanel;
    private Canvas _canvasOptionsPanel;
    private Canvas _canvasInfoPanel;
    private Canvas _canvasGameOverPanel;
    private Canvas _canvasVictoryPanel;

    //scripts
    private EnemyController _enemyControllerScript;

    private AudioSource _click;
    private bool _gameIsPaused = false;

    public bool GameIsPaused
    {
        get { return _gameIsPaused; }
    }

    void Awake()
    {
        //ui
        _canvasPaused = GameObject.Find("Canvas/Paused").GetComponent<Canvas>();
        _canvasUnpaused = GameObject.Find("Canvas/Unpaused").GetComponent<Canvas>();
        _canvasPausePanel = GameObject.Find("Canvas/Paused/Pause Panel").GetComponent<Canvas>();
        _canvasOptionsPanel = GameObject.Find("Canvas/Paused/Options Panel").GetComponent<Canvas>();
        _canvasInfoPanel = GameObject.Find("Canvas/Paused/Info Panel").GetComponent<Canvas>();
        _canvasGameOverPanel = GameObject.Find("Canvas/Paused/Game Over Panel").GetComponent<Canvas>();
        _canvasVictoryPanel = GameObject.Find("Canvas/Paused/Victory Panel").GetComponent<Canvas>();

        //scripts
        _enemyControllerScript = (EnemyController)GameObject.Find("EnemyController").GetComponent(typeof(EnemyController));


        _click = GameObject.Find("GameController/Sound/UI Clicks").GetComponent<AudioSource>();
    }
    

    public void PauseTheGame()
    {
        _click.Play();

        _gameIsPaused = !_gameIsPaused;

        _canvasPaused.enabled = !_canvasPaused.enabled;
        _canvasUnpaused.enabled = !_canvasUnpaused.enabled;
        _canvasPausePanel.enabled = !_canvasPausePanel.enabled;

        Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        Lowpass();
    }

    
    public void ChangeLevel(string startTheGame)
    {
        _click.Play();
        //reset all ui elements, sounds, and set timescale back to 1. otherwise the game starts in pause mode
        _canvasPaused.enabled = false;
        _canvasUnpaused.enabled = true;
        _canvasPausePanel.enabled = false;
        _canvasGameOverPanel.enabled = false;
        _canvasVictoryPanel.enabled = false;
        Time.timeScale = 1;
        Lowpass();

        //load main menu
        SceneManager.LoadScene(startTheGame);
    }

    public void PushOptionsButton()
    {
        _click.Play();

        _canvasPausePanel.enabled = !_canvasPausePanel.enabled;
        _canvasOptionsPanel.enabled = !_canvasOptionsPanel.enabled;
    }


    public void PushInfoButton()
    {
        _click.Play();

        _canvasOptionsPanel.enabled = !_canvasOptionsPanel.enabled;
        _canvasInfoPanel.enabled = !_canvasInfoPanel.enabled;
    }


    public void SetSfxLvl(float sfxLvl)
    {
        if (sfxLvl < -20)
        {
            sfxLvl = -80;

        }
        audioMixer.SetFloat("sfxVol", sfxLvl);
        audioMixer.SetFloat("duckMusic", sfxLvl * 2);

    }

    public void SetMusicLvl(float musicLvl)
    {
        if (musicLvl < -20)
        {
            musicLvl = -80;
        }
        audioMixer.SetFloat("musicVol", musicLvl);
    }

    public void GameOver()
    {
        _click.Play();

        _gameIsPaused = !_gameIsPaused;

        _canvasPaused.enabled = !_canvasPaused.enabled;
        _canvasUnpaused.enabled = !_canvasUnpaused.enabled;
        _canvasGameOverPanel.enabled = !_canvasGameOverPanel.enabled;

        Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        Lowpass();
    }

    public void restartCurrentScene()
    {
        _click.Play();
        int scene = SceneManager.GetActiveScene().buildIndex;

        //reset all ui elements, sounds, and set timescale back to 1. otherwise the game starts in pause mode
        _canvasPaused.enabled = false;
        _canvasUnpaused.enabled = true;
        _canvasPausePanel.enabled = false;
        _canvasGameOverPanel.enabled = false;
        Time.timeScale = 1;
        Lowpass();

        //restart scene
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void Victory()
    {
        PauseTheGame();
        _canvasPausePanel.enabled = !_canvasPausePanel.enabled;
        _canvasVictoryPanel.enabled = true;

    }

    void Lowpass()
    {
        if (Time.timeScale == 0)
        {
            paused.TransitionTo(.01f);
        }
        else
        {
            unpaused.TransitionTo(.01f);
        }
    }


}
