using UnityEngine;
using System.Collections;

public class LevelRotation : MonoBehaviour {

    [SerializeField] float m_RotationSpeed;
    private Quaternion m_StartingRotation;
    private bool m_RotationComplete = true;

    //Unity Lifecycle=================================================================================

	// Use this for initialization
	void Start () {

        m_StartingRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

        if (m_RotationComplete)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopAllCoroutines();
                StartCoroutine(Rotate(90));
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                StopAllCoroutines();
                StartCoroutine(Rotate(-90));
            }
        }


    }

    //-----------------------------------------------------------------------------------------

    private IEnumerator Rotate(float _amount)
    {
        m_RotationComplete = false;
        Quaternion _finalRotation = Quaternion.Euler(0, _amount, 0) * m_StartingRotation;

        while (transform.rotation != _finalRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _finalRotation, Time.deltaTime * m_RotationSpeed);
            yield return 0;
        }

        m_RotationComplete = true;
        m_StartingRotation = transform.rotation;
    }
}
