using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject StartPanal;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        ClearUI();
        StartPanal.SetActive(true);
    }

    public void ClearUI()
    {
        StartPanal.SetActive(false);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);

    }


}
