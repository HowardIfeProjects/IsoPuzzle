using UnityEngine;
using System.Collections;

public class NotificationTrigger : MonoBehaviour {

    public enum TypeOfTrigger { OnTriggerEnter, OnTriggerExit, OnCollisionEnter, OnCollisionExit, Distance }

    [Header("Select Type of Trigger")]
    public TypeOfTrigger m_TypeOfTrigger = new TypeOfTrigger();

    [Header("Text To Display")]
    public string m_Notification;
    public float m_TimeToDisplay;

    private bool IsTriggered = false;
    private Transform m_Target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        CheckDistance();

	}

    private void CheckDistance()
    {
        if (m_TypeOfTrigger != TypeOfTrigger.Distance)
            return;
    }

    private void Trigger()
    {
        if (!IsTriggered)
        {
            IsTriggered = true;
            InGameUIManager.InitOnUpdateNotification(m_Notification, m_TimeToDisplay);
        }
    }

    private IEnumerator ShowText()
    {
        yield return null;
    }


    //Unity Collision stuff==========================================================================

    public void OnCollisionEnter(Collision other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnCollisionEnter)
            return;

        if (other.collider.tag != "Player")
            return;

        Trigger();
    }

    public void OnCollisionExit(Collision other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnCollisionExit)
            return;

        if (other.collider.tag != "Player")
            return;

        Trigger();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnTriggerEnter )
            return;

        if (other.tag != "Player")
            return;

        Trigger();
    }

    public void OnTriggerExit(Collider other)
    {
        if (m_TypeOfTrigger != TypeOfTrigger.OnTriggerExit)
            return;

        if (other.tag != "Player")
            return;

        Trigger();
    }
}
