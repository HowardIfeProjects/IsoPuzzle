using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Player))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidBody;

	void Start ()
    {
        myRigidBody = GetComponent <Rigidbody> ();
      //  Debug.Log("This is working");
	}

    public void Move(Vector3 _velocity)//This value will be the one pushed from "Player" in the function "Update"
    {
        velocity = _velocity;//the velocity should equals 5 in whatever direction the player presses 
       // Debug.Log(_velocity);
    }

    public void FixedUpdate()
    {
        myRigidBody.MovePosition(myRigidBody.position + velocity * Time.fixedDeltaTime);
      //  Debug.Log("RigidBody is " + myRigidBody.position);
    }

    public void LookAt(Vector3 lookPoint)//11.00
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}