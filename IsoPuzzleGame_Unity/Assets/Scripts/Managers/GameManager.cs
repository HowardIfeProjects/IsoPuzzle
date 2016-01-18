using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Game States
    public static bool isPaused = false;

    //SAVE / LOAD Delegates
    public delegate void E_Save();
    public static event E_Save OnSave;

    public delegate void E_Load();
    public static event E_Load OnLoad;


    //UNITY LIFECYCLE===================================================
    // Use this for initialization

    private void Awake() {

        GameObject.DontDestroyOnLoad(gameObject);

    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //==================================================================
}
