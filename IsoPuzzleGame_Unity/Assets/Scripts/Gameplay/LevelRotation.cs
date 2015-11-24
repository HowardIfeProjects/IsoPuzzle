using UnityEngine;
using System.Collections;

public class LevelRotation : MonoBehaviour {

    [Header("DEBUG")]
    [SerializeField] bool IsAttachedToPlayer;
    private Vector3 DebugPlayerPos;
    private Vector3 DebugCameraPos;

    [Header("Movement Variables")]
    [SerializeField] float m_RotationSpeed;
    [SerializeField] float m_MovementSpeed;
    [SerializeField] float m_FollowDelayTime;

    public enum TrackingEnum
    {
        Horizontal,
        VerticalandHorizontal
    }
    [Header("Tracking Mode Not Working Currently")]
    public TrackingEnum TrackingMode = new TrackingEnum();

    //Private Variables
    private Quaternion m_StartingRotation;
    private bool m_RotationComplete = true;
    private GameObject m_Camera;
    private float m_currentLerpTime;
    private bool m_StartFollowingPlayer = true;
    private bool m_DelayStarted = false;
    private GameObject m_Player;

    //Unity Lifecycle=================================================================================

    // Use this for initialization
    void Start () {

        m_StartingRotation = transform.rotation;
        Initialise();
	}
	
	// Update is called once per frame
	void Update () {

        PlayerInput();
        TrackPlayer();

    }

    private void Initialise()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = Camera.main.gameObject;
    }

    //-----------------------------------------------------------------------------------------

    private void TrackPlayer()
    {
        if (!m_RotationComplete)
            return;

        Vector3 _playerPos = m_Player.transform.position;
        Vector3 _camPos = m_Camera.transform.position;
        DebugPlayerPos = _playerPos;
        DebugCameraPos = _camPos;

        Vector3 _rot = m_Camera.transform.eulerAngles;

        if (TrackingMode == LevelRotation.TrackingEnum.VerticalandHorizontal)
            _camPos.y = _playerPos.y;

        if (Mathf.Round(_rot.y) == 0)
            _camPos.x = _playerPos.x;
        else if (Mathf.Round(_rot.y) == 90)
            _camPos.z = _playerPos.z;
        else if (Mathf.Round(_rot.y) == 180)
            _camPos.x = _playerPos.x;
        else if (Mathf.Round(_rot.y) == 270)
            _camPos.z = _playerPos.z;

        if (m_StartFollowingPlayer)
            m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, _camPos, m_MovementSpeed * Time.deltaTime);

        if ((m_Camera.transform.position - _camPos).magnitude < 0.02f)
        {
            m_Camera.transform.position = _camPos;
            m_StartFollowingPlayer = false;
            m_DelayStarted = false;
        }
        else
        {
            if (!m_StartFollowingPlayer && !m_DelayStarted)
                StartCoroutine(DelayBeforeFollow(m_FollowDelayTime));
        }
        
    }

    private void PlayerInput()
    {
        if (!m_RotationComplete)
            return;

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

    private IEnumerator DelayBeforeFollow(float _time)
    {
        m_DelayStarted = true;
        yield return new WaitForSeconds(_time);
        m_StartFollowingPlayer = true;
        m_DelayStarted = false;
    }

    private IEnumerator Rotate(float _amount)
    {
        if (!IsAttachedToPlayer)
            m_Camera.transform.parent = gameObject.transform;

        m_RotationComplete = false;
        Quaternion _finalRotation = Quaternion.Euler(0, _amount, 0) * m_StartingRotation;

        while (transform.rotation != _finalRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _finalRotation, Time.deltaTime * m_RotationSpeed);
            yield return 0;
        }

        m_RotationComplete = true;
        transform.rotation = _finalRotation;
        m_StartingRotation = transform.rotation;

        m_Camera.transform.parent = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(DebugCameraPos, new Vector3(1f, 1f, 1f));
        Gizmos.DrawWireCube(DebugPlayerPos, new Vector3(1f, 1f, 1f));
    }
}
