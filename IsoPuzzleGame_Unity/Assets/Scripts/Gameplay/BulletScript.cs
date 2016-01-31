using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    [SerializeField] float m_speed;
    private Rigidbody m_rigidbody;

	// Use this for initialization
	void Start () {

        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.velocity = transform.TransformDirection(0, 0, m_speed);

        Destroy(gameObject, 5f);
	}

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
	
}
