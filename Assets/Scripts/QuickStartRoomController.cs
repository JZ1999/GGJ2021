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
		int currentPlayerCount = 0;
		if (joinedRoom && !startedGame)
		{
			if(PhotonNetwork.CurrentRoom?.Players?.Count != null)
			{
				currentPlayerCount = PhotonNetwork.CurrentRoom.Players.Count;
			}
			displayPlayers.text = currentPlayerCount + "/" + quickStartLobby.roomSize + " Players";
			if (PhotonNetwork.CurrentRoom.Players.Count == quickStartLobby.roomSize)
			{
				StartGame();
				startedGame = true;
			}
		}
		displayPlayers.text = currentPlayerCount + "/" + quickStartLobby.roomSize + " Players";
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
		//PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.LoadLevel(multiplayerSceneIndex);	
	}
}
