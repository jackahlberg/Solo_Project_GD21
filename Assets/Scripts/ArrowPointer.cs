using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public SpriteRenderer sprite;
    private void RotateArrow()
    {
        Vector2 arrowPosition = transform.position;
        Vector2 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseDirection - arrowPosition;
        transform.right = direction;
    }
    void Update()
    {
        RotateArrow();
    }

}
