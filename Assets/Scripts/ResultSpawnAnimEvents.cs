using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpawnAnimEvents : MonoBehaviour {

    private GameObject _results;
    private bool _doItOnce = true;
    private Animator _animator;
    private Animator _animatorCannon;

    private bool _cannonIsLoaded;
    public bool CannonIsLoaded
    {
        get { return _cannonIsLoaded; }
        set { _cannonIsLoaded = value; }
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animatorCannon = GameObject.Find("Cannon").GetComponent<Animator>();
    }

    void Update()
    {
        if(_doItOnce && transform.childCount > 0)
        {
            _results = transform.GetChild(0).gameObject;
            Invoke("PlayFlyAnimation", 2);
            _doItOnce = false;
        }
    }


    void DestroyResult()
    {
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            _doItOnce = true;
        }
    }

    void PlayReloadAnimation()
    {
        _animatorCannon.SetBool("Reload Cannon", true);
        _cannonIsLoaded = true;
    }

    void PlayFlyAnimation()
    {
        _animator.SetBool("Fly", true);
        CancelInvoke("PlayFlyAnimation");
    }

    void FlyReset()
    {
        _animator.SetBool("Fly", false);
    }
}
