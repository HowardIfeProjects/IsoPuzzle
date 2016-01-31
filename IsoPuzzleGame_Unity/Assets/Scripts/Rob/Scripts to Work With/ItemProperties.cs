using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemProperties : MonoBehaviour {


    //m_ = enum or method?
    int m_itemID;

    [SerializeField]
    public string m_itemDescription;

    [SerializeField]
    private Sprite m_itemImage;

    public GameObject _hud;

    public string m_itemTitle;

    public GameObject m_itemUI;

    public Text m_itemTitleText;

    public ItemID.ItemIdentity m_ItemType = new ItemID.ItemIdentity();

    private HUDScript hudScript;

    private void Awake()
    {
        hudScript = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDScript>();
        SetID();
        m_itemUI.SetActive(false);
        SetUpUI();
    }

    private void SetID()
    {
        switch(m_ItemType)
        {
            case ItemID.ItemIdentity.BlueCube:
                m_itemID = ItemID.BlueCubeID;
                break;

            case ItemID.ItemIdentity.BrownCube:
                m_itemID = ItemID.BrownCubeID;
                break;

            case ItemID.ItemIdentity.YellowCube:
                m_itemID = ItemID.YellowCubeID;
                break;
        }
    }

    public void SetUpUI()
    {
        m_itemTitleText.text = m_itemTitle;
    }

    public void PickUpItem()
    {
        //change to a delegent 
        //ManagerSystem.callDelegent
        hudScript.AddItemImage(m_itemImage, m_itemID);
        //
        InventorySystemManager.CallAddItem(m_itemID, m_itemDescription, m_itemImage);
        Destroy(gameObject);
    }
}
