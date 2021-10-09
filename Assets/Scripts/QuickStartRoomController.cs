using Photon.Pun;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{

	public int multiplayerSceneIndex;

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
		StartGame();
	}

	private void StartGame()
	{
		Debug.Log("Starting Game");
		PhotonNetwork.LoadLevel(multiplayerSceneIndex);	
	}
}
