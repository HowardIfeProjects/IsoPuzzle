using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformMovement : MonoBehaviour {

    [HeaderAttribute("DEBUG")]
    [SerializeField] bool SHOW_PATHS;

    public enum PlatformType { ConstantMovement, IncrementMovement, DelayedMovement }
    [Header("Variables")]
    public PlatformType m_PlatformType = new PlatformType();
    public float m_Speed;
    public bool m_Loop;
    public bool m_FollowPathBackwards;
    public bool m_Move;
    public bool m_MoveInIntervals;
    public float m_DelayTime;
    [SerializeField] List<Transform> m_Points = new List<Transform>();

    //Move() stuff
    private int currentindex = 0;
    private int m_currentIndex {
        get { return currentindex; }
        set { currentindex = value; }
    }

    private bool m_CounterStarted = false;
    private bool m_IncreaseIndex = true;


    private LineRenderer m_LineRenderer;

    //UNITY LIFECYCLE=======================================================
    //======================================================================

    // Use this for initialization
    private void Start() {

        for (int i = 0; i < m_Points.Count; i++) {
            m_Points[i].parent = null;
        }

        m_LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update() {

        if (GameManager.isPaused)
            return;

        Move();
        DrawPath();

    }
    //======================================================================

    //TODO: Tidy up the m_current waypoint stuff into getter and setter
    private void Move() {

        if (m_Move)
        {
            if (m_PlatformType == PlatformType.IncrementMovement || m_PlatformType == PlatformType.ConstantMovement)
            {
                if (m_currentIndex < m_Points.Count)
                {
                    Vector3 _curWaypointPos = m_Points[m_currentIndex].position;
                    Vector3 _moveDirection = _curWaypointPos - transform.position;

                    if (_moveDirection.magnitude < 0.1f)
                    {
                        transform.position = _curWaypointPos;

                        if (!m_MoveInIntervals)
                        {
                            if (m_currentIndex <= 0)
                                m_IncreaseIndex = true;

                            if (m_IncreaseIndex)
                                m_currentIndex++;
                            else
                                m_currentIndex--;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, _curWaypointPos, Time.deltaTime * m_Speed);
                    }
                }
                else
                {
                    if (m_FollowPathBackwards)
                    {
                        if (m_currentIndex == 0)
                            m_IncreaseIndex = true;
                        else if (m_currentIndex >= m_Points.Count)
                        {
                            m_currentIndex = (m_Points.Count - 2);
                            m_IncreaseIndex = false;
                        }
                    }
                    else if (m_Loop)
                        m_currentIndex = 0;
                    else
                    {
                        m_Move = false;
                        m_currentIndex = 0;
                    }
                }
            }
            else if (m_PlatformType == PlatformType.DelayedMovement)
            {
                if (m_currentIndex < m_Points.Count)
                {
                    Vector3 _curWaypointPos = m_Points[m_currentIndex].position;
                    Vector3 _moveDirection = _curWaypointPos - transform.position;

                    if (_moveDirection.magnitude < 0.1f)
                    {
                        transform.position = _curWaypointPos;

                        if (!m_CounterStarted)
                            StartCoroutine(Delay(m_DelayTime));
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, _curWaypointPos, Time.deltaTime * m_Speed);
                    }
                }
                else
                {
                    if (m_Loop)
                        m_currentIndex = 0;
                    else
                    {
                        m_Move = false;
                        m_currentIndex = 0;
                    }
                }
            }
        }
    }

    private IEnumerator Delay(float _time)
    {
        m_CounterStarted = true;
        yield return new WaitForSeconds(_time);
        m_currentIndex++;
        m_CounterStarted = false;
    }

    private void DrawPath()
    {
        if (m_LineRenderer == null)
            return;

        if (m_FollowPathBackwards)
            m_LineRenderer.SetVertexCount(m_Points.Count);
        else
            m_LineRenderer.SetVertexCount(m_Points.Count + 1);

        for (int i = 0; i < m_Points.Count; i++)
        {
            m_LineRenderer.SetPosition(i, m_Points[i].transform.position);

            if (!m_FollowPathBackwards && i == (m_Points.Count - 1))
                m_LineRenderer.SetPosition(i + 1, m_Points[0].transform.position);
      
        }

    }

    //Send Message Methods
    public void EventTriggered()
    {
        m_Move = !m_Move;
    }


    public void NextTarget()
    {
        m_currentIndex++;
    }
    //GIZMOS====================================

    void OnDrawGizmos(){

        if (!SHOW_PATHS || m_Points == null || m_Points.Count < 2)
            return;

        if (!m_FollowPathBackwards)
        {
            for (int i = 1; i < m_Points.Count; i++)
            {

                if (i == m_Points.Count - 1)
                {
                    if (m_Loop)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(m_Points[i].position, m_Points[0].position);
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(m_Points[i].position, 0.25f);
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(m_Points[i - 1].position, m_Points[i].position);
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(m_Points[i].position, 0.25f);
                    }

                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(m_Points[i - 1].position, m_Points[i].position);
                Gizmos.DrawWireSphere(m_Points[i - 1].position, 0.25f);
            }
        }
        else
        {
            for (int i = 1; i < m_Points.Count; i++)
            {
                if (i == m_Points.Count - 1)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(m_Points[i - 1].position, m_Points[i].position);
                    Gizmos.DrawWireSphere(m_Points[i].position, 0.25f);
                }
                Gizmos.color = Color.green;
                Gizmos.DrawLine(m_Points[i - 1].position, m_Points[i].position);
                Gizmos.DrawWireSphere(m_Points[i - 1].position, 0.25f);
            }
        }      

    }
}
