using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private GameObject _target;
    private GameObject _targetToMove;
    private Calculate _Calculate;
    private bool _isMouseDrag;
    private Vector3 _screenPosition;
    private Vector3 _offset;
    private int _equationPositionTrigger = 0;
    private GameObject _dragAndDropPos;

    private GameObject _equationLeft;
    private GameObject _equationRight;
    private GameObject _equation1;
    private GameObject _equation2;

    //sound
    private AudioSource _cannonAudioSource;
    private AudioClip[] _audioClipsLoadCannon1;
    private AudioClip[] _audioClipsLoadCannon2;
    

    public int equationPositionTrigger
    {
        get { return _equationPositionTrigger; }
        set { _equationPositionTrigger = value; }
    }

    public bool IsDragging
    {
        get { return _isMouseDrag; }
    }


    // saves origin position of the gameObject with compared tag
    private Vector3 _resetTargetToMovePosition;

    void Awake()
    {
        _Calculate = GetComponent<Calculate>();

        //equation positions
        _equationLeft = GameObject.Find("Operators/EquationPositionLeft");
        _equationRight = GameObject.Find("Operators/EquationPositionRight");

        //
        _dragAndDropPos = GameObject.Find("Operators/DragNDrop Position");
        
        //sound
        _cannonAudioSource = GameObject.Find("GameController/Sound/Cannon").GetComponent<AudioSource>();
        _audioClipsLoadCannon1 = GameObject.Find("GameController/Sound/Cannon/Load First").GetComponent<SoundClips>().AudioClipsArray;
        _audioClipsLoadCannon2 = GameObject.Find("GameController/Sound/Cannon/Load Second").GetComponent<SoundClips>().AudioClipsArray;
    }

    void Update()
    {


        if (_isMouseDrag)
        {
            
            //track mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + _offset;

            //It will update target gameobject's current postion.
            if(_target != null)
            {
            _target.transform.position = currentPosition;
            }
        }

    }

    public void ClickDragAndDropButton()
    {
        RaycastHit hitInfo;
        _target = ReturnClickedObject(out hitInfo);


        if (_target != null)
        {
            if(_target.CompareTag("Operator") || _target.CompareTag("Equation"))
            {
                _isMouseDrag = true;


                //get gameobject what compared with right tag
                _targetToMove = _target;

                //disable boxcollider to not block raycast
                _targetToMove.GetComponent<BoxCollider>().enabled = false;

                //Convert world position to screen position.
                _screenPosition = Camera.main.WorldToScreenPoint(_dragAndDropPos.transform.position);
                _offset = _dragAndDropPos.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPosition.z));

                //save the origin position to reset it after you drop it somewhere else
                _resetTargetToMovePosition = _target.transform.position;
            }
            
        }
    }

    public void ReleaseDragAndDropButton()
    {
        _isMouseDrag = false;

        RaycastHit hitInfo;
        _target = ReturnClickedObject(out hitInfo);
        //player DOESNT put object in the canon and object will be moved to his origin
        if (_targetToMove != null)
        {
            _targetToMove.transform.position = _resetTargetToMovePosition;
            _targetToMove.GetComponent<BoxCollider>().enabled = true;
        }

        //player put object in canon and object will be moved to the equation position
        if (_target != null && _targetToMove != null && _targetToMove.tag != "Equation")
        {
            if(_targetToMove.CompareTag("Operator") && _target.CompareTag("Canon"))
            {
                _Calculate.ListOfOperands.Add(_targetToMove.GetComponent<Operand>().OperandValue);
                _targetToMove.tag = "Equation";
                

                if (_equationPositionTrigger == 0)
                {
                    _targetToMove.transform.position = _equationLeft.transform.position;
                    _equation1 = _targetToMove;
                    SoundController.RandomizeSfx(.95f, 1.05f, _cannonAudioSource, _audioClipsLoadCannon1);
                    _equationPositionTrigger = 1;
                }
                else if (_equationPositionTrigger == 1)
                {
                    _targetToMove.transform.position = _equationRight.transform.position;
                    _equation2 = _targetToMove;
                    SoundController.RandomizeSfx(.95f, 1.05f, _cannonAudioSource, _audioClipsLoadCannon2);
                    _equation1.transform.parent = _equationLeft.transform;
                    _equation2.transform.parent = _equationRight.transform;
                }
                else
                {
                    Debug.Log("equation position error");
                }
                
            }

            _target = null;
        }

        //put equation back to the old place
        if (_target != null && _targetToMove != null)
        {
            if(_target.CompareTag("OperandReset"))
            {
                _targetToMove.tag = "Operator";
                _equationPositionTrigger = 0;
                _Calculate.ListOfOperands.Clear();
                _targetToMove.transform.localPosition = new Vector3(0, 0, 0); //move gameobject back to the parent position
                _targetToMove.GetComponent<BoxCollider>().enabled = true;
            }

        }

            _targetToMove = null;
    }

    public GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;

        }
        return target;
    }



}
