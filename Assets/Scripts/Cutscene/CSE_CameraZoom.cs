using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CSE_CameraZoom : CutsceneElementBase
{
    [SerializeField] private float targetSize;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    
    private CinemachineVirtualCamera vCam;

    public override void Execute()
    {
        vCam = cutsceneHandler.vCam;
        StartCoroutine(ZoomCamera());
    }

    private IEnumerator ZoomCamera()
    {
        Vector3 originalPosition = vCam.transform.position;
        Vector3 targetPosition = target.position + offset;

        float OriginalSize = vCam.m_Lens.OrthographicSize;
        float startTime = Time.time;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(OriginalSize, targetSize, t);
            vCam.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

            elapsedTime = Time.time - startTime;
            yield return null;
        }

        vCam.m_Lens.OrthographicSize = targetSize;
        vCam.transform.position = targetPosition;

        cutsceneHandler.PlayNextElement();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
