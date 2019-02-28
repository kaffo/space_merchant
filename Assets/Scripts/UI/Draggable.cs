using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IDragHandler
{
    RectTransform m_transform = null;
    private Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float cameraZoom = mainCamera.orthographicSize * 0.0025f;
        m_transform.position += new Vector3(eventData.delta.x * cameraZoom, eventData.delta.y * cameraZoom);
        // magic : add zone clamping if's here.
    }
}