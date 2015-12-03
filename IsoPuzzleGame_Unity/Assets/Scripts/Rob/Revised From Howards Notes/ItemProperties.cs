using UnityEngine;
using System.Collections;

public class ItemProperties : MonoBehaviour {


    //m_ = enum or method?
    int m_itemID;

    [SerializeField]
    private string m_itemDescription;

    [SerializeField]
    private Sprite m_itemImage;

    public GameObject m_itemUI;

    public ItemID.ItemIdentity m_ItemType = new ItemID.ItemIdentity();

    private void Awake()
    {
        SetID();
        m_itemUI.SetActive(false);
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

    public void PickUpItem()
    {
        InventorySystemManager.CallAddItem(m_itemID, m_itemDescription, m_itemImage);
        Destroy(gameObject);
    }
}
