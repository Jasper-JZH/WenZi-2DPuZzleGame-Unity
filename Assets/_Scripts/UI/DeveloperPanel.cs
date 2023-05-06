using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperPanel : PanelBase
{

    private UIManager uIManager;
    /// <summary>
    /// 返回按钮
    /// </summary>
    [SerializeField] private Button returnButton;

    private void Start()
    {
        uIManager = UIManager.Instance;
        //按钮事件绑定
        returnButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.ClosePanel(UIManager.Panels.Developer);
        });
    }

    public override void OnPanelShow()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnPanelClose()
    {
        this.gameObject.SetActive(false);
    }
}
