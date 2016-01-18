using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class MainMenuManager : MonoBehaviour {

    [Header("Canvases")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject LevelSelect;

	// Use this for initialization
	void Start () {

        ToMenu("MainMenu");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToMenu(string str)
    {
        switch (str)
        {
            case "MainMenu":
                MainMenu.SetActive(true);
                LevelSelect.SetActive(false);
                break;
            case "LevelSelect":
                MainMenu.SetActive(false);
                LevelSelect.SetActive(true);
                break;

            case "NewGame":
                SceneManager.LoadScene("Level_01");
                break;
        }
    }
}
