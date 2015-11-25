using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInventorySystem : MonoBehaviour {

    //TODO: Set up a gameObject script and assign it into this Class, then find a way to show a hud when near an object 


    [SerializeField]
    bool DEBUG;

    [SerializeField]
    GameObject[] m_Items;//creates an array for the items to be tempoaraly stored in 
    private GameObject m_Player;//creates a var for the player to be assigned to 
    private List<GameObject> m_itemsInView = new List<GameObject>();//creates a list for the items to be permantly stored in

    public GameObject itemUI;

    // Use this for initialization
    void Start()
    {

        m_Player = GameObject.FindGameObjectWithTag("Player");//Sets a gameObject up for anything assinged with the tag "Player" 
        m_Items = GameObject.FindGameObjectsWithTag("Item");//adds a gameObject to the array if it has the tag "Item"
    }

    // Update is called once per frame
    void Update()
    {
        float closestObject;
        closestObject = Mathf.Infinity;

        for (int i = 0; i < m_Items.Length; i++)//if i is less than the number items in the array, run this function and add 1 to the value of i
        {

            if (m_Items[i] != null)//if the item number in the array does not equal nothing e.g. m_Items[2] has a gameObject in, this will run the function however if m_Item[2] has no gameObject in then the function will not run 
            {
                RaycastHit _hit;//get info back from the ray hitting an object 
                Vector3 _dir = m_Player.transform.position - m_Items[i].transform.position;//sets the end point for the ray which is the posistion of the player minus the posistion of the current gameObject this function is running for

                if (Physics.Raycast(m_Items[i].transform.position, _dir, out _hit))//(start pos: current gameObjects's posistion, end pos: the player's current posistion, the ray being used for this raycast)
                {
                    if (_hit.collider.gameObject == m_Player)
                    {
                        if (!m_itemsInView.Contains(m_Items[i].gameObject))//if the list m_Items does not contain the current gameObject for this function...
                            m_itemsInView.Add(m_Items[i].gameObject);//...add it to the list 

                        if (DEBUG)
                            Debug.Log("Item: " + m_Items[i].name + "   Distance : " + (m_Items[i].transform.position - m_Player.transform.position).magnitude);
                    }
                    else
                        m_itemsInView.Remove(m_Items[i].gameObject);//if it's already on the list then remove this current attempt since it isn't needed 
                    
                    /*vector3.distance: compares two arguments. Like a ray first argument is the start posistion and the end is the 2nd argment
                    Distance detects from where the player stands from the gameobject that this function is running for,
                    if that distance is less than the current value in 'closestObject' then it will run this function*/

                    if(Vector3.Distance(m_Player.transform.position,m_Items[i].transform.position)<closestObject)
                    {
                        //if ran, the current value in 'closestObject' will be overwritten with the distance that triggered this function
                        closestObject = Vector3.Distance(m_Player.transform.position, m_Items[i].transform.position);
                        Debug.Log("The closest object is: " + m_Items[i]);//this shows what object is the closest

                        bool closeToItem = (Vector3.Distance(m_Player.transform.position, m_Items[i].transform.position) <= 2);//if the distance between the player and the iteam in leaas or equal to 2, then this will set as true
                        bool uiExists = (GameObject.Find("Item UI"));//if the script finds an object in the game called "Item UI", then this will be true however, this currently doesnt work
                        if(closeToItem)
                        {
                            if(!uiExists)
                            {
                                Instantiate(itemUI, m_Items[i].transform.position, Quaternion.identity);//spawns the gameobject associated with 'itemUI' however, this statement does not work at the moment 
                            }
                        }
                        else
                        {
                            if(uiExists)
                            {
                                Destroy(itemUI);
                            }
                        }

                        if ((Input.GetKeyDown(KeyCode.E)) && (closeToItem))
                        {
                            Destroy(m_Items[i]);
                            //m_itemsInView.Remove(m_Items[i]);
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
