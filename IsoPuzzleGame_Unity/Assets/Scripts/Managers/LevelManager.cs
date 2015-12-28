using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int _Level;
    public string[] _Objectives;
    public int _CurObjectiveIndex;
    public bool _LevelComplete;
}

public class LevelManager : MonoBehaviour {

    //level tracking
    public List<LevelData> li_LevelData = new List<LevelData>();

    void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        PrefabFactory.InitPrefabs();
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }
}
