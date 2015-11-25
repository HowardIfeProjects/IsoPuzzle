using UnityEngine;
using System.Collections;

public class PlayerInventoryController : MonoBehaviour {

    //CHANGE LATER
    //All these should be assigned to the gizmo childs placed under player, this gives the rays posistions
    public GameObject rayFrontMiddle;
    public GameObject rayFrontLeft;
    public GameObject rayFrontRight;

    public GameObject rayBackMiddle;
    public GameObject rayBackLeft;
    public GameObject rayBackRight;

    public GameObject rayLeftMiddle;
    public GameObject rayLeftLeft;
    public GameObject rayLeftRight;

    public GameObject rayRightMiddle;
    public GameObject rayRightLeft;
    public GameObject rayRightRight;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DebugLines();

        //wallStop = Physics2D.Linecast(this.transform.position, jumpRayEnd.position, 1 << LayerMask.NameToLayer("Wall"));

        //Physics.RaycastAll(this.transform.position, rayFrontMiddle.transform.position); //<< this should work for the meantime? check on rayFrontMiddle since that may stop it

    }

    void DebugLines()
    {
        Debug.DrawLine(this.transform.position, rayFrontMiddle.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayFrontLeft.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayFrontRight.transform.position, Color.white);

        Debug.DrawLine(this.transform.position, rayBackMiddle.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayBackLeft.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayBackRight.transform.position, Color.white);

        Debug.DrawLine(this.transform.position, rayLeftMiddle.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayLeftLeft.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayLeftRight.transform.position, Color.white);

        Debug.DrawLine(this.transform.position, rayRightMiddle.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayRightLeft.transform.position, Color.white);
        Debug.DrawLine(this.transform.position, rayRightRight.transform.position, Color.white);
    }
}
