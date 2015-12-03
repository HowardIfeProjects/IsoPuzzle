using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayExample : MonoBehaviour {

    [SerializeField] bool DEBUG;

    [SerializeField] GameObject[] m_Items;//creates an array for the items to be tempoaraly stored in 
    private GameObject m_Player;//creates a var for the player to be assigned to 
    private List<GameObject> m_itemsInView = new List<GameObject>();//creates a list for the items to be permantly stored in


	// Use this for initialization
	void Start () {
        
        m_Player = GameObject.FindGameObjectWithTag("Player");//Sets a gameObject up for anything assinged with the tag "Player" 
        m_Items = GameObject.FindGameObjectsWithTag("Item");//adds a gameObject to the array if it has the tag "Item"

	}
	
	// Update is called once per frame
	void Update () {

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
                       if(!m_itemsInView.Contains(m_Items[i].gameObject))//if the list m_Items does not contain the current gameObject for this function...
                           m_itemsInView.Add(m_Items[i].gameObject);//...add it to the list 

                       if(DEBUG)
                            Debug.Log( "Item: " + m_Items[i].name + "   Distance : " + (m_Items[i].transform.position - m_Player.transform.position).magnitude);
                    }
                    else
                        m_itemsInView.Remove(m_Items[i].gameObject);//if it's already on the list then remove this current attempt since it isn't needed 

                }

                if(DEBUG)
                 Debug.DrawRay(m_Items[i].transform.position, _dir, Color.yellow);
            }
        }

        if (m_itemsInView.Count > 0)
        {
           //if(Vector3.Distance(m_Player.transform.position, m_Items[i]))
        }

	}
}
