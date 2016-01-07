using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour {

    private Vector3 m_RepspawnPos;
	// Use this for initialization
	void Start () {

        m_RepspawnPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 _Pos = transform.position;
        if (_Pos.y < -5f)
            transform.position = m_RepspawnPos;
	
	}
}
