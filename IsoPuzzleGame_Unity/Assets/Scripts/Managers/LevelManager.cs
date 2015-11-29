using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


    void Awake()
    {
        PrefabFactory.InitPrefabs();
    }
	// Use this for initialization
	void Start () {

        if (Test.CallCheckForItem(ItemID.Gun_ID) != null)
        {
            // have gun
        }
        else
        {
            //fail mission
        }
    }
	
	// Update is called once per frame
	void Update () {



    }
}
