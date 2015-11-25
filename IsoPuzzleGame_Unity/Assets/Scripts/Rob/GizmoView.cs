using UnityEngine;
using System.Collections;

public class GizmoView : MonoBehaviour {

    public float gizmoRadius = 1f;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);//draws a sphere in the scene view that is in the same posistion as the gameObejct and has a circular radius on 1
    }
}
