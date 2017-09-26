using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamController : MonoBehaviour {
    public float duration = .5f;
    public float maxWidth = .2f;

    float width;
    float timeActivated;
    bool isActive = false;

    LineRenderer lineRenderer;

	void Update () {
        if (!isActive) return;
        float progressInPercent = GetProgress();
        if (progressInPercent > 1) Destroy(gameObject); // Destroy the beam after one shot

        width = maxWidth * (1 - progressInPercent);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
	}

    public void Fire(Vector3 start, Vector3 end)
    {
        if (isActive) return;
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] positions = { start, end };
        lineRenderer.SetPositions(positions);
        SetActive(true);
    }

    void SetActive(bool active)
    {
        isActive = active;
        lineRenderer.enabled = active;
        if (active) timeActivated = Time.time;
    }

    float GetProgress()
    {
        return (Time.time - timeActivated) / duration;
    }
}
