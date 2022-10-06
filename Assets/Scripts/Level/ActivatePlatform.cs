using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ActivatePlatform : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    private bool _onPressurePlate = false;
    
    public float OpenSpeed = 5f;
    public float ClosingSpeed = 5f;

    
    private void Update()
    {
        if (_onPressurePlate)
            EndPos();

        if (!_onPressurePlate)
            StartPos();
    }

    
    
    private void OnTriggerStay2D(Collider2D col) => _onPressurePlate = true;

    
    private void OnTriggerExit2D(Collider2D other) => _onPressurePlate = false;

    
    private void EndPos()
    {
        _platform.transform.position = Vector3.MoveTowards(_platform.transform.position, _endPos.transform.position,
            OpenSpeed * Time.deltaTime);
    }
    
    
    
    private void StartPos()
    {
        _platform.transform.position = Vector3.MoveTowards(_platform.transform.position, _startPos.transform.position, 
            ClosingSpeed * Time.deltaTime);
    }
}
