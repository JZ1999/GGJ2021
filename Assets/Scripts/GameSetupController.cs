using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{

	public PhotonView photonView;

	public List<GameObject> players;

	public Transform victimSpawn;
	public Transform capturerSpawn;

	public GameObject prefab;

	public GameObject[] props;


	// Start is called before the first frame update
	void Start()
	{
		players = new List<GameObject>();
		photonView = gameObject.AddComponent<PhotonView>();
		photonView.ViewID = 1;
		CreatePlayer();
	}

	public void SendMessage(string type, string json, string viewID)
	{
		photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, type, json, viewID);
	}

	[PunRPC]
	void SendChat(Photon.Realtime.Player sender, string type, string json, string viewID)
	{

		if (sender.IsLocal)
			return;
		Vector3 direction;
		switch (type)
		{
			case "movement":
				direction = JsonUtility.FromJson<Vector3>(json);
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<JoystickPlayerExample>().ApplyForce(direction);
				break;
			case "rotation":
				Vector3 newVector = JsonUtility.FromJson<Vector3>(json);
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<PlayerRotate>().ApplyRotation(newVector);
				break;
			case "jump":
				direction = JsonUtility.FromJson<Vector3>(json);
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<Jump>().rb.AddForce(direction);
				break;
			case "prop":
				PropInfo propInfo = JsonUtility.FromJson<PropInfo>(json);
				foreach(GameObject prop in props)
				{
					if(prop.GetComponent<Prop>().propName == propInfo.name)
					{
						if(propInfo.interactionType == "activate")
						{
							prop.GetComponent<Prop>().interactWithProp();
						} else if(propInfo.interactionType == "activate")
						{
							prop.GetComponent<Prop>().deactivateProp();
						}
					}
				}
				break;
		}
	}

	private void CreatePlayer()
	{
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		string prefabName = playersInRoom % 2 == 1 ? "Victim" : "Capturer";
		Transform spawn = playersInRoom % 2 == 1 ? victimSpawn : capturerSpawn;
		GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), spawn.position, Quaternion.identity);
		player.GetComponent<JoystickPlayerExample>().gameSetup = this;
		player.GetComponent<PlayerRotate>().gameSetup = this;
		player.GetComponent<Jump>().gameSetup = this;
		if (player.GetComponentInChildren<CanCaptureVictim>())
		{
			player.GetComponentInChildren<CanCaptureVictim>().gameSetup = this;
		}
		

		players.Add(player);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

	public void Destroy(int viewID)
	{
		PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
	}
}
