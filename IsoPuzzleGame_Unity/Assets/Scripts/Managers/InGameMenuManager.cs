using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class InGameMenuManager : MonoBehaviour {

    public GameObject m_PauseMenu;

	// Use this for initialization
	void Start () {

        m_PauseMenu.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.isPaused = !GameManager.isPaused;

            if (GameManager.isPaused)
                m_PauseMenu.SetActive(true);
            else
                m_PauseMenu.SetActive(false);
        }
	
	}

    public void ToMainMenu() {

        SceneManager.LoadScene("MainMenu");

    }
}
