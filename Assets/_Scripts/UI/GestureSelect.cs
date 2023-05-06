using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GestureSelect : MonoBehaviour
{
    #region 单例实现
    public static GestureSelect Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        dictionary = Dictionary.Instance;
        InitPanel();    // 初始化选字面板
        InitButtons();  //  生成按钮并订阅事件
        Dictionary.AddPlayerDicAction += AddNewButton;  //订阅事件
    }
    #endregion

    public string selectedGestureName;

    private Dictionary dictionary;
    /// <summary>
    /// 字按钮预制体
    /// </summary>
    [SerializeField] private Transform buttonPrefab;
    /// <summary>
    /// 选字的UI面板Panel
    /// </summary>
    [SerializeField] private Transform selectPanel;
    /// <summary>
    /// 按钮集
    /// </summary>
    [SerializeField] private List<Button> buttonList = new List<Button>();
    /// <summary>
    /// 当前显示的字帖
    /// </summary>
    [SerializeField] private TextMeshProUGUI curCopyBook;
    //[SerializeField] private Material TMPFontMaterial;
    private DrawController drawController;
    private void Start()
    {
        
        drawController = DrawController.Instance;
        //InitButtons();  // 初始化面板上的按钮
    }

    private void OnEnable()
    {
        //Debug.Log("GestureSelect onEnable");
        
        StartCoroutine(ChangeShowCharacter(selectedGestureName));   //显示字帖
    }

    private IEnumerator showUI()
    {
        bool show = false;
        while (!show)
        {
            show = true;
            yield return new WaitForSeconds(0.3f);
        }
        yield return ChangeShowCharacter(selectedGestureName);
    }


    /// <summary>
    /// 初始化选字面板
    /// </summary>
    private void InitPanel()
    {
        //绑定字帖
        selectPanel = transform.GetChild(0);
        curCopyBook = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //Debug.Log("选字面板初始化完毕！");
    }

    /// <summary>
    /// 添加一个新的按钮，并绑定按钮点击事件
    /// </summary>
    /// <param name="_name"></param>
    public void AddNewButton(string _name)
    {
        Transform newButton = Instantiate(buttonPrefab, selectPanel) as Transform;
        newButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = _name;
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(
            delegate () {
                AudioManager.PlayerAudio(AudioName.Click, false);
                selectedGestureName = _name;
                drawController.ChangeTarget(Dictionary.SearchGesture(selectedGestureName));
                //Debug.Log($"匹配目标更换为{selectedGestureName}:");
                StartCoroutine(ChangeShowCharacter(selectedGestureName));   //更新字帖的显示
            });
        buttonList.Add(button);
    }

    /// <summary>
    /// 修改字帖显示的字
    /// </summary>
    /// <param name="_name"></param>
    public IEnumerator ChangeShowCharacter(string _name)
    {
        if (_name == null) yield return null;
        else if (curCopyBook.text == _name)
        {
            curCopyBook.transform.localScale = new Vector3(0f, 0f);
        }
        else
        {
            curCopyBook.transform.DOScale(0f, .25f).SetUpdate(true);
            
        }
        yield return new WaitForSeconds(.26f);
        curCopyBook.text = _name;
        curCopyBook.transform.DOScale(1f, .25f).SetUpdate(true);

    }

    private void InitButtons()
    {
        List<string> names;
        dictionary.GetPlayerDicCharacterName(out names);

        //生成按钮
        foreach (string name in names)
        {
            Transform newButton = Instantiate(buttonPrefab, selectPanel) as Transform;
            newButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            buttonList.Add(newButton.GetComponent<Button>());
        }

        //绑定事件
        foreach (Button button in buttonList)
        {
            string name = button.GetComponentInChildren<TextMeshProUGUI>().text;
            button.onClick.AddListener(
                delegate () {
                    AudioManager.PlayerAudio(AudioName.Click, false);
                    selectedGestureName = name;
                    drawController.ChangeTarget(Dictionary.SearchGesture(selectedGestureName));
                    //Debug.Log($"匹配目标更换为{selectedGestureName}:");
                    StartCoroutine(ChangeShowCharacter(selectedGestureName));   //更新字帖的显示
                });
        }
    }

/*    public void ResetCopyBook()
    {
        if (curCopyBook == null) return;
        curCopyBook.text = "";
    }*/
}
