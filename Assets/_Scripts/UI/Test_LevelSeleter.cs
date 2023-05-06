using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_LevelSeleter : MonoBehaviour
{
    [SerializeField] private List<Button> buttonsList = new List<Button>();
    private UIManager uIManager;

    private int levelCount;

    private void Start()
    {
        uIManager = UIManager.Instance;
        //levelCount = GameManager.levelNum;
        int level = 1;
        foreach (var button in buttonsList)
        {
            int i = level;
            button.onClick.AddListener(
                delegate ()
                {
                    AudioManager.PlayerAudio(AudioName.Click, false);
                    uIManager.ClosePanel(UIManager.Panels.Menu);
                    uIManager.SelectLevel(i);
                });
            level++;
        }
    }
}
