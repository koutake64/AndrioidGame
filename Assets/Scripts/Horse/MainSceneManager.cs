using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject horseRacingButton; // ���n�{�^���̎Q��

    private void Start()
    {
        horseRacingButton.GetComponent<Button>().onClick.AddListener(LoadHorseScene);
    }

    private void LoadHorseScene()
    {
        SceneManager.LoadScene("HorseScene");
    }
}
