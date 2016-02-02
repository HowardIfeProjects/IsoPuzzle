using UnityEngine;
using System.Collections;

public class TurretBase : MonoBehaviour {

    protected GameObject m_Target;
    public float m_speed;
    public GameObject m_Projectile;
    public float m_ReloadTime;
    public float m_SightRange;
    protected bool CanFire = true;

    public float m_FieldOfView;

    public virtual void Target()
    {
        //set target pos
        Vector3 _targetPos = m_Target.transform.position;
        _targetPos.y = transform.position.y;
        Vector3 _moveDir = _targetPos - transform.position;

        if (Vector3.Distance(transform.position, _targetPos) < m_SightRange)
        {
            //rotate object
            Quaternion _rotation = Quaternion.LookRotation(_moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, Time.deltaTime * m_speed);
        }

    }

    public virtual void Fire()
    {
        //fire stuff
        GameObject clone;
        clone = Instantiate(m_Projectile, transform.TransformPoint(Vector3.forward), transform.rotation) as GameObject;

        CanFire = false;
        StartCoroutine(Reload(m_ReloadTime));
    }

    public virtual bool CanSeeTarget()
    {
        RaycastHit _hit;
        Vector3 _playerPos = m_Target.transform.position;
        _playerPos.y = _playerPos.y + 0.5f;
        Vector3 _raycastDirection = transform.TransformDirection(Vector3.forward) * m_SightRange;
        Debug.DrawRay(transform.position, _raycastDirection, Color.red);

        if (Physics.Raycast(transform.position, _raycastDirection, out _hit))
        {
            if (Vector3.Distance(transform.position, _hit.point) < m_SightRange)
            {
                if (_hit.collider.tag == "Player")
                    return true;
            }
            
        }
        return false;
    }

    public IEnumerator Reload(float t)
    {
        yield return new WaitForSeconds(t);
        CanFire = true;
    }
}
