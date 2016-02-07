using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIManager : MonoBehaviour {

    [Header("UI Objects")]
    [SerializeField] GameObject NotifcationScreen;
    [SerializeField] Text NotificationText;

    public delegate void E_UpdateNotification(string s, float t);
    public static event E_UpdateNotification OnUpdateNotification;

    //======================================================================================
    //UNITY LIFECYCLE=======================================================================
    //======================================================================================
    private void Awake()
    {
        InGameUIManager.OnUpdateNotification += UpdateNotification;
    }

    // Use this for initialization
    private  void Start () {

        NotifcationScreen.SetActive(false);
    }
	
	// Update is called once per frame
	private void Update () {
	
	}
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------

    //======================================================================================
    //Private Methods=======================================================================
    //======================================================================================

    private void UpdateNotification(string s, float t)
    {
        StopAllCoroutines();

        IEnumerator _instance = DisplayNotifcation(s, t);
        StartCoroutine(_instance);
    }

    private IEnumerator DisplayNotifcation(string s, float t)
    {
        NotifcationScreen.SetActive(true);
        NotificationText.text = s;

        yield return new WaitForSeconds(t);

        NotifcationScreen.SetActive(false);
    }

    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------

    //========================================================================================
    //Statics=================================================================================
    //========================================================================================

    public static void InitOnUpdateNotification(string s, float t)
    {
        InGameUIManager.OnUpdateNotification(s, t);
    }

    //----------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------
}
