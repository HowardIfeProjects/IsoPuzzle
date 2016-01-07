using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public string _LevelName;
    public int _LevelID;
    public string[] _Objectives;
    public int _CurObjectiveIndex;
    public bool _IsLevelComplete;
}

public class LevelManager : MonoBehaviour {

    //level tracking
    public List<LevelData> li_LevelData = new List<LevelData>();
    public int m_CurrentLevel;

    //Level Events--------------------------------------------------------------
    public delegate void E_LevelComplete();
    public static event E_LevelComplete OnLevelComplete;

    public delegate void E_LoadLevel(int _curLevel);
    public static event E_LoadLevel OnLoadLevel;

    public delegate void E_LoadNextObjective();
    public static event E_LoadNextObjective OnLoadNextObjective;

    public delegate LevelData E_ReturnLevelData();
    public static event E_ReturnLevelData OnReturnLevelData;

    public delegate void E_Nullify();
    public static event E_Nullify OnNullify;

    //==========================================================================

    //Hud Stuff
    [SerializeField] Text m_ObjectiveText;

    //UNITY LIFECYCLE-----------------------------------------------------------

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        PrefabFactory.InitPrefabs();

        //init delegates
        LevelManager.OnReturnLevelData += ReturnLevelData;
        LevelManager.OnLevelComplete += LevelComplete;
        LevelManager.OnLoadNextObjective += LoadNextObjective;

        Initialise();

    }

    private void Initialise()
    {
        m_ObjectiveText = GameObject.Find("Mission_Text").GetComponent<Text>();
    }

	// Use this for initialization
	private void Start () {

        InitFirstObjective();

    }
	
	// Update is called once per frame
	private void Update () {

    }

    //==========================================================================


    //Delegates
    private LevelData ReturnLevelData()
    {
        return li_LevelData[m_CurrentLevel];
    }

    private void LoadNextObjective()
    {
        //if the current index is greater then the length of the objective array
        if (li_LevelData[m_CurrentLevel]._CurObjectiveIndex > li_LevelData[m_CurrentLevel]._Objectives.Length) {
            //Next Level
            LevelManager.OnLevelComplete();
        }
        else
            li_LevelData[m_CurrentLevel]._CurObjectiveIndex++;

        //update HUD
        if(m_CurrentLevel < li_LevelData.Count-1)
            m_ObjectiveText.text = li_LevelData[m_CurrentLevel]._Objectives[li_LevelData[m_CurrentLevel]._CurObjectiveIndex];
    }

    private void InitFirstObjective()
    {
        //update HUD
        m_ObjectiveText.text = li_LevelData[m_CurrentLevel]._Objectives[li_LevelData[m_CurrentLevel]._CurObjectiveIndex];
    }

    private void LevelComplete()
    {
    }

    

    //--------------------------------------------------------------------------


    //Statics for Delegates

    public static void Init_LoadLevel(int i)
    {
        LevelManager.OnLoadLevel(i);
    }

    public static void Init_LevelComplete()
    {
        LevelManager.OnLevelComplete();
    }

    public static void Init_LoadNextObjective()
    {
        LevelManager.OnLoadNextObjective();
    }

    public static LevelData Init_ReturnLevelData()
    {
        return OnReturnLevelData();
    }
    //---------------------------------------------------------------------------
}
