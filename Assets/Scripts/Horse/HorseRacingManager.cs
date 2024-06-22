using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HorseRacingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject horseRacingPanel;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Text moneyText;

    private string playerName;
    private int money;

    private void Start()
    {
        // MainSceneで保存されたユーザー名をPlayerPrefsから取得
        playerName = PlayerPrefs.GetString("UserName", "User" + PlayerPrefs.GetString("UID", "0000").Substring(4, 4));
        money = PlayerPrefs.GetInt("Money", 0);
        moneyText.text = "Money: $" + money;

        joinRoomButton.onClick.AddListener(OnJoinRoomButtonClicked);
        leaveRoomButton.onClick.AddListener(OnLeaveRoomButtonClicked);

        horseRacingPanel.SetActive(true);
    }

    private void OnJoinRoomButtonClicked()
    {
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainScene");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // ロビーに参加した後にルームを作成または参加
        Debug.Log("Joined Lobby. Now joining or creating room...");
        StartCoroutine(JoinOrCreateRoom());
    }

    private IEnumerator JoinOrCreateRoom()
    {
        // 確実にロビーに参加してからルームを作成または参加するために少し待つ
        yield return new WaitForSeconds(0.5f);

        string roomName = "Room_" + (System.DateTime.Now.Minute / 5).ToString();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // ルーム参加後のUI更新
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered: " + newPlayer.NickName);
        // プレイヤー参加後のUI更新
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left: " + otherPlayer.NickName);
        // プレイヤー退出後のUI更新
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause.ToString());
        // ネットワーク切断後のUI更新
    }

    public void PlaceBet(int horseIndex, int betAmount)
    {
        // ベットロジックを実装
        money -= betAmount;
        PlayerPrefs.SetInt("Money", money);
        moneyText.text = "Money: $" + money;
    }
}
