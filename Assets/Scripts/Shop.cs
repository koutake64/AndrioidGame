using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public Text stoneText;
    public int stone = 10;
    public int stonePrice = 120;

    public InputField giftCodeInput;
   // public GameObject giftCodePanel;
    public Text giftCodeResultText;

    private string googleSheetUrl = "https://docs.google.com/spreadsheets/d/14lPK2sPa89Z2ah_KekS2tpGIwURTWlc0E8BGh_j8KB8/export?format=csv";
    private string googleFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLSc_qxw38hiqJGj4UTCwJkpDyqwNJIaG8JuRnsYATEQ7CmCSSg/formResponse";

    private Dictionary<string, int> itemInventory = new Dictionary<string, int>();
    private HashSet<string> usedCodes = new HashSet<string>();

    [SerializeField] private GameObject Scroll;
    [SerializeField] private GameObject ItemBuyPanel;
    [SerializeField] private GameObject CharacterGatyaPanel;
    [SerializeField] private GameObject BukiPanel;
    [SerializeField] private GameObject GiftCodePanel;
    [SerializeField] private GameObject HelpPanel;

    private void Start()
    {
        // 初期値を設定
        itemInventory["石"] = stone;
        UpdateStoneUI();
        AllFalse();
        Scroll.SetActive(true);
    }

    private void AllFalse()
    {
        Scroll.SetActive(false);
        ItemBuyPanel.SetActive(false);
        CharacterGatyaPanel.SetActive(false);
        BukiPanel.SetActive(false);
        GiftCodePanel.SetActive(false);
        HelpPanel.SetActive(false);
    }

    public void PurchaseStone()
    {
        itemInventory["石"]++;
        UpdateStoneUI();
    }


    public void RedeemGiftCode()
    {
        if (usedCodes.Contains(giftCodeInput.text))
        {
            giftCodeResultText.text = "このギフトコードは既に使用されています。";
        }
        else
        {
            StartCoroutine(CheckGiftCode(giftCodeInput.text));
        }
    }

    private IEnumerator CheckGiftCode(string code)
    {
        UnityWebRequest www = UnityWebRequest.Get(googleSheetUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            giftCodeResultText.text = "ネットワークエラーが発生しました。";
        }
        else
        {
            string[] rows = www.downloadHandler.text.Split('\n');
            bool codeValid = false;

            for (int i = 1; i < rows.Length; i++) // Skip the header row
            {
                string[] columns = rows[i].Split(',');
                if (columns[0] == code)
                {
                    string itemName = columns[1];
                    int quantity = int.Parse(columns[2]);
                    int remainingUses = int.Parse(columns[3]);
                    string validUntil = columns[4];

                    if (remainingUses <= 0)
                    {
                        giftCodeResultText.text = "このギフトコードは使用回数を超えています。";
                        codeValid = true;
                        break;
                    }

                    if (itemName == "Stone")
                    {
                        itemName = "石";
                    }

                    if (!itemInventory.ContainsKey(itemName))
                    {
                        giftCodeResultText.text = "存在しないアイテムです。";
                        codeValid = true;
                        break;
                    }

                    // Update the inventory
                    if (itemInventory.ContainsKey(itemName))
                    {
                        itemInventory[itemName] += quantity;
                    }
                    else
                    {
                        itemInventory[itemName] = quantity;
                    }

                    // Decrease the remaining uses in the Google Sheets
                    remainingUses--;
                    StartCoroutine(UpdateGoogleSheet(code, remainingUses));

                    giftCodeResultText.text = $"{itemName} を {quantity} 個入手しました。";
                    codeValid = true;
                    usedCodes.Add(code);
                    UpdateStoneUI(); // 確実にUIを更新
                    break;
                }
            }

            if (!codeValid)
            {
                giftCodeResultText.text = "ギフトコードが無効です。";
            }
        }
    }

    private IEnumerator UpdateGoogleSheet(string code, int remainingUses)
    {
        string entryCode = "entry.2091533468"; // ギフトコードのエントリID
        string entryRemainingUses = "entry.1672600359"; // 使用可能回数のエントリID

        string updateUrl = $"{googleFormUrl}?{entryCode}={code}&{entryRemainingUses}={remainingUses}";
        UnityWebRequest www = UnityWebRequest.Get(updateUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error updating Google Sheet");
        }
        else
        {
            Debug.Log("Google Sheet updated successfully");
        }
    }

    private void UpdateStoneUI()
    {
        if (stoneText != null)
        {
            stoneText.text = $"石: {itemInventory["石"]}個";
            // Display other items in the inventory
            foreach (KeyValuePair<string, int> item in itemInventory)
            {
                if (item.Key != "石")
                {
                    stoneText.text += $"\n{item.Key}: {item.Value}個";
                }
            }
        }
        else
        {
            Debug.LogWarning("stoneText is not set in the Inspector.");
        }
    }

    public void RetrunMenu()
    {
        AllFalse();
        Scroll.SetActive(true);
    }

    public void OpenItemBuy()
    {
        AllFalse();
        ItemBuyPanel.SetActive(true);
    }

    public void OpenKyaraGatya()
    {
        AllFalse();
        CharacterGatyaPanel.SetActive(true);
    }

    public void OpenBuki()
    {
        AllFalse();
        BukiPanel.SetActive(true);
    }

    public void OpenGift()
    {
        AllFalse();
        GiftCodePanel.SetActive(true);
    }

    public void OpenHelp()
    {
        AllFalse();
        HelpPanel.SetActive(true);
    }
}
