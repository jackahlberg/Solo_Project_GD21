using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialImages;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            _tutorialImages.SetActive(true);
    }

    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _tutorialImages.SetActive(false);
            Destroy(_tutorialImages);
        }
    }
}
