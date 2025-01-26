using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomerShooter
{
    public class RotateItem : MonoBehaviour
    {
        [SerializeField] Vector3 dropDirection = Vector3.up;
        [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
        public float RotSpeed = 1;
        public bool Reverse = false;
        private float drop = 0;
        public float DropSpeed = 0.1f;
        public float maxDrop = 1;
        private bool dropReverse = false;
        public Space Orientation = Space.World;
        
        // Use this for initialization
        void Start()
        {
            if (Reverse)
            {
                RotSpeed = -RotSpeed;
            }
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Rotate(direction, RotSpeed, Orientation);
            if (drop >= maxDrop)
            {
                dropReverse = false;
            }
            if (drop <= -maxDrop)
            {
                dropReverse = true;
            }
            if (dropReverse == false)
            {
                drop -= DropSpeed;
            }
            else
            {
                drop += DropSpeed;
            }

            transform.Translate(dropDirection * (drop * Time.fixedDeltaTime), Orientation);
        }
    }
}