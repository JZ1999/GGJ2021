using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseManager : MonoBehaviour
{
    [Range(100, 200)]
    public float maxSoundForAwaking;
    [Range(100, 200)]
    public float maxTimeForAwaking;
    public float soundForAwaking;
    public Image uiBarFront;
    public GameObject uiLoseVictims;
    public GameObject uiLoseCapture;

    // Start is called before the first frame update
    void Start()
    {
        uiBarFront.fillAmount = 0f;
    }

    private void Update()
    {
        maxTimeForAwaking -= Time.deltaTime;
        if(maxTimeForAwaking <= 0)
        {
            LoseVictims();
        }
    }


    public void AddSum(float added)
    {

        soundForAwaking += added;
        uiBarFront.fillAmount = soundForAwaking / maxSoundForAwaking;

        if (soundForAwaking >= maxSoundForAwaking)
        {
            Awaking();
        }
    }

    public void Awaking()
    {
        uiLoseCapture.SetActive(true);
        Debug.Log("se despertó el humano, victimas han ganado");
    }

    public void LoseVictims()
    {
        uiLoseVictims.SetActive(true);
        Debug.Log("se hizo de dia, victimas han perdido");
    }
}
