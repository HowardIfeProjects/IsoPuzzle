using UnityEngine;
using System.Collections;

public class TurretBehaviour : TurretBase {

    public enum TypeOfTurret { Basic }
    public TypeOfTurret m_TypeOfTurret = new TypeOfTurret();

    void Start() {
        m_Target = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update () {

        base.Target();

        if (base.CanSeeTarget())
        {
            if (CanFire)
            {
                if(m_TypeOfTurret == TypeOfTurret.Basic)
                    base.Fire();
            }
        }
	}

    
}
