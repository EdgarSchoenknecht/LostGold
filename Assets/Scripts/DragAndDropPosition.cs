using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropPosition : MonoBehaviour {

    private Vector3 _screenPosition;
    private Vector3 screenPosition;

    void Update ()
    {

        screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        

        //track mouse position.
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

        //convert screen position to world position with offset changes.
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace);

        //It will update target gameobject's current postion.
        transform.position = currentPosition;
        

    }
}
