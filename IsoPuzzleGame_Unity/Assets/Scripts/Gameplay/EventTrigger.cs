using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

    public enum TypeOfTrigger { ButtonPress, Distance, OnCollisonEnter, OnCollisionExit, OnCollisionStay, OnTriggerEnter, OnTriggerExit, OnTriggerStay }
    [Header("How is the Trigger Set Off")]
    public TypeOfTrigger m_TypeOfTrigger = new TypeOfTrigger();

    [Header("Assigned Gameobject")]
    public GameObject m_EventObject;
    [Header("Objective Number Dependency")]
    public int[] m_ObjectiveDependency;
    [Header("Player Range from Object")]
    public float m_Range;
    [Header("Set to True if the event has no Precursor")]
    public bool IsPrecursorTriggered;
    [Header("Is This a Mission Objective")]
    public bool IsMissionObjective;
    [Header("Is This Triggered Via a Button Press")]
    public bool IsTriggeredByButton;

    private GameObject m_Player;
    [Header("Serialized For Debugging Purposes")]
    [SerializeField] bool IsEventTriggered = false;
    [SerializeField] bool HasObjectiveBeenUpdated = false;


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

            if (IsTriggeredByButton && Input.GetKeyDown(KeyCode.R)) {
                SendMessageToObject();
            }
            else if(!IsTriggeredByButton)
                SendMessageToObject();
        }
    }

    private void SendMessageToObject() {

        if (IsMissionObjective)
        {
            if (!IsEventTriggered && IsPrecursorTriggered)
            {
                LevelData _levelData = LevelManager.Init_ReturnLevelData();

                for (int i = 0; i < m_ObjectiveDependency.Length; i++)
                {
                    //Debug.Log("Lev Data: " + _levelData._CurObjectiveIndex + "     Object i: " + m_ObjectiveDependency[i]);
                    if (_levelData._CurObjectiveIndex == m_ObjectiveDependency[i])
                    {
                        if (m_EventObject == null)
                        {
                           // Debug.Log("Event Object is Not Assigned/Destroyed On: " + gameObject.name);
                            return;
                        }

                        //Debug.Log("Sending Message to Event Object");
                        m_EventObject.SendMessage("EventTriggered", SendMessageOptions.DontRequireReceiver);

                        if (!HasObjectiveBeenUpdated)
                        {
                            LevelManager.Init_LoadNextObjective();
                            HasObjectiveBeenUpdated = true;
                        }

                        IsEventTriggered = true;
                        return;
                    }
                }
               
                //if loop finishes
               // Debug.Log("Objective Not Met");
            }
            else
            {
                Feedback();

                if (!HasObjectiveBeenUpdated)
                {
                    LevelManager.Init_LoadNextObjective();
                    HasObjectiveBeenUpdated = true;
                }
            }
        }
        else
        {
            if (m_EventObject == null)
            {
                //Debug.Log("Event Object is Not Assigned/Destroyed On: " + gameObject.name);
                return;
            }

            m_EventObject.SendMessage("EventTrigger", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Feedback() {

        //play some sort of visual effect to represent the event cant be triggered
    }

    public void EventTriggered()
    {
        IsPrecursorTriggered = true;
    }


    //Unity Collider Stuff
    private void OnCollisionEnter(Collision _col)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnCollisonEnter)
            return;
    }

    private void OnCollisionStay(Collision _col)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnCollisionStay)
            return;
    }

    private void OnCollisionExit(Collision _col)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnCollisionExit)
            return;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnTriggerEnter)
            return;
    }

    private void OnTriggerExit(Collider _other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnTriggerExit)
            return;
    }

    private void OnTriggerStay(Collider _other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnTriggerStay)
            return;
    }
}
