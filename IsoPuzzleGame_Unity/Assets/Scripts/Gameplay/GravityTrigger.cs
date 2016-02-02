using UnityEngine;
using System.Collections;

public class GravityTrigger : MonoBehaviour {

    public float GravityModifier;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        IEditable _editable = other.transform.parent.GetComponent<Editable>();

        if (_editable == null)
            return;

        _editable.GravityEdit(GravityModifier);
    }

    public void OnTriggerExit(Collider other)
    {
        IEditable _editable = other.transform.parent.GetComponent<Editable>();

        if (_editable == null)
            return;

        _editable.GravityEdit(-20f);
    }
}
