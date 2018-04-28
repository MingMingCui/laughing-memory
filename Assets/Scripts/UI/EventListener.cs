using UnityEngine.EventSystems;
using UnityEngine;
using System;

public delegate void TriggerDelegate(GameObject go);
public delegate void EventDataDelegate(PointerEventData data);
public delegate void ToggleDelegate(PointerEventData data);

public class EventListener : EventTrigger
{
    /************************************************************************************************************************************
        * ------ https://docs.unity3d.com/ScriptReference/EventSystems.EventTrigger.html -------- *
        * **********************************************************************************************************************************
            OnBeginDrag	Called before a drag is started.
            OnCancel	Called by the EventSystem when a Cancel event occurs.
            OnDeselect	Called by the EventSystem when a new object is being selected.
            OnDrag	Called by the EventSystem every time the pointer is moved during dragging.
            OnDrop	Called by the EventSystem when an object accepts a drop.
            OnEndDrag	Called by the EventSystem once dragging ends.
            OnInitializePotentialDrag	Called by the EventSystem when a drag has been found, but before it is valid to begin the drag.
            OnMove	Called by the EventSystem when a Move event occurs.
            OnPointerClick	Called by the EventSystem when a Click event occurs.
            OnPointerDown	Called by the EventSystem when a PointerDown event occurs.
            OnPointerEnter	Called by the EventSystem when the pointer enters the object associated with this EventTrigger.
            OnPointerExit	Called by the EventSystem when the pointer exits the object associated with this EventTrigger.
            OnPointerUp	Called by the EventSystem when a PointerUp event occurs.
            OnScroll	Called by the EventSystem when a Scroll event occurs.
            OnSelect	Called by the EventSystem when a Select event occurs.
            OnSubmit	Called by the EventSystem when a Submit event occurs.
            OnUpdateSelected	Called by the EventSystem when the object associated with this EventTrigger is updated.
         *  *********************************************************************************************************************************/
    public TriggerDelegate OnClick;
    public EventDataDelegate PointerDown;
    public TriggerDelegate PointerUp;
    public TriggerDelegate PointerEnter;
    public TriggerDelegate PointerExit;

    public static EventListener Get(GameObject go)
    {
        EventListener listener = go.GetComponent<EventListener>();
        if (listener == null) listener = go.AddComponent<EventListener>();
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
            OnClick(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown != null)
            PointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUp != null)
            PointerUp(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (PointerEnter != null)
            PointerEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (PointerExit != null)
            PointerExit(gameObject);
    }

    #region 滑动
    public EventDataDelegate BegineDragEvent;
    public EventDataDelegate DragEvent;
    public EventDataDelegate EndDragEvent;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (BegineDragEvent != null)
            BegineDragEvent(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (DragEvent != null)
            DragEvent(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (EndDragEvent != null)
            EndDragEvent(eventData);
    }
    #endregion
}