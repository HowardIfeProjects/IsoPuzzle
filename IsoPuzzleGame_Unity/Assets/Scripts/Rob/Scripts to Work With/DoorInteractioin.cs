using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorInteractioin : MonoBehaviour
{

    [SerializeField]
    private bool _debug = false;

    [SerializeField]
    private GameObject inventoryManager;

    private GameObject m_player;

    public GameObject m_itemUI;

    public Text m_itemTitle;

    public Text m_useItem;

    public Text m_tempLogo;

    private InventorySystemManager _inventoryLibary;

    private GameObject playerInventory;

    public ItemID.ItemIdentity m_ItemID = new ItemID.ItemIdentity();

    public int _requestedItemNumber;//put in the item number found in 'ItemID' so the script can look for a item associated to value in this int

    private int m_itemNumber;

    private HUDScript hudScript;//remove? doesn't look like it's being used

    private void Awake()
    {
        hudScript = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDScript>();//remove? doesn't look like it is being used 
        m_itemUI.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");

        if (inventoryManager.GetComponent<InventorySystemManager>())
        {
            _inventoryLibary = inventoryManager.GetComponent<InventorySystemManager>();//assigns the InventorySystemManager script assigned on the inventoryManager gameObject 


        }
    }

    // Organise all this, it's cluttered
    void Update ()
    {
       // PlayerInteract();
        CheckPlayerDistance();

        if(_inventoryLibary.li_inventoryHolder.ContainsKey(_requestedItemNumber))
        {
            if(_debug)
            {
                Debug.Log("Player has picked up number "+ _requestedItemNumber +"!");
            }
        }

            if(_debug)
            
            Debug.Log("This component exists! " +  _inventoryLibary.li_inventoryHolder.Count);//counts how many items are in that dictionary 
              }

        void PlayerInteract()
    {
        m_itemUI.SetActive(true);

        if (_inventoryLibary.li_inventoryHolder.ContainsKey(_requestedItemNumber))
        {
            m_useItem.text = "Open door";
            m_tempLogo.text = "Space";

            if (Input.GetKeyDown(KeyCode.Space))
                {
                    Run();
                }
        }
        else
        {
            m_useItem.text = "Pick up Blue Box to open";
            m_tempLogo.text = "";
        }
    }

        void Run()
    {
        //InventorySystemManager.CallCheckForItem(m_ItemID);
        if (_inventoryLibary.li_inventoryHolder.ContainsKey(_requestedItemNumber))
        {
            hudScript.RemoveItemImage(_requestedItemNumber);
            InventorySystemManager.CallUseItem(_requestedItemNumber);
            Destroy(gameObject);
        }

        if (_debug)
            Debug.Log("This is running! Looking for: " + m_ItemID);
    }

    void CheckPlayerDistance() //Didn't make sense to use since it is only used for one object 
    {
       Vector3 _dir = m_player.transform.position - this.transform.position;
        Debug.DrawRay( this.transform.position, _dir, Color.red);
        if (Vector3.Distance(m_player.transform.position, this.transform.position) <= 6f) //needs explaining 
        {
            PlayerInteract();
        }
        else
            m_itemUI.SetActive(false);
    }
}
