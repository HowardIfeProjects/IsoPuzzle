using UnityEngine;
using System.Collections;
using UnityEditor;

public class TurretBehaviour : TurretBase {

    public enum TypeOfTurret { Basic, Scanning }
    public TypeOfTurret m_TypeOfTurret = new TypeOfTurret();

    private bool IsLerping = false;
    private Vector3 m_StartRot;

    void Start() {
        m_Target = GameObject.FindGameObjectWithTag("Player");
        m_StartRot = transform.eulerAngles;
    }

	// Update is called once per frame
	void Update () {

        if (m_TypeOfTurret == TypeOfTurret.Basic)
        {
            base.Target();

            if (base.CanSeeTarget())
            {
                if (CanFire)
                {
                    if (m_TypeOfTurret == TypeOfTurret.Basic)
                        base.Fire();
                }
            }
        }
        else if (m_TypeOfTurret == TypeOfTurret.Scanning)
        {
            if(!base.CanSeeTarget())
                Target();
            else
                if (CanFire)
                    base.Fire();
        }
	}


    //SCANNING TURRET ========================================================
    public override void Target()
    {
        if(!IsLerping)
        {
            StartCoroutine(RotationLerp(1f, m_StartRot.y));
        }
    }

    private IEnumerator RotationLerp(float t, float y)
    {
        IsLerping = true;

        float elapsedTime = 0;
        float angle = m_FieldOfView;
        float startY = y - (angle / 2);
        float endY = y + (angle / 2);

        while (elapsedTime < t)
        {
            y = Mathf.Lerp(endY, startY, (elapsedTime / t));
            elapsedTime += Time.deltaTime;

            Vector3 _eular = transform.eulerAngles;
            _eular.y = y;
            transform.eulerAngles = _eular;

            yield return new WaitForEndOfFrame();
        }

        elapsedTime = 0;

        while (elapsedTime < t)
        {
            y = Mathf.Lerp(startY, endY, (elapsedTime / t));
            elapsedTime += Time.deltaTime;

            Vector3 _eular = transform.eulerAngles;
            _eular.y = y;
            transform.eulerAngles = _eular;

            yield return new WaitForEndOfFrame();
        }

        IsLerping = false;
    }

    // ==========================================================================


    public Vector3 DirFromAngle(float d, bool IsGlobal)
    {
        if (!IsGlobal)
        {
            d += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(d * Mathf.Deg2Rad), 0, Mathf.Cos(d * Mathf.Deg2Rad));
    }
}
