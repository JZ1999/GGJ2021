using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class GameSetupController : MonoBehaviourPun, IPunObservable
{
	[Header("Multiplayer")]
	public PhotonView photonView;
	public enum prefabsChoices { creatureCapturer, creature, none };
	public prefabsChoices playerPrefab = prefabsChoices.none;

	[Space]
	[Header("Spawns")]
	public Transform victimSpawn;
	public Transform capturerSpawn;

	[Space]
	[Header("Props")]
	[Tooltip("The props in the match")]
	public GameObject[] props;

	[Space]
	[Header("Managers")]
	[SerializeField]
	private WinLoseManager winLoseManager;

	private IDictionary<GameObject, bool> playersReady;




	// Start is called before the first frame update
	void Start()
	{
		photonView = gameObject.AddComponent<PhotonView>();
		photonView.ViewID = 1;
		CreatePlayer();
		PhotonNetwork.CurrentRoom.IsVisible = false; // Lock room so no more join
	}

	public bool arePlayersReady()
	{
		foreach(bool playerReady in playersReady.Values)
		{
			if (!playerReady)
				return false;
		}
		return true;
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
		switch (type)
		{
			case "catch":
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<SimpleSampleCharacterControl>().SetTimeAnda();
				break;
			case "fillbar":
				FillBarInfo fillbar = JsonUtility.FromJson<FillBarInfo>(json);
				winLoseManager.SetSoundForAwaking(fillbar.fillAmount);
				break;
			case "teleport":
				PhotonView.Find(Int32.Parse(viewID)).gameObject.transform.position = JsonUtility.FromJson<Vector3>(json);
				break;
			case "movement":
				InputsInfo inputs = JsonUtility.FromJson<InputsInfo>(json);
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<SimpleSampleCharacterControl>().TankUpdate(inputs.horizontal, inputs.vertical);
				break;
			case "rotation":
				Vector3 newVector = JsonUtility.FromJson<Vector3>(json);
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<PlayerRotate>().ApplyRotation(newVector);
				break;
			case "jump":
				PhotonView.Find(Int32.Parse(viewID)).gameObject.GetComponent<SimpleSampleCharacterControl>().m_jumpInput = true;
				break;
			case "prop":
				PropInfo propInfo = JsonUtility.FromJson<PropInfo>(json);
				foreach (GameObject prop in props)
				{
					if(prop.GetComponent<Prop>().propName == propInfo.name)
					{
						if(propInfo.interactionType == "activate")
						{
							prop.GetComponent<Prop>().interactWithProp();
						} else if(propInfo.interactionType == "deactivate")
						{
							prop.GetComponent<Prop>().deactivateProp();
						}
					}
				}
				break;
			default:
				Debug.LogWarningFormat("Incorrect message type: {0} from {1}", type, viewID);
				break;
		}
	}

	[PunRPC]
	void MasterClientMessage(Photon.Realtime.Player sender, string type, string json, int viewID)
	{
		switch (type)
		{
			case "loaded":
				for(int i = 0; i < playersReady.Keys.Count; i++) {
					GameObject player = new List<GameObject>(playersReady.Keys)[i];
					if(player.GetComponent<PhotonView>().ViewID == viewID)
					{
						playersReady[player] = true;
					}
				}
				break;
			default:
				Debug.LogWarningFormat("Incorrect message type: {0} from {1}", type, viewID);
				break;
		}
	}

	private void CreatePlayer()
	{
		int playersInRoom = PhotonNetwork.CurrentRoom.Players.Keys.Count;
		string prefabName = PhotonNetwork.IsMasterClient ? "Creature Capturer" : "Creature";
		Transform spawn = PhotonNetwork.IsMasterClient ? capturerSpawn : victimSpawn;
#if UNITY_EDITOR
		if(playerPrefab == prefabsChoices.creature)
        {
			prefabName = "Creature";
			spawn = victimSpawn;

		}
        else if(playerPrefab == prefabsChoices.creatureCapturer)
        {
			prefabName = "Creature Capturer";
			spawn = capturerSpawn;
		}
#endif
		GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), spawn.position, Quaternion.identity);
		player.GetComponent<SimpleSampleCharacterControl>().gameSetup = this;
		//player.GetComponent<PlayerRotate>().gameSetup = this;
		//player.GetComponent<Jump>().gameSetup = this;
		if (player.GetComponentInChildren<CanCaptureVictim>())
		{
			player.GetComponentInChildren<CanCaptureVictim>().gameSetup = this;
		}

		if (PhotonNetwork.IsMasterClient)
		{
			playersReady = new Dictionary<GameObject, bool>(PhotonNetwork.CurrentRoom.PlayerCount);
			playersReady.Add(player, false);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

	public void Destroy(int viewID)
	{
		PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
	}
}
