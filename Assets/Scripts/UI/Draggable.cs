using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IDragHandler
{
    RectTransform m_transform = null;

    // Use this for initialization
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_transform.position += new Vector3(eventData.delta.x * 0.02f, eventData.delta.y * 0.02f);
        // magic : add zone clamping if's here.
    }
}