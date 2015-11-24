using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayExample : MonoBehaviour {

    [SerializeField] bool DEBUG;

    [SerializeField] GameObject[] m_Items;
    private GameObject m_Player;
    private List<GameObject> m_itemsInView = new List<GameObject>();


	// Use this for initialization
	void Start () {

        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Items = GameObject.FindGameObjectsWithTag("Item");

	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < m_Items.Length; i++)
        {
            if (m_Items[i] != null)
            {
                RaycastHit _hit;
                Vector3 _dir = m_Player.transform.position - m_Items[i].transform.position;

                if (Physics.Raycast(m_Items[i].transform.position, _dir, out _hit))
                {
                    if (_hit.collider.gameObject == m_Player)
                    {
                       if(!m_itemsInView.Contains(m_Items[i].gameObject))
                           m_itemsInView.Add(m_Items[i].gameObject);

                       if(DEBUG)
                            Debug.Log( "Item: " + m_Items[i].name + "   Distance : " + (m_Items[i].transform.position - m_Player.transform.position).magnitude);
                    }
                    else
                        m_itemsInView.Remove(m_Items[i].gameObject);

                }

                if(DEBUG)
                 Debug.DrawRay(m_Items[i].transform.position, _dir, Color.yellow);
            }
        }

        if (m_itemsInView.Count > 0)
        {
           
        }

	}
}
