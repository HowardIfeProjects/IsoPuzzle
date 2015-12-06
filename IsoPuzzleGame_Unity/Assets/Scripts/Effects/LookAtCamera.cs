using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Update is called once per frame
	private void Update () {

        if (Camera.main == null)
            return;

        Vector3 _relativePos = Camera.main.transform.position - transform.position;
        Quaternion _rotation = Quaternion.LookRotation(-_relativePos);
        transform.rotation = _rotation;

    }
}
