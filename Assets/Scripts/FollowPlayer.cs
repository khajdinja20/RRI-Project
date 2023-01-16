using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] float multiplier = 1.5f;

    Vector3 PlayerPos;
    Vector3 MousePos;
    Vector3 CameraPos;

    GameObject playerObj;
    Camera cameraObj;
    // Start is called before the first frame update
    void Start()
    {
        cameraObj = Camera.main;
    }

    private void LateUpdate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        MousePos = Input.mousePosition;
        PlayerPos = playerObj.transform.position;

        MousePos = cameraObj.ScreenToWorldPoint(MousePos);
        CameraPos = MousePos - PlayerPos;
        CameraPos = CameraPos.normalized * multiplier;

        PlayerPos.z = -10;
        CameraPos.z = -10;

        cameraObj.transform.position = PlayerPos + CameraPos;
    }
}
