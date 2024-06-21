using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Config : MonoBehaviour
{
    [SerializeField] private InputField userNameInputField;
    [SerializeField] private Text userNameDisplay;

    private string googleFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdM1wSRq1QdPUoUMrH_mVV0f8aLI7TxbGiffYF2T7NRARlTVw/formResponse";

    private StatusGreadeUp statusGreadeUp;

    private void Start()
    {
        // 現在のユーザー名を表示
        userNameDisplay.text = PlayerPrefs.GetString("UserName", "User" + PlayerPrefs.GetString("UID", "0000").Substring(4, 4));

        // StatusGreadeUpコンポーネントの参照を取得
        statusGreadeUp = FindObjectOfType<StatusGreadeUp>();

        // データの定期保存
        InvokeRepeating("SaveData", 300f, 300f);

        // ゲーム終了時にデータを保存
        Application.quitting += SaveData;
    }

    public void ChangeUserName()
    {
        string newUserName = userNameInputField.text;
        PlayerPrefs.SetString("UserName", newUserName);
        userNameDisplay.text = newUserName;
        statusGreadeUp.SaveData(); // StatusGreadeUpのデータを保存
        StartCoroutine(UpdateUserDataInSheet(newUserName));
    }

    private IEnumerator UpdateUserDataInSheet(string newUserName)
    {
        string uid = PlayerPrefs.GetString("UID");
        int rank = PlayerPrefs.GetInt("Rank", 1);
        int money = PlayerPrefs.GetInt("Money", 0);
        int stone = PlayerPrefs.GetInt("Stone", 0);
        string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        string url = $"{googleFormUrl}?entry.1054435270={uid}&entry.342211498={newUserName}&entry.66232749={rank}&entry.69852853={money}&entry.2135115026={stone}&entry.298013753={timestamp}";

        Debug.Log("Sending data to: " + url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error updating user data: " + www.error);
        }
        else
        {
            Debug.Log("User data updated successfully: " + www.downloadHandler.text);
        }
    }

    public void SaveData()
    {
        // StatusGreadeUpのデータを保存
        statusGreadeUp.SaveData();
        StartCoroutine(SaveUserData());
    }

    private IEnumerator SaveUserData()
    {
        string userName = PlayerPrefs.GetString("UserName", "User" + PlayerPrefs.GetString("UID", "0000").Substring(4, 4));
        string uid = PlayerPrefs.GetString("UID");
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
}
