using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseManager : MonoBehaviour
{
    [Range(100, 200)]
    public float maxSoundForAwaking;
    public float soundForAwaking;
    public Image uiBarFront;

    // Start is called before the first frame update
    void Start()
    {
        uiBarFront.fillAmount = 0f;
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
        Debug.Log("se despertó el humano, victimas han ganado");
    }
}
