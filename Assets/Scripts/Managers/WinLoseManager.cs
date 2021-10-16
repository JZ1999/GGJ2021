using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    [Header("Sound Validators")]
    [Range(100, 200)]
    public float maxSoundForAwaking; // how much sound is needed for awaking;
    [Range(100, 500)]
    public float maxTimeForAwaking; 
    private float soundForAwaking; // current sound value

    [Space]
    [Header("UI")]
    public Image uiBarFront;
    public GameObject uiLoseVictims;
    public GameObject uiLoseCapture;
	public TMP_Text timeText;

    [Space]
    [Header("Multiplayer")]
    public int multiplayerSceneIndex = 0;
    [Range(1, 10)]
    public int timeForRestart = 5;
    private bool isUIActive = false;

    // Start is called before the first frame update
    void Start()
    {
        uiBarFront.fillAmount = 0f;
    }


    private void FixedUpdate()
    {
        isUIActive = !uiLoseCapture.activeSelf && !uiLoseVictims.activeSelf;
        if (maxTimeForAwaking >= 0)
            maxTimeForAwaking -= Time.deltaTime;
        else
            maxTimeForAwaking = 0;
        
		timeText.text = Mathf.Round(maxTimeForAwaking).ToString();
        if(maxTimeForAwaking <= 0 && isUIActive )
        {
            LoseVictims();
        }
    }


    public void AddSum(float added)
    {

        soundForAwaking += added;
        uiBarFront.fillAmount = soundForAwaking / maxSoundForAwaking;

        if (soundForAwaking >= maxSoundForAwaking && isUIActive)
        {
            Awaking();
        }
    }


    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(timeForRestart);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(multiplayerSceneIndex);
    }

    public void Awaking()
    {
        uiLoseCapture.SetActive(true);
        StartCoroutine(RestartGame());
    }

    public void LoseVictims()
    {
        uiLoseVictims.SetActive(true);
        StartCoroutine(RestartGame());
    }
}
