using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Vector2 startPosition;

    private Queue<GameObject> dropZones = new Queue<GameObject>();

    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
        if (!dropZones.Contains(dropZone))
        {
            dropZones.Enqueue(dropZone);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (dropZones.Contains(collision.gameObject))
        {
            dropZones.Dequeue();
        }
        if (dropZones.Count > 0)
        {
            dropZone = dropZones.Peek();
        }
        else 
        { 
            dropZone = null;
            isOverDropZone = false;
        }
    }
}
