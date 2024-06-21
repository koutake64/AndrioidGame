using UnityEngine;

public class SelectGames : MonoBehaviour
{
    //====== PANEL =====
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject EquipmentPanel;
    [SerializeField] private GameObject SelectPanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject ConfigPanel;

    private void Start()
    {
        CloseUI();
        GamePanel.SetActive(true);
    }

    private void CloseUI()
    {
        GamePanel.SetActive(false);
        EquipmentPanel.SetActive(false);
        SelectPanel.SetActive(false);
        ShopPanel.SetActive(false);
        ConfigPanel.SetActive(false);
    }

    public void OpenGamePanel()
    {
        CloseUI();
        GamePanel.SetActive(true);
    }

    public void OpenEquipmentPanel()
    {
        CloseUI();
        EquipmentPanel.SetActive(true);
    }

    public void OpenSelectPanel()
    {
        CloseUI();
        SelectPanel.SetActive(true);
    }

    public void OpenShopPanel()
    {
        CloseUI();
        ShopPanel.SetActive(true);
    }

    public void OpenConfigPanel()
    {
        CloseUI();
        ConfigPanel.SetActive(true);
    }
}
