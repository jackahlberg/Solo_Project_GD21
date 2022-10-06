using UnityEngine;

public class ActivateGameObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < _objects.Length; i++)
                _objects[i].SetActive(true);
        }
    }
}
