using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Sprite[] HealthSprites;

    private Image _healthHUD;
    private PlayerController _playerControllerScript;

    void Awake()
    {
        _playerControllerScript = GetComponent<PlayerController>();
        _healthHUD = GameObject.Find("Canvas/Unpaused/Player Health/Image").GetComponent<Image>();
    }

    void Start()
    {
        _healthHUD.enabled = true;
    }

    void Update()
    {
        if (_playerControllerScript.PlayerHealth <= 0)
        {
            _healthHUD.enabled = false;
        }

        _healthHUD.sprite = HealthSprites[_playerControllerScript.PlayerHealth];

    }
}
