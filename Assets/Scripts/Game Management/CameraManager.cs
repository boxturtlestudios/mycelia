using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogAssertion("Duplicate UIControl destroyed!");
            Destroy(gameObject);
        }
    }

    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera houseCam;
    public CinemachineVirtualCamera interiorCam;

    public CinemachineVirtualCamera[] cameras;

    private void Start() 
    {
        SwitchToCamera(playerCam);
    }

    [EasyButtons.Button]
    private void LoadCameras()
    {
        cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
    }

    public void ViewHouse()
    {
        SwitchToCamera(houseCam);
    }

    public void ViewPlayer()
    {
        SwitchToCamera(playerCam);
    }

    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach(CinemachineVirtualCamera cam in cameras)
        {
            cam.enabled = (cam == targetCamera);
        }
    }

}
