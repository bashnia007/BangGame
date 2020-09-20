using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;

    private GameObject zoom;

    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        zoom = Instantiate(gameObject, new Vector2(200, 250), Quaternion.identity);
        zoom.transform.SetParent(Canvas.transform, true);
        zoom.layer = LayerMask.NameToLayer("Zoom");
        zoom.GetComponent<Image>().raycastTarget = false;

        RectTransform rect = zoom.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240, 344);
    }

    public void OnHoverExit()
    {
        Destroy(zoom);
    }
}
