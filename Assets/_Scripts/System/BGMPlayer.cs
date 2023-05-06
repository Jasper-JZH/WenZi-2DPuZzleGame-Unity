using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private int Level;

    private void Start()
    {
        switch(Level)
        {
            case 0:
            AudioManager.PlayerAudio(AudioName.L0,true);
                break;
            case 1:
            AudioManager.PlayerAudio(AudioName.L1, true);
                break;
            case 2:
                AudioManager.PlayerAudio(AudioName.L2, true);
                break;
            case 3:
                AudioManager.PlayerAudio(AudioName.L3, true);
                break;
            case 4:
                AudioManager.PlayerAudio(AudioName.L4, true);
                break;
            case 5:
                AudioManager.PlayerAudio(AudioName.L5, true);
                break;
            case 6:
                AudioManager.PlayerAudio(AudioName.L6, true);
                break;
        }
    }
}
