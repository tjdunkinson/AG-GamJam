using InControl;
using UnityEngine;

public partial class Player
{
    InputDevice _controller;
    private void Awake()
    {
        Controller.detectCollisions = true;
        InputManager.Setup();
        _controller = InputManager.ActiveDevice;
    }

    private void Update()
    {
        InputManager.Update();
        InputUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Gizmos.DrawRay(transform.position, Vector3.right);
        //Gizmos.DrawRay(transform.position, Vector3.left);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * GetWallClimbDist));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * GetWallClimbDist));
    }
}