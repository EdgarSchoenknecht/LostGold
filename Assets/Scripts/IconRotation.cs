using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotation : MonoBehaviour
{
    private Camera playersCamera;

    void Awake()
    {
        playersCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

	void Start ()
    {
        gameObject.transform.eulerAngles = new Vector3(playersCamera.transform.eulerAngles.x, playersCamera.transform.eulerAngles.y, playersCamera.transform.eulerAngles.z);
    }
}
