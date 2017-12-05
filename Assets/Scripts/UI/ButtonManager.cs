using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    private Canvas _canvasMainMenu;
    private Canvas _canvasLoading;
    private Canvas _canvasOptions;
    private Canvas _canvasChooseLevel;
    private AudioSource _audioSource;

    void Awake()
    {
        _canvasMainMenu = GameObject.Find("Canvas/Main Menu").GetComponent<Canvas>();
        _canvasLoading = GameObject.Find("Canvas/Loading").GetComponent<Canvas>();
        _canvasOptions = GameObject.Find("Canvas/Options").GetComponent<Canvas>();
        _canvasChooseLevel = GameObject.Find("Canvas/Choose Level").GetComponent<Canvas>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartGameButton(string startTheGame)
    {
        _audioSource.Play();
        _canvasMainMenu.enabled = false;
        _canvasChooseLevel.enabled = false;
        _canvasLoading.enabled = true;
        SceneManager.LoadScene(startTheGame);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void PushOptionsButton()
    {
        _audioSource.Play();
        _canvasMainMenu.enabled = !_canvasMainMenu.enabled;
        _canvasOptions.enabled = !_canvasOptions.enabled;
    }

    public void ChooseLevel()
    {
        _audioSource.Play();
        _canvasMainMenu.enabled = !_canvasMainMenu.enabled;
        _canvasChooseLevel.enabled = !_canvasChooseLevel.enabled;
    }
}
