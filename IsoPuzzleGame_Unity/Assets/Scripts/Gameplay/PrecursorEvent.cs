using UnityEngine;
using System.Collections;

public class PrecursorEvent : MonoBehaviour {

    [Header("Assign the Gameobject next used after this one")]
    [SerializeField] GameObject m_NextObjective;

    public void NextObjectiveReady()
    {
        if (m_NextObjective == null)
        {
            Debug.Log("Next Objective Not Assigned");
            return;
        }

        m_NextObjective.SendMessage("PrecursorComplete", SendMessageOptions.DontRequireReceiver);
    }
}
