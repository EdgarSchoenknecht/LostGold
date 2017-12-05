using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonOutline : MonoBehaviour {

    public Material NormalMaterial;
    public Material OutlineMaterial;

    private Renderer _renderer;
    private DragAndDrop _dragAndDropScript;

    void Awake()
    {
        _renderer = GameObject.Find("Cannon/Cannon_1_6/CannonHighPoly").GetComponent<Renderer>();
        _dragAndDropScript = (DragAndDrop)GameObject.Find("GameController").GetComponent(typeof(DragAndDrop));
    }

    void Update()
    {
        if(_dragAndDropScript.IsDragging)
        {
            _renderer.material = OutlineMaterial;
        }
        else
        {
            _renderer.material = NormalMaterial;
        }


    }


}
