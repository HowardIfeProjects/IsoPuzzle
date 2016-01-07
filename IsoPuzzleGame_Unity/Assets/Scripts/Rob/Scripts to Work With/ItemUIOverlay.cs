using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUIOverlay : MonoBehaviour {

    [SerializeField]
    private GameObject attachedItem;

    private Text m_itemTitle;

	// Use this for initialization
	void Start () {
        m_itemTitle.GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {
        //m_itemTitle.text = attachedItem.
	
	}

    void DisplayItemProperties()
    {

    }
}
