using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void Awake()
    {
        Controller.detectCollisions = true;
    }

    private void Update()
    {
        InputUpdate();
    }

    //// keep track of all collideables we encounter on collision

    //private float _lastHitTime;
    //private Queue<GameObject> _collidedObjects = new Queue<GameObject>();

    //private void
    //OnControllerColliderHit(ControllerColliderHit other)
    //{
    //    // each frame
    //    if (Time.time != _lastHitTime)
    //    {
    //        // reset 
    //        _lastHitTime = Time.time;
    //        _collidedObjects.Clear();
    //    }

    //    _collidedObjects.Enqueue(other.collider.gameObject);
    //}
}