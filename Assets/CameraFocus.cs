using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocus : MonoBehaviour
{
    private CinemachineVirtualCamera cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        CinemachineBrain.SoloCamera = cam;
    }

    // Update is called once per frame
    void Update()
    {
        CinemachineBrain.SoloCamera = cam;
    }
}
