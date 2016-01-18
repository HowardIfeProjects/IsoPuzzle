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
    [SerializeField] float m_verticalDiff = 20f;
    [SerializeField] float m_elevationAngle = 10f;

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
    private Vector3 m_currentVector;

    //V2
    float m_angle;

    float m_horizontalDiff;
    float s;
    float c;
    float m_lengthRatios;
    Vector2 dir;

    //Unity Lifecycle=================================================================================

    // Use this for initialization
    private void Start () {

        m_StartingRotation = transform.rotation;
        Initialise();

	}

    private void Update() {

        if (GameManager.isPaused)
            return;

        PlayerInput();

    }
	
	// Update is called once per frame
	private void LateUpdate () {

        if (GameManager.isPaused)
            return;

        TrackPlayer();

    }

    private void Initialise()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = Camera.main.gameObject;
    }

    //-----------------------------------------------------------------------------------------

    private void HorizontalCalculation()
    {
        m_horizontalDiff = m_verticalDiff / Mathf.Tan(m_elevationAngle * Mathf.Deg2Rad);
    }

    private void TrackPlayer()
    {
        if (!m_RotationComplete)
            return;

        Vector3 _playerPos = m_Player.transform.position;
        Vector3 _camPos = m_Camera.transform.position;

        Vector3 _rot = m_Camera.transform.eulerAngles;

        if (TrackingMode == LevelRotation.TrackingEnum.VerticalandHorizontal)
            _camPos.y = _playerPos.y;

        HorizontalCalculation();

        s = Mathf.Sin(m_angle * Mathf.Deg2Rad);
        c = Mathf.Cos(m_angle * Mathf.Deg2Rad);

        dir = new Vector2(s, c).normalized * m_horizontalDiff;

        _camPos = ReturnCamPosition(_rot.y, _camPos);

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
            StartCoroutine(Rotate(-90));
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();
            StartCoroutine(Rotate(90));
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

    private Vector3 ReturnCamPosition(float y, Vector3 _pos)
    {
        if (Mathf.Round(y) == 0)
        {
            _pos.x = m_Player.transform.position.x + dir.x;
            _pos.z = m_Player.transform.position.z - dir.y;
            _pos.y = m_Player.transform.position.y + m_verticalDiff;
        }
        else if (Mathf.Round(y) == 180)
        {
            _pos.x = m_Player.transform.position.x - dir.x;
            _pos.z = m_Player.transform.position.z + dir.y;
            _pos.y = m_Player.transform.position.y + m_verticalDiff;
        }
        else if (Mathf.Round(y) == 90)
        {
            _pos.x = m_Player.transform.position.x - dir.y;
            _pos.z = m_Player.transform.position.z + dir.x;
            _pos.y = m_Player.transform.position.y + m_verticalDiff;
        }
        else if (Mathf.Round(y) == 270)
        {
            _pos.x = m_Player.transform.position.x + dir.y;
            _pos.z = m_Player.transform.position.z - dir.x;
            _pos.y = m_Player.transform.position.y + m_verticalDiff;
        }

        return _pos;
    }



}
