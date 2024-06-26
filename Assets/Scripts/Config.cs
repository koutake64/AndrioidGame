using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Config : MonoBehaviour
{
    [SerializeField] private InputField userNameInputField;
    [SerializeField] private Text userNameDisplay;
    [SerializeField] private GameObject Scroll;
    [SerializeField] private GameObject InformationPanel;
    [SerializeField] private GameObject LoginBonusPanel;
    [SerializeField] private GameObject UserInformationPanel;
    [SerializeField] private GameObject BackUpPanel;
    [SerializeField] private GameObject SupportPanel;
    [SerializeField] private GameObject HelpPanel;
    [SerializeField] private GameObject RiyoukiyakuPanel;
    [SerializeField] private GameObject KurejittoPanel;
    [SerializeField] private GameObject AccountDeletePanel;
    [SerializeField] private Text backupStatusText; // バックアップステータス表示用テキスト

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

        AllFalse();
        Scroll.SetActive(true);
    }

    private void AllFalse()
    {
        Scroll.SetActive(false);
        InformationPanel.SetActive(false);
        LoginBonusPanel.SetActive(false);
        UserInformationPanel.SetActive(false);
        BackUpPanel.SetActive(false);
        SupportPanel.SetActive(false);
        HelpPanel.SetActive(false);
        RiyoukiyakuPanel.SetActive(false);
        KurejittoPanel.SetActive(false);
        AccountDeletePanel.SetActive(false);
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

    public void ReturnTitle()
    {
        // データを保存
        SaveData();
        // タイトルシーンに遷移
        SceneManager.LoadScene("Title");
    }

    public void RetrunMenu()
    {
        AllFalse();
        Scroll.SetActive(true);
    }

    public void OpenInformation()
    {
        AllFalse();
        InformationPanel.SetActive(true);
    }

    public void OpenLoginBonus()
    {
        AllFalse();
        LoginBonusPanel.SetActive(true);
    }

    public void OpenUserInfomation()
    {
        AllFalse();
        UserInformationPanel.SetActive(true);
    }

    public void OpenBackUp()
    {
        AllFalse();
        BackUpPanel.SetActive(true);
    }

    public void OpenSupport()
    {
        AllFalse();
        SupportPanel.SetActive(true);
    }

    public void OpenHelp()
    {
        AllFalse();
        HelpPanel.SetActive(true);
    }

    public void OpenRiyoukiyaku()
    {
        AllFalse();
        RiyoukiyakuPanel.SetActive(true);
    }

    public void OpenKurejitto()
    {
        AllFalse();
        KurejittoPanel.SetActive(true);
    }

    public void OpenAccountDelete()
    {
        AllFalse();
        AccountDeletePanel.SetActive(true);
    }

    public void BackUpOCB()
    {
        StartCoroutine(BackUpData());
    }

    private IEnumerator BackUpData()
    {
        string userName = PlayerPrefs.GetString("UserName", "User" + PlayerPrefs.GetString("UID", "0000").Substring(4, 4));
        string uid = PlayerPrefs.GetString("UID");
        int rank = PlayerPrefs.GetInt("Rank", 1);
        int money = PlayerPrefs.GetInt("Money", 0);
        int stone = PlayerPrefs.GetInt("Stone", 0);
        string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        string url = $"{googleFormUrl}?entry.1054435270={uid}&entry.342211498={userName}&entry.66232749={rank}&entry.69852853={money}&entry.2135115026={stone}&entry.298013753={timestamp}";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            backupStatusText.text = "バックアップ失敗: " + www.error;
            Debug.LogError("Error backing up data: " + www.error);
        }
        else
        {
            backupStatusText.text = "バックアップ完了: " + timestamp;
            Debug.Log("Data backed up successfully: " + www.downloadHandler.text);
        }
    }

    //===== URLリンク =====
    public void OpenWeb()
    {
        var uri = new System.Uri("https://take1564.com");
        Application.OpenURL(uri.AbsoluteUri);
    }

    public void OpenX()
    {
        var uri = new System.Uri("https://x.com/takesLabo");
        Application.OpenURL(uri.AbsoluteUri);
    }

    public void OpenMail()
    {
        string mailAddress = "take@take1564.com";
        string subject = Uri.EscapeDataString("{変更禁止}お問い合わせ[上手いとこどり]");
        string body = Uri.EscapeDataString(
            "迷惑メールに入る場合や受信しない場合はtake@take1564.comからのメールを許可して下さい。\n" +
            "=== 以下に[UID]を記載の上お問い合わせ内容を記入 ===\n" +
            "UID：\n" +
            "お問い合わせ内容："
        );

        var uri = new System.Uri($"mailto:{mailAddress}?subject={subject}&body={body}");
        Application.OpenURL(uri.AbsoluteUri);
    }
}
