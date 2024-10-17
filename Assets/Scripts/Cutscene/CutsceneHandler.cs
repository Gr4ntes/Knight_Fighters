using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneHandler : MonoBehaviour
{
    public Camera cam;
    public CinemachineVirtualCamera vCam;
    private CutsceneElementBase[] cutsceneElements;
    private int index = -1;

    public delegate void CutSceneStarted();
    public static CutSceneStarted cutSceneStarted;

    public delegate void CutSceneEnded();
    public static CutSceneStarted cutSceneEnded;

    public void Start()
    {
        cutsceneElements = GetComponents<CutsceneElementBase>();
    }

    private void ExecuteCurrentElement()
    {
        if (index >= 0 && index < cutsceneElements.Length)
            cutsceneElements[index].Execute();
    }

    public void PlayNextElement()
    {
        index++;
        print(index);
        if (index >= cutsceneElements.Length) {
            cutSceneEnded?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            ExecuteCurrentElement();
        }
        
    }
}
