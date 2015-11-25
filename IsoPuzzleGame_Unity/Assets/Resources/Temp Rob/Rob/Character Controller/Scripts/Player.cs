using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]//When this script is added to a gameObject, the script "PlayerController" will also be added to the gameObject 
public class Player : MonoBehaviour
{
    public float moveSpeed = 5;

    Camera viewCamera;
    PlayerController controller;

	void Start ()
    {
        controller = GetComponent<PlayerController>();//The RequireComponent line will make sure that there will be no error with this line
        viewCamera = Camera.main;
	}
	
	void Update ()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed; //(from: http://docs.unity3d.com/ScriptReference/Vector3.Normalize.html) "When normalized, a vector keeps the same direction but its length is 1.0."
        controller.Move(moveVelocity);//the value for moveVelocity is being pushed into "PlayerController" script, this value will be "_velocity" in that script

        //Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);//creates a ray from the camera to the posistion of the mouse cursor on the screen 
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

       // if(groundPlane.Raycast(ray,out rayDistance))//11.00
       // {
          //  Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);

         //   controller.LookAt(point);
        //}
    }
}