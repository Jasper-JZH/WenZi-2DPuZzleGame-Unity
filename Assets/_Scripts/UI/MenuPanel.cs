using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuPanel : PanelBase
{
    private void Awake()
    {
        //获取父Canvas，让Canvas不会被销毁
        //DontDestroyOnLoad(transform.parent.gameObject);
    }
    [Header("UI")]
    /// <summary>
    /// 游戏标题
    /// </summary>
    [SerializeField] private Image gameTitle;
    /// <summary>
    /// 开始按钮
    /// </summary>
    [SerializeField] private Button startButton;
    /// <summary>
    /// 设置按钮
    /// </summary>
    [SerializeField] private Button setButton;
    /// <summary>
    /// 开发者按钮
    /// </summary>
    [SerializeField] private Button developerButton;
    /// <summary>
    /// 退出按钮
    /// </summary>
    [SerializeField] private Button exitButton;


    [Header("引用")]
    private UIManager uIManager;

    private void Start()
    {
        uIManager = UIManager.Instance;
        //按钮事件绑定
        startButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.ClosePanel(UIManager.Panels.Menu);
            uIManager.SelectLevel(GameManager.GetCurLevel());
        });
        setButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.ShowPanel(UIManager.Panels.Set);
        });
        developerButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.ShowPanel(UIManager.Panels.Developer);
        });
        exitButton.onClick.AddListener(delegate ()
        {
            AudioManager.PlayerAudio(AudioName.Click, false);
            uIManager.gameManager.QuitGame();
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
