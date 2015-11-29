using UnityEngine;
using System.Collections;

public class ItemInstance : MonoBehaviour {

    int m_ID;

    [SerializeField]
    string m_Description;

    public ItemID.ItemEnum m_ItemType = new ItemID.ItemEnum();

    private void Awake()
    {
        InitID();
    }

    private void InitID()
    {
        switch (m_ItemType)
        {
            case ItemID.ItemEnum.Gun:
                m_ID = ItemID.Gun_ID;
                break;
            case ItemID.ItemEnum.Shoe:
                m_ID = ItemID.Shoe_ID;
                break;
            case ItemID.ItemEnum.Stick:
                m_ID = ItemID.Stick_ID;
                break;
        }
    }

    public void PickUpItem()
    {
        Test.CallAddItem(m_ID, m_Description);
        Destroy(gameObject);
    }
}
