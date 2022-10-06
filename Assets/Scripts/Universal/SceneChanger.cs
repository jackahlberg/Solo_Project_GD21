using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LevelOne() => SceneManager.LoadScene(1);
    

    public void LevelTwo() => SceneManager.LoadScene(2);

    
    public void ExitGame() => Application.Quit();
    
    
    public void Respawn() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
