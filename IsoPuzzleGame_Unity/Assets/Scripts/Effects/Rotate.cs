using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

	
	// Update is called once per frame
	void Update () {

        transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
      

	}
}
