using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skydomerotate : MonoBehaviour {

    public float Speed = 1f;
    public string ChooseAxis;
	void Update () {

        if (ChooseAxis.Equals("x"))
        {
            transform.Rotate(Vector3.right, Speed * Time.deltaTime);
        }
        else if (ChooseAxis.Equals("y"))
        {
            transform.Rotate(Vector3.up, Speed * Time.deltaTime);
        }
        else if (ChooseAxis.Equals("z"))
        {
            transform.Rotate(Vector3.forward, Speed * Time.deltaTime);
        }
        

    }
}
