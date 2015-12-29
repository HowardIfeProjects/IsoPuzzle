using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInventorySystemManager : MonoBehaviour {

    //TODO: Set up a gameObject script and assign it into this Class, then find a way to show a hud when near an object 

    //public InventorySystemManager m_inventorySystem; not needed atm

    [SerializeField]
    bool DEBUG;

    [SerializeField]
    GameObject[] m_Items;
    private GameObject m_Player;
    private List<GameObject> m_itemsInView = new List<GameObject>();

    //public GameObject itemUI; Not needed atm 

    void Start()
    {

        m_Player = GameObject.FindGameObjectWithTag("Player");

        if(GameObject.FindGameObjectWithTag("Item"))
            m_Items = GameObject.FindGameObjectsWithTag("Item");
    }

    void Update()
    {
        if (m_Items.Length <= 0)
            return;

        float closestObject;
        closestObject = Mathf.Infinity;


        for (int i = 0; i < m_Items.Length; i++)
        {
            ItemProperties m_itemPorperties;
            GameObject currentObject = m_Items[i];

            // Debug.Log(currentObject);


            /*
            if (m_Items[i].GetComponent<ItemProperties>() != null)
            {
                itemPorperties = m_Items[i].GetComponent<ItemProperties>();
                //itemPorperties.TestPass();//the variable will trigger this method from the other script (make sure it's public else it wont work)
                Debug.Log(m_Items[i] + " Works!");
            }*/

            if (m_Items[i] != null)
            {
                RaycastHit _hit;
                Vector3 _dir = m_Player.transform.position - m_Items[i].transform.position;

                if (Physics.Raycast(m_Items[i].transform.position, _dir, out _hit))
                {
                    if (_hit.collider.gameObject == m_Player)
                    {
                        if (!m_itemsInView.Contains(m_Items[i].gameObject))
                            m_itemsInView.Add(m_Items[i].gameObject);

                        if (DEBUG)
                            Debug.Log("Item: " + m_Items[i].name + "   Distance : " + (m_Items[i].transform.position - m_Player.transform.position).magnitude);
                    }
                    else
                        m_itemsInView.Remove(m_Items[i].gameObject);

                    if (Vector3.Distance(m_Player.transform.position, m_Items[i].transform.position) < closestObject)
                    {
                        closestObject = Vector3.Distance(m_Player.transform.position, m_Items[i].transform.position);
                        if(DEBUG)
                            Debug.Log("The closest object is: " + m_Items[i]);

                        bool closeToItem = (Vector3.Distance(m_Player.transform.position, m_Items[i].transform.position) <= 3f);

                            if (m_Items[i].GetComponent<ItemProperties>() != null)
                            {
                                m_itemPorperties = m_Items[i].GetComponent<ItemProperties>();

                        if (closeToItem)
                        {

                                m_itemPorperties.m_itemUI.gameObject.SetActive(true);

                                if ((Input.GetKeyDown(KeyCode.R)) && (closeToItem))
                                {
                                    //itemPorperties.TestPass();//the variable will trigger this method from the other script (make sure it's public else it wont work)
                                     m_itemPorperties.PickUpItem();
                                }

                                //m_itemsInView.Remove(m_Items[i]);
                            }
                            else
                            {
                               m_itemPorperties.m_itemUI.gameObject.SetActive(false);
                            }
                        }
                    }

                    if (DEBUG)
                        Debug.DrawRay(m_Items[i].transform.position, _dir, Color.yellow);
                }
            }

            if (m_itemsInView.Count > 0)
            {
            }
        }
    }
}
