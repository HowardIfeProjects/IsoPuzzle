using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //SAVE / LOAD Delegates
    public delegate void E_Save();
    public static event E_Save OnSave;

    public delegate void E_Load();
    public static event E_Load OnLoad;


    //UNITY LIFECYCLE===================================================
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //==================================================================
}
