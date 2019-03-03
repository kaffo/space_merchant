using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    public float minXPos = -25f;
    public float maxXPos = 25f;
    public float minYPos = -25f;
    public float maxYPos = 25f;

    public float maxZoom = 20f;

    public bool allowStartMouseDrag = true;

    float mainSpeed = 10.0f; //regular speed
    float zoomSpeed = 0.5f;
    float shiftAdd = 10.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 20.0f; //Maximum speed when holdin gshift
    float camSens = 0.02f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    private bool allowDrag = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMouse = Input.mousePosition;
            allowDrag = allowStartMouseDrag;
        }

        if (allowDrag && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMouse;
            transform.Translate(-delta.x * camSens, -delta.y * camSens, 0);
            lastMouse = Input.mousePosition;
        }

        float mouseZoom = Input.mouseScrollDelta.y;
        float oldZoom = gameObject.GetComponent<Camera>().orthographicSize;
        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(oldZoom + -(mouseZoom * zoomSpeed), 0.5f, maxZoom);

        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        transform.Translate(p);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXPos, maxXPos), Mathf.Clamp(transform.position.y, minYPos, maxYPos), transform.position.z);
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}