using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    [SerializeField] private string GameWorldSceneName;

    public void WorldSelectBOC()
    {
        SceneManager.LoadScene(GameWorldSceneName);
    }
}
