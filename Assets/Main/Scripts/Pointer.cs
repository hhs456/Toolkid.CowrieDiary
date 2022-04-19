using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pointer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    private Transform dragBase;
    public Canvas canvas;
    public float threshold = 0.5f;
    public float changeBuffer = 0.2f;
    private bool hasReorder = false;
    private bool isHover = false;
    private bool onDrag = false;
    private bool onDragEnd = false;
    private float holdTime = 0;
    private float coldTime = 0;

    void Awake() {
        dragBase = transform.parent;
    }

    void FixedUpdate() {        
        if (isHover) {
            if (Input.touchCount > 0) {
                holdTime += Time.deltaTime;
                onDrag = holdTime > threshold;
                if (onDrag) {
                    coldTime -= Time.deltaTime;
                    dragBase.parent.parent.parent.GetComponent<ScrollRect>().vertical = false;
                    Vector2 position;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        (RectTransform)canvas.transform, Input.GetTouch(0).position, canvas.worldCamera, out position);
                    Drag.current.position = canvas.transform.TransformPoint(position);
                    if (coldTime < 0) {
                        if (Drag.current != Drag.lastPointerEnter && Drag.current == dragBase) {
                            coldTime = changeBuffer;
                            Drag.current.SetSiblingIndex(Drag.lastPointerEnter.GetSiblingIndex());
                            hasReorder = true;
                        }
                    }
                }
            }
            else {
                isHover = false;                
                onDragEnd = onDrag;
                onDrag = false;                
                holdTime = 0;
                dragBase.parent.parent.parent.GetComponent<ScrollRect>().vertical = true;
                dragBase.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
                Drag.lastPointerEnter = null;
            }
        }
        if (onDragEnd) {
            onDragEnd = false;
            dragBase.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
            dragBase.parent.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
            dragBase.localPosition = new Vector2(0f, dragBase.localPosition.y);
            if (hasReorder)
                Event.OnKeepReorder(this, new EntryKeepArgs(transform.parent.GetComponent<EntryKeep>().info));
            hasReorder = false;
        }
    }
    public void OnPointerDown(PointerEventData eventData) {
        isHover = true;
        Drag.current = dragBase;        
    }
    public void OnPointerEnter(PointerEventData eventData) {
        Drag.lastPointerEnter = dragBase;
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (!onDrag) {
            isHover = false;
            holdTime = 0;            
        }
    }    
}
