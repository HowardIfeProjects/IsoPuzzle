using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

    [Header("Assigned Gameobject")]
    public GameObject m_EventObject;
    [Header("Player Range from Object")]
    public float m_Range;
    [Header("Set to True if the event has no Precursor")]
    public bool IsPrecursorTriggered;

    private GameObject m_Player;
    private bool IsEventTriggered = false;

	// Use this for initialization
	void Start () {

        m_Player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {

        TrackPlayer();

	}

    private void TrackPlayer() {

        if (m_Player == null)
            return;

        if (Vector3.Distance(transform.position, m_Player.transform.position) < m_Range) {

            if (Input.GetKeyDown(KeyCode.R)) {
                SendMessageToObject();         
            }
        }
    }

    private void SendMessageToObject() {

        if (!IsEventTriggered && IsPrecursorTriggered) {

            if (m_EventObject == null) {
                Debug.Log("Event Object is Not Assigned/Destroyed On: " + gameObject.name);
                return;
            }

            m_EventObject.SendMessage("EventTrigger", SendMessageOptions.DontRequireReceiver);
            IsEventTriggered = true;
        }
        else
            Feedback();
    }

    private void Feedback() {

        //play some sort of visual effect to represent the event cant be triggered
    }
}
