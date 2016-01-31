using UnityEngine;
using System.Collections;

public class Editable : MonoBehaviour, IEditable {

    public void GravityEdit(float gVal)
    {
        com.ootii.Actors.ActorController _controller = GetComponent<com.ootii.Actors.ActorController>();
        _controller.Gravity = new Vector3(0f, gVal,0f);
    }
}
