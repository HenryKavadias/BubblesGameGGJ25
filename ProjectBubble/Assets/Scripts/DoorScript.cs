using System;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Transform _door, _door2;
    private bool _closeDoor;
    [SerializeField] private float _doorCloseSpeed = 512,_doorMoveSpeed = 5;

    [SerializeField] private Vector3 newRot, newPos, startRot;
    private Vector3 startPos,startPos2;

    private void Start()
    {
        startPos = _door.transform.position;
        startRot = _door.transform.eulerAngles;
        if (!_door2)
            return;
        startPos2  = _door2.transform.position;
    }

    public void CloseDoor()
    {
        _closeDoor = true;
    }

    private void Update()
    {
        if (!_closeDoor) return;
        _door.position =Vector3.MoveTowards(_door.position, startPos+(newPos), Time.deltaTime*_doorMoveSpeed);
        
        _door.rotation = Quaternion.RotateTowards(_door.rotation, Quaternion.Euler(startRot+newRot), Time.deltaTime*_doorCloseSpeed);

        if (!_door2)
            return;
        _door2.rotation = Quaternion.RotateTowards(_door2.rotation, Quaternion.Euler(-(startRot+newRot)), Time.deltaTime*_doorCloseSpeed);
        _door2.position =Vector3.MoveTowards(_door2.position, startPos2-(newPos), Time.deltaTime*_doorMoveSpeed);
    }
}
