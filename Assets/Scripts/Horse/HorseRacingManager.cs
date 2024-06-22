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
        // MainScene�ŕۑ����ꂽ���[�U�[����PlayerPrefs����擾
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
        // ���r�[�ɎQ��������Ƀ��[�����쐬�܂��͎Q��
        Debug.Log("Joined Lobby. Now joining or creating room...");
        StartCoroutine(JoinOrCreateRoom());
    }

    private IEnumerator JoinOrCreateRoom()
    {
        // �m���Ƀ��r�[�ɎQ�����Ă��烋�[�����쐬�܂��͎Q�����邽�߂ɏ����҂�
        yield return new WaitForSeconds(0.5f);

        string roomName = "Room_" + (System.DateTime.Now.Minute / 5).ToString();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // ���[���Q�����UI�X�V
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered: " + newPlayer.NickName);
        // �v���C���[�Q�����UI�X�V
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left: " + otherPlayer.NickName);
        // �v���C���[�ޏo���UI�X�V
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause.ToString());
        // �l�b�g���[�N�ؒf���UI�X�V
    }

    public void PlaceBet(int horseIndex, int betAmount)
    {
        // �x�b�g���W�b�N������
        money -= betAmount;
        PlayerPrefs.SetInt("Money", money);
        moneyText.text = "Money: $" + money;
    }
}
