using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public SpriteRenderer ArrowSprite;
    
    
    private void Update() => RotateArrow();

    
    private void RotateArrow()
    {
        Vector2 arrowPosition = transform.position;
        Vector2 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseDirection - arrowPosition;
        transform.right = direction;
    }
}
