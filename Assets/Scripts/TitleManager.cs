using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private string nextSceneName;
    [SerializeField] private InputField uidInputField;
    [SerializeField] private Text uidDisplay;
    [SerializeField] private Button recoverButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button saveButton; // データ保存ボタンを追加

    private string uid;
    private string googleSheetUrl = "https://docs.google.com/spreadsheets/d/1sL7pPnmzV3DQsrOTeY_rYVJXVublSKK1ivlQMPoBnhY/export?format=csv";
    private string googleFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdM1wSRq1QdPUoUMrH_mVV0f8aLI7TxbGiffYF2T7NRARlTVw/formResponse";

    private StatusGreadeUp statusGreadeUp;

    private void Start()
    {
        ClearUI();
        StartPanel.SetActive(true);

        // UIDの生成または読み込み
        if (PlayerPrefs.HasKey("UID"))
        {
            uid = PlayerPrefs.GetString("UID");
        }
        else
        {
            uid = GenerateUID();
            PlayerPrefs.SetString("UID", uid);
        }

        // UIDの表示
        uidDisplay.text = "UID: " + (string.IsNullOrEmpty(uid) ? "Null" : uid);

        // ボタンのクリックイベントを設定
        saveButton.onClick.AddListener(SaveData);
        recoverButton.onClick.AddListener(RecoverData); // データ復旧ボタン
        deleteButton.onClick.AddListener(DeleteLocalData); // データ削除ボタン

        // StatusGreadeUpコンポーネントの参照を取得
        statusGreadeUp = FindObjectOfType<StatusGreadeUp>();
    }

    private void ClearUI()
    {
        StartPanel.SetActive(false);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private string GenerateUID()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    private void SaveData()
    {
        // StatusGreadeUpのデータを保存
        statusGreadeUp.SaveData();

        StartCoroutine(SaveUserData());
    }

    private IEnumerator SaveUserData()
    {
        string userName = PlayerPrefs.GetString("UserName", "User" + uid.Substring(4, 4));
        int rank = PlayerPrefs.GetInt("Rank", 1);
        int money = PlayerPrefs.GetInt("Money", 0);
        int stone = PlayerPrefs.GetInt("Stone", 0);
        string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        string url = $"{googleFormUrl}?entry.1054435270={uid}&entry.342211498={userName}&entry.66232749={rank}&entry.69852853={money}&entry.2135115026={stone}&entry.298013753={timestamp}";

        Debug.Log("Sending data to: " + url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error saving data: " + www.error);
        }
        else
        {
            Debug.Log("Data saved successfully: " + www.downloadHandler.text);
        }
    }

    public void RecoverData()
    {
        string inputUid = uidInputField.text;
        StartCoroutine(LoadUserData(inputUid));
    }

    private IEnumerator LoadUserData(string inputUid)
    {
        UnityWebRequest www = UnityWebRequest.Get(googleSheetUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error loading data: " + www.error);
        }
        else
        {
            string[] rows = www.downloadHandler.text.Split('\n');
            bool found = false;
            DateTime latestTimestamp = DateTime.MinValue;
            string[] latestData = null;

            for (int i = 1; i < rows.Length; i++) // Skip header row
            {
                string[] columns = rows[i].Split(',');

                if (columns[1] == inputUid) // UIDが列1にあるため、columns[1]に変更
                {
                    DateTime timestamp = DateTime.Parse(columns[0]);
                    if (timestamp > latestTimestamp)
                    {
                        latestTimestamp = timestamp;
                        latestData = columns;
                        found = true;
                    }
                }
            }

            if (found && latestData != null)
            {
                PlayerPrefs.SetString("UID", latestData[1]);
                PlayerPrefs.SetString("UserName", latestData[2]);
                PlayerPrefs.SetInt("Rank", int.Parse(latestData[3]));
                PlayerPrefs.SetInt("Money", int.Parse(latestData[4]));
                PlayerPrefs.SetInt("Stone", int.Parse(latestData[5]));
                uidDisplay.text = "UID: " + latestData[1];
                statusGreadeUp.LoadData(); // StatusGreadeUpのデータをロード
                Debug.Log("Data recovered successfully");
            }
            else
            {
                Debug.Log("No data found for UID: " + inputUid);
            }
        }
    }

    public void DeleteLocalData()
    {
        PlayerPrefs.DeleteKey("UID");
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("Rank");
        PlayerPrefs.DeleteKey("Money");
        PlayerPrefs.DeleteKey("Stone");
        uidDisplay.text = "UID: Null";
        Debug.Log("Local data deleted");
    }
}
