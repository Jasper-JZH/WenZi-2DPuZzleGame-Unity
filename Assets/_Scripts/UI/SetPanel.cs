using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetPanel : PanelBase
{
    private UIManager uIManager;
    /// <summary>
    /// 返回按钮
    /// </summary>
    [SerializeField] private Button returnButton;

    [Header("音量相关")]
    [SerializeField] private AudioMixer audioMixer;
    /// <summary>
    /// 背景音乐音量调节条
    /// </summary>
    [SerializeField] private Slider bgmSlider;
    /// <summary>
    /// 游戏音效音量调节条
    /// </summary>
    [SerializeField] private Slider sfxSlider;


    private void Start()
    {
        uIManager = UIManager.Instance;
        //按钮事件绑定
        returnButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.ClosePanel(UIManager.Panels.Set);
        });

        //音量相关
        InitSlider();
    }

    private void InitSlider()
    {
        float bgmValue, sfxValue;
        audioMixer.GetFloat("BGM", out bgmValue);
        audioMixer.GetFloat("SFX", out sfxValue);
        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;

        //绑定事件
        bgmSlider.onValueChanged.AddListener(value => 
        {
            if (value == bgmSlider.minValue)
                value = -80f;
            audioMixer.SetFloat("BGM", value);
        });
        sfxSlider.onValueChanged.AddListener(value =>
        {
            if (value == sfxSlider.minValue)
                value = -80f;
            audioMixer.SetFloat("SFX", value);
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
