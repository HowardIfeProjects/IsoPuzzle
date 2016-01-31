using UnityEngine;
using System.Collections;

public class GravityTrigger : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        IEditable _editable = other.transform.parent.GetComponent<Editable>();

        if (_editable == null)
            return;

        _editable.GravityEdit(20f);
    }

    public void OnTriggerExit(Collider other)
    {
        IEditable _editable = other.transform.parent.GetComponent<Editable>();

        if (_editable == null)
            return;

        _editable.GravityEdit(-20f);
    }
}
