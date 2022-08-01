using UnityEngine;

public class UIEnabler : MonoBehaviour
{
    [SerializeField] private GameObject tutorialImages;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            tutorialImages.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialImages.SetActive(false);
            Destroy(tutorialImages);
        }
    }
}
