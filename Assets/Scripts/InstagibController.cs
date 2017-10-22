using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InstagibController : NetworkBehaviour {
    public float cooldownTime = .5f;
	public int beamDamage = 55;
    public GameObject beamPrefab;
    public GameObject explosionPrefab;

    private float _timeLastShot = 0;

    private MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private Vector3 StartPosition()
    {
        return _meshRenderer.transform.position + _meshRenderer.transform.forward * _meshRenderer.bounds.size.x;
    }

    private Vector3 Forward()
    {
        return _meshRenderer.transform.forward;
    }

    private Vector3 EndPosition(int distance)
    {
        return _meshRenderer.transform.position + _meshRenderer.transform.forward * distance;
    }

    void Update () {
        Debug.DrawLine(StartPosition(), EndPosition(100), Color.red);
    }

    public void Shoot()
    {
        float timeSinceLastShot = Time.time - _timeLastShot;
        if (timeSinceLastShot < cooldownTime) return;

        _timeLastShot = Time.time;

        Vector3 target = EndPosition(100);
        RaycastHit hitInfo;
        if (Physics.Raycast(StartPosition(), Forward(), out hitInfo))
        {
            GameObject collidingObject = hitInfo.collider.gameObject;
            var damageZone = collidingObject.GetComponent<DamageZone>();
//            Debug.Log(collidingObject.name);
//            Debug.Log(damageZone);
            if (damageZone != null)
            {
				damageZone.Hit(beamDamage, gameObject.transform.parent.gameObject);
            }
            target = hitInfo.point;
            RenderExplosion(target);
        }
        RenderBeam(StartPosition(), target);
    }

    void RenderExplosion(Vector3 target)
    {
        GameObject instance = Instantiate(explosionPrefab);
        instance.transform.position = target;
		NetworkServer.Spawn (instance);
    }

    void RenderBeam(Vector3 source, Vector3 target)
    {
        GameObject instance = Instantiate(beamPrefab);
        BeamController beamController = instance.GetComponent<BeamController>();
        if (beamController == null)
        {
            Debug.LogError("The beam prefab requires a beam controller");
            return;
        }
		NetworkServer.Spawn (instance);
		beamController.RpcFire(source, target);
	}
}
