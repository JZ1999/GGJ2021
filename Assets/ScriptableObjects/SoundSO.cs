using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound/Sound", order = 3)]
public class SoundSO : ScriptableObject
{
    public string nameSound;

    public AudioSource[] sourceList;
}