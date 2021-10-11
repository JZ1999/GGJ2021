using Photon.Pun;
using UnityEngine;
using TMPro;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{

	public int multiplayerSceneIndex;
	private bool joinedRoom = false;
	public QuickStartLobbyController quickStartLobby;
	public TMP_Text displayPlayers;
	private bool startedGame = false;

	private void Update()
	{
		if (joinedRoom && !startedGame)
		{
			displayPlayers.text = PhotonNetwork.CurrentRoom.Players.Count + "/" + quickStartLobby.roomSize + " Players";
			if (PhotonNetwork.CurrentRoom.Players.Count == quickStartLobby.roomSize)
			{
				StartGame();
				startedGame = true;
			}
		}
		
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		Debug.Log("Joined room");
		joinedRoom = true;
	}

	private void StartGame()
	{
		Debug.Log("Starting Game");
		PhotonNetwork.LoadLevel(multiplayerSceneIndex);	
	}
}
