using UnityEngine;
using UnityEngine.UI;

public class StatusGreadeUp : MonoBehaviour
{
    // Money関連
    public Text moneyText;
    public Text moneyLevelText;
    public Text moneyCostText;

    private int money = 0;
    private int moneyLevel = 1;
    private float moneyIncreaseRate = 10f;
    private int moneyUpgradeCost = 100;

    // Exp関連
    public Text expLevelText;
    public Text expCostText;
    public Text rankText;

    private int exp = 0;
    private int expLevel = 1;
    private float expIncreaseRate = 10f;
    private int expUpgradeCost = 100;
    private int rank = 1;
    private int nextRankExp = 100;

    private float timer = 0f;

    private void Start()
    {
        LoadData();
        UpdateMoneyUI();
        UpdateExpUI();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            money += (int)(moneyLevel * moneyIncreaseRate);
           // Debug.Log("Money: " + money); // デバッグメッセージ
            exp += (int)(expLevel * expIncreaseRate);
            CheckRankUp();
            timer = 0f;
            UpdateMoneyUI();
            UpdateExpUI();
        }
    }

    public void UpgradeMoneyLevel()
    {
        if (money >= moneyUpgradeCost)
        {
            money -= moneyUpgradeCost;
            moneyLevel++;
            moneyIncreaseRate *= 1.05f;
            moneyUpgradeCost = Mathf.RoundToInt(moneyUpgradeCost * 1.1f);
            UpdateMoneyUI();
        }
    }

    public void UpgradeExpLevel()
    {
        if (exp >= expUpgradeCost)
        {
            exp -= expUpgradeCost;
            expLevel++;
            expIncreaseRate *= 1.05f;
            expUpgradeCost = Mathf.RoundToInt(expUpgradeCost * 1.1f);
            UpdateExpUI();
        }
    }

    private void CheckRankUp()
    {
        if (exp >= nextRankExp)
        {
            rank++;
            exp -= nextRankExp;
            nextRankExp = Mathf.RoundToInt(nextRankExp * 2.5f);
        }
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + money;
        moneyLevelText.text = "Lv: " + moneyLevel;
        moneyCostText.text = "次回必要money: $" + moneyUpgradeCost;
    }

    private void UpdateExpUI()
    {
        expLevelText.text = "Lv: " + expLevel;
        expCostText.text = "次回必要money: " + expUpgradeCost;
        rankText.text = "Rank: " + rank;
    }

    public void CharacterBOC()
    {
        money += 1;
        UpdateMoneyUI();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("MoneyLevel", moneyLevel);
        PlayerPrefs.SetFloat("MoneyIncreaseRate", moneyIncreaseRate);
        PlayerPrefs.SetInt("MoneyUpgradeCost", moneyUpgradeCost);

        PlayerPrefs.SetInt("Exp", exp);
        PlayerPrefs.SetInt("ExpLevel", expLevel);
        PlayerPrefs.SetFloat("ExpIncreaseRate", expIncreaseRate);
        PlayerPrefs.SetInt("ExpUpgradeCost", expUpgradeCost);
        PlayerPrefs.SetInt("Rank", rank);
        PlayerPrefs.SetInt("NextRankExp", nextRankExp);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
            moneyLevel = PlayerPrefs.GetInt("MoneyLevel");
            moneyIncreaseRate = PlayerPrefs.GetFloat("MoneyIncreaseRate");
            moneyUpgradeCost = PlayerPrefs.GetInt("MoneyUpgradeCost");

            exp = PlayerPrefs.GetInt("Exp");
            expLevel = PlayerPrefs.GetInt("ExpLevel");
            expIncreaseRate = PlayerPrefs.GetFloat("ExpIncreaseRate");
            expUpgradeCost = PlayerPrefs.GetInt("ExpUpgradeCost");
            rank = PlayerPrefs.GetInt("Rank");
            nextRankExp = PlayerPrefs.GetInt("NextRankExp");
        }
    }
}
