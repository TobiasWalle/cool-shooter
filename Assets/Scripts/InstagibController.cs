using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagibController : MonoBehaviour {

    public float cooldownTime = 2f;
    float timeLastShot = 0;
    LineRenderer line;

	void Start () {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
	}
	
	void Update () {
        Vector3 forward = Vector3.forward * 100;
        Debug.DrawLine(transform.position, transform.position + forward, Color.red);
	}

    public void Shoot()
    {
        float timeSinceLastShot = Time.time - timeLastShot;
        if (timeSinceLastShot < cooldownTime) return;
        print("Shot");
        timeLastShot = Time.time;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo))
        {
            GameObject gameObject = hitInfo.collider.gameObject;
            if (gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}
