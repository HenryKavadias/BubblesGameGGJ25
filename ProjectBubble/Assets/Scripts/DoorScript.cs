using System;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Transform _door, _door2;
    private bool _closeDoor;
    [SerializeField] private float _doorCloseSpeed = 512;
    public void CloseDoor()
    {
        _closeDoor = true;
    }

    private void Update()
    {
        if (!_closeDoor) return;
        _door.rotation = Quaternion.RotateTowards(_door.rotation, Quaternion.Euler(new Vector3(0,0,0)), Time.deltaTime*_doorCloseSpeed);
        _door2.rotation = Quaternion.RotateTowards(_door2.rotation, Quaternion.Euler(new Vector3(0,0,0)), Time.deltaTime*_doorCloseSpeed);
    }
}
