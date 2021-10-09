using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{

	public PhotonView photonView;

	public List<GameObject> players;

	public Transform spawn;

	public GameObject prefab;


	// Start is called before the first frame update
	void Start()
	{
		Debug.Log(gameObject);
		players = new List<GameObject>();
		photonView = gameObject.AddComponent<PhotonView>();
		photonView.ViewID = 1;
		CreatePlayer();
		//Send message
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "test");
	}

	public void SendMessage(string message)
	{
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, message);
	}

	public void SendMessage(string type, string json)
	{
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, type, json);
	}

	[PunRPC]
	void SendChat(Photon.Realtime.Player sender, string debuff)
	{
		
		if (sender.IsLocal)
			return;
		Debug.Log(string.Format("{0} {1} {2} {3} {4} {5}", sender.IsLocal, sender.UserId, sender.IsMasterClient, sender.NickName, sender.HasRejoined, sender.ActorNumber));
		Debug.Log(debuff);
	}

	private void CreatePlayer()
	{
		Debug.Log("Creating Player");
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		
		GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Victim"), spawn.position, Quaternion.identity);

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log(stream);
		Debug.Log(info);
	}
}
