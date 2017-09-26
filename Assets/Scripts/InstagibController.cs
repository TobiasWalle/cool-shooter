using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagibController : MonoBehaviour {
    public float cooldownTime = .5f;
    public GameObject beamPrefab;

    float timeLastShot = 0;

    void Update () {
        Vector3 forward = Vector3.forward * 100;
        Debug.DrawLine(transform.position, transform.position + forward, Color.red);
	}

    public void Shoot()
    {
        float timeSinceLastShot = Time.time - timeLastShot;
        if (timeSinceLastShot < cooldownTime) return;

        timeLastShot = Time.time;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo))
        {
            RenderBeam(hitInfo.point);
            GameObject gameObject = hitInfo.collider.gameObject;
            if (gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        } else
        {
            RenderBeam(transform.position + Vector3.forward * 10000);
        }
    }

    void RenderBeam(Vector3 target)
    {
        GameObject instance = Instantiate(beamPrefab);
        BeamController beamController = instance.GetComponent<BeamController>();
        if (beamController == null)
        {
            Debug.LogError("The beam prefab requires a beam controller");
            return;
        }

        beamController.Fire(transform.position, target);
    }
}
