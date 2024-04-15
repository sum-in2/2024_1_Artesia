using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public RectInt screenRect;

    public GameObject player;

    private void Update(){
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        this.transform.Translate(moveVector);
        CalculateScreenBorderRect();
    }

    void CalculateScreenBorderRect(){
        Camera cam = Camera.main;
        if (cam == null){
            Debug.LogError("Camera reference not set!");
            return;
        }

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.7f, Screen.height, cam.nearClipPlane));

        int xMin = Mathf.FloorToInt(bottomLeft.x);
        int yMin = Mathf.FloorToInt(bottomLeft.y);
        int width = Mathf.CeilToInt(topRight.x) - xMin;
        int height = Mathf.CeilToInt(topRight.y) - yMin;

        screenRect = new RectInt(xMin, yMin, width, height);
    }
    
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(screenRect.xMin, screenRect.yMin, 0);
        Vector3 bottomRight = new Vector3(screenRect.xMax, screenRect.yMin, 0);
        Vector3 topRight = new Vector3(screenRect.xMax, screenRect.yMax, 0);
        Vector3 topLeft = new Vector3(screenRect.xMin, screenRect.yMax, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}