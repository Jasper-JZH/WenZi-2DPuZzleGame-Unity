using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogePanel : PanelBase
{
    private UIManager uIManager;
    /// <summary>
    /// 定义题目答案结构体
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 题干
        /// </summary>
        public string question;
        public string[] options;
        /// <summary>
        /// 四个选项
        /// </summary>
        public string A, B, C, D;  
        /// <summary>
        /// 答对和打错的提示
        /// </summary>
        public string rightDialoge, wrongDialoge;
        /// <summary>
        /// 正确答案
        /// </summary>
        public string answer;
        public Question(string _q, string _rD, string _wD, string _aw, params string[] _options)
        {
            question = _q;
            rightDialoge = _rD;
            wrongDialoge = _wD;
            answer = _aw;
            options = new string[4];
            for(int i = 0; i < _options.Length; i++)
            {
                options[i] = _options[i];
            }
        }
    }

    /// <summary>
    /// 答案选项Toggle集
    /// </summary>
    [SerializeField] private List<Toggle> answersToggleList;
    /// <summary>
    /// 答案选项对应的文本
    /// </summary>
    private List<TextMeshProUGUI> answersTextList = new List<TextMeshProUGUI>();
    /// <summary>
    /// 确认按钮
    /// </summary>
    [SerializeField] private Button checkButton;
    /// <summary>
    /// 答题面板
    /// </summary>
    [SerializeField] private GameObject QAPanel;
    /// <summary>
    /// 对话框文字
    /// </summary>
    [SerializeField] private TextMeshProUGUI dialogeText;
    /// <summary>
    /// NPC的“字”，即大头像
    /// </summary>
    [SerializeField] private TextMeshProUGUI npcName;
    /// <summary>
    /// 对话内容，通过关键字索引对话的文本
    /// </summary>
    public Dictionary<string, List<string>> dialogeDic = new Dictionary<string, List<string>>();
    /// <summary>
    /// 当前字的所有对话
    /// </summary>
    private List<string> curDialoge;
    /// <summary>
    /// 当前NPC的对话数
    /// </summary>
    private int dialogeCount;
    /// <summary>
    /// 当前已显示（进行）的对话数(从0开始)
    /// </summary>
    private int curCount;
    /// <summary>
    /// 强行显示所有文字
    /// </summary>
    private bool forceShow;

    [Header("答题相关")]

    /// <summary>
    /// 当前npc是否有题目
    /// </summary>
    private bool hasQuestion;

    /// <summary>
    /// 每道题选项的文本(题目对应的字，题目)
    /// </summary>
    public Dictionary<string, Question> questionDic = new Dictionary<string, Question>();
    /// <summary>
    /// 当前题目
    /// </summary>
    [SerializeField] private Question curQuestion;
    /// <summary>
    /// 当前题目对应的字
    /// </summary>
    [SerializeField] private string questionName;
    /// <summary>
    /// Toggle改变的事件
    /// </summary>
    private UnityAction<bool> onToggleValueChange;
    /// <summary>
    /// 选对了
    /// </summary>
    private bool isRight;
    /// <summary>
    /// 题号的对外接口
    /// </summary>
    public string QuestionName
    {
        get { return questionName; }
        set
        {   //每次设置题号的同时也会更新对应的题目
            questionName = value;
            //加载题目
            LoadQuestion(questionName);
        }
    }

    /// <summary>
    /// 当前是否为问答模式
    /// </summary>
    [SerializeField]private bool isQA;

    private void Awake()
    {
        InitQuestionDic();  //初始化题目
        InitDialogeDic();   //初始化所有对话文本
        InitToggles();      //初始化选项相关
    }

    private void Start()
    {
        uIManager = UIManager.Instance;
    }

    private void Update()
    {
        //在对话中时，按下左键则跳过当前对话到下一句（答题时无效）
        if(Input.GetMouseButtonDown(0))
        {
            if(hasQuestion)     //含题目的对话
            {
                if (!isQA)   //对话中
                {
                    if (!forceShow)  //显示所有字
                    {
                        dialogeText.text = curDialoge[curCount];
                        ++curCount;
                        forceShow = true;
                    }
                    else
                    {
                        NextOne();
                        forceShow = false;
                    }
                }
                else if (isRight)    //答对问题
                {
                    //提示玩家要写字，关闭对话框
                    OnPanelClose();
                    //让UIManager唤起DrawController
                    uIManager.EndDialoge(questionName);
                }
            }
            else if(curCount < dialogeCount)  //不含题目的一段对话
            {
                if (!forceShow)  //显示所有字
                {
                    dialogeText.text = curDialoge[curCount];
                    ++curCount;
                    forceShow = true;
                }
                else
                {
                    NextOne();
                    forceShow = false;
                }
            }
            else  //不含题目的一段对话结束
            {
                OnPanelClose();
                uIManager.EndDialoge(questionName);
            }
        }
    }

    /// <summary>
    /// 和新的NPC交互
    /// </summary>
    /// <param name="_npcName"></param>
    public void NewDialoge(string _npcName)
    {
        OnPanelShow();                  //显示面板，但还没有加载信息
        npcName.text = _npcName[0].ToString();        //更新头像
        QuestionName = _npcName;        //尝试更新题目
        LoadDialoge(_npcName);          //加载对话文本
        NextOne();                      //开始播放第一句话
    }

    /// <summary>
    /// 下一句话
    /// </summary>
    private void NextOne()
    {
        if (curCount < dialogeCount)
        {
            //显示下一句
            StartCoroutine(PopPrint(curDialoge[curCount]));
        }
        else //说明是对话已结束，接下来进入答题环节
        {
            if (!hasQuestion) return;
            isQA = true;
            //显示题目
            InitQAPanel();
        }
    }

    /// <summary>
    /// 逐字打印
    /// </summary>
    /// <param name="_string"></param>
    /// <returns></returns>
    private IEnumerator PopPrint(string _string)
    {
        int oldCurCount = curCount;
        int length = _string.Length;
        int count = 0;  //已显示的字数
        string text = "";
        while(count < length)
        {
            if (oldCurCount != curCount)//说明玩家提前结束了这句话
            {
                forceShow = true;
                break;
            }    
            text += _string[count];
            dialogeText.text = text;
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        if (count == length && oldCurCount == curCount)
        {
            forceShow = true;
            ++curCount;
        }
    }    

    public override void OnPanelShow()
    {
        this.gameObject.SetActive(true);
        QAPanel.gameObject.SetActive(false);
        forceShow = false;
        GameManager.ShowCursor(true);
    }

    public override void OnPanelClose()
    {
        this.gameObject.SetActive(false);
        GameManager.ShowCursor(false);
    }

    /// <summary>
    /// 打开（初始化）QA面板
    /// </summary>
    private void InitQAPanel()
    {
        QAPanel.SetActive(true);
        //载入题目
        StartCoroutine(PopPrint(curQuestion.question));
        //载入选项
        for (int i = 0; i < curQuestion.options.Length; i++)
        {
            answersTextList[i].text = curQuestion.options[i];
        }

    }

    /// <summary>
    /// 关闭QA面板
    /// </summary>
    private void CloseQAPanel()
    {
        QAPanel.SetActive(false);
    }

    /// <summary>
    /// 初始化按钮
    /// </summary>
    private void InitToggles()
    {
        //onToggleValueChange += Check;
        //onToggleValueChange += 绑定音效
        if (answersToggleList.Count > 0)
        {
            foreach(var toggle in answersToggleList)
            {
                //toggle.onValueChanged.AddListener(onToggleValueChange);
                answersTextList.Add(toggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>());
            }
        }
        //绑定确认按钮
        
        checkButton.onClick.AddListener(delegate () {
            AudioManager.PlayerAudio(AudioName.Click, false);
            Check(); });
    }
    /// <summary>
    /// 加载对话
    /// </summary>
    /// <param name="_name"></param>
    private void LoadDialoge(string _name)
    {
        isQA = false;
        if (dialogeDic.TryGetValue(_name, out curDialoge))
        {
            //Debug.Log($"成功更新与{_name}的对话");
            dialogeCount = curDialoge.Count;
            curCount = 0;
        }
        else Debug.LogWarning($"找不到与{_name}的对话");
    }

    /// <summary>
    /// 加载对应的题目
    /// </summary>
    /// <param name="_questionID"></param>
    private void LoadQuestion(string _name)
    {
        isRight = false;
        if (questionDic.TryGetValue(_name, out curQuestion))
        {
            //Debug.Log($"成功更新{_name}的题目");
            hasQuestion = true;
        }
        else
        {
            Debug.LogWarning($"找不到与{_name}的题目");
            //说明这个NPC是纯对话的
            hasQuestion = false;
        }
    }

    /// <summary>
    /// 判断玩家是否选对了
    /// </summary>
    private void Check()
    {
        string curChosed = "";
        foreach (var toggle in answersToggleList)
        {
            if (toggle.isOn) curChosed += toggle.name;
        }
        if (curChosed == curQuestion.answer)
        {
            isRight = true;
            //Debug.Log("答对了！");
            //更新对话
            StartCoroutine(PopPrint(curQuestion.rightDialoge));
            //关闭QA面板
            CloseQAPanel();
        }
        else
        {
            //Debug.Log("答错了！");
            //更新对话
           StartCoroutine(PopPrint(curQuestion.wrongDialoge));
        }
        //清空toggle选项
        foreach(var toggle in answersToggleList)
        {
            toggle.isOn = false;
        }
    }

    /// <summary>
    /// 初始化所有对话文本
    /// </summary>
    private void InitDialogeDic()
    {

        //火
        string[] dialogeArray = new string[] {
            "火，毁也。南方之行，炎而上。象行。——《说文解字》",
            "我是火，为世界带去了温暖的火。但是，我也有给世界带去灾祸的另一面。",
            "这个特殊的世界是由五行八卦来构成维系的，也因此只有五行八卦具有改变世界的力量。",
            "并且只有当你真正了解一个字的时候，你才能将文字的力量化为己用。",
            "当然除了我以外，这个世界还有许多像我一样的存在，只要通过了它们的考验，你也可以获得它们的力量。",
            "在你未来的旅程中遇到困难时，只需在写字台上书写你对应的字帖，就可以暂时借用我们的力量帮助你度过难关。",
             "写好后按下鼠标的“右键”就可以尝试激活字的力量，如果写的不够工整，可能就无法成功激活。",
             "当然，在写字时写错了不用担心，按下“F键”就可以重写啦。",
            "那么首先来接受我的第一个测验吧。"
            };
        List<string> newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("火", newDialoges);

        //火2
        dialogeArray = new string[] {
            "看来你已经对我有充分的了解了，那就把我的力量借给你吧。",
            "对了，差点忘记告诉你，只需要按下“C键”就可以打开你的写字台了。",
            "好了，开始踏上你的旅程吧！"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("火2", newDialoges);

        //师
        dialogeArray = new string[] {
            "欢迎来到文字世界",
            "首先来活动活动手脚吧",
            "使用键盘上的“A” “D” 按键来控制人物的左右移动，“空格键”跳跃。",
            "当遇到其它字的时候，可以尝试按下“E键”和它们对话",
            "好了，现在踏上你的旅程吧！"
            };

        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("师", newDialoges);

        //木
        dialogeArray = new string[] {
            "冒也。冒地而生。东方之行。从屮（che四声），下象其根。——《说文解字》",
            "我是木，“一年树谷，十年树木，百年树人”。",
            "我为世界带去了生机，让生物得以生存。",
            "不知，你是否做好了丰富这个世界的准备？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("木", newDialoges);

        //水
        dialogeArray = new string[] {
            "谢谢你拯救了我！",
            "我是水，”上善若水，水善利万物而不争“。",
            "水，准也。北方之行。象众水并流，中有微阳之气也。——《说文解字》",
            "老子曾将我视为君子的准则。那么你是否是君子呢？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("水", newDialoges);

        //金
        dialogeArray = new string[] {
            "金，五色金也，黄为之长。西方之行。生於土，从土。——《说文解字》",
            "我是金，不知你，是否有能将我收入麾下的能力？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("金", newDialoges);

        //土
        dialogeArray = new string[] {
            "地之吐生物者也。二象地之下、地之中，物出行也。——《说文解字》",
            "我是土，乃“烟”仙人座下一童子，取自“积土而成山”，有坚持不懈之意。",
            "仙人知你要来，特命我于此等待 ，协助你度过难关面见仙人。",
            "那你是否具有足够毅力去到仙人面前呢？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("土", newDialoges);

        //烟
        dialogeArray = new string[] {
            "烟，火气也。从火、yan声。烟，或从因。——《说文解字》",
            "吾乃烟仙人，“大漠孤烟直，长河落日圆”。",
             "你成功通过了我设下的难关来到了我的面前，不知你准备好面对最后的难关了吗？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("烟", newDialoges);

        //坎
        dialogeArray = new string[] {
            "陷也。从土欠声。苦感切。——《说文解字》",
            "我是坎，八卦之一，亦可作六十四卦之一的坎卦。",
             "“有孚维心，亨，行有尚。”即有诚信系于心间，做事就会亨通顺利，行动受到奖赏。",
             "你是否是一个诚信的人？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("坎", newDialoges);

        //巽
        dialogeArray = new string[] {
            "巽，具也。——《说文解字》",
            "我是巽，八卦之一，也作六十四卦之一的巽卦。",
             "“利有攸往，利见大人。”做人做事要谦虚，才有利于自己的成长与发展。这是我能告诉你的道理。",
             "先让我们从谦虚求学开始吧。"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("巽", newDialoges);

        //震
        dialogeArray = new string[] {
            "霹雳，振物者。从雨辰声。《春秋传》曰：“震夷伯之庙。”——《说文解字》",
            "我是震，八卦之一，也是六十四卦之一的震卦。",
             "“震来xi々，笑言哑哑；震惊百里，不丧匕鬯。”即使惊雷打在附近都面不改色，处变不惊。",
             "你是否有这种素质呢？"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("震", newDialoges);

        //辰
        dialogeArray = new string[] {
            "辰，震也。三月，阳气动，雷电振，民农时也。辰，房星也，天时也。——《说文解字》",
            "我是辰，”危楼高百尺，手可摘星辰“的辰。"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("辰", newDialoges);

        //飞
        dialogeArray = new string[] {
            "鸟翥也。象形。凡飞之属皆从飞。甫微切。——《说文解字》",
            "我是飞，”月下飞天镜，云生结海楼“的飞。",
            "本义是指鸟类在空中扇翅的动作，后又扩展出其他动物的飞翔，以及物品的漂浮。"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("飞", newDialoges);

        //雨
        dialogeArray = new string[] {
            "雨，水从云下也。一象天，冂象云，水nen其闲也。凡雨之属皆从雨。——《说文解字》",
            "我是雨，“清明时节雨纷纷，路上行人欲断魂”的雨。",
            "我是一种常见的天气现象，也是农作物的生命之源。"
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("雨", newDialoges);

        //坤
        dialogeArray = new string[] {
            "..."
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("坤", newDialoges);

        //云
        dialogeArray = new string[] {
            "山川气也。从雨，云象云回转形。凡云之属皆从云。云，古文省雨。——《说文解字》",
            "我是云，“月下飞天镜，云生结海楼”的云",
            };
        newDialoges = new List<string>(dialogeArray);
        dialogeDic.Add("云", newDialoges);
    }

    /// <summary>
    /// 初始化问题字典
    /// </summary>
    private void InitQuestionDic()
    {
        //从文件中读取所有题目
        // 火
        Question newQuestion = new Question("告诉我，我的字义！",
            "哈哈哈，我很欣赏你的知识，来让我们一起冒险吧！", "回答错误，看来你还不具备足够的知识。",
            "BCD",
            "形容词：摇曳的", "名词：物体燃烧放出的光和热", "动词：焚烧", "形容词：像火一样的颜色");
        questionDic.Add("火", newQuestion);

        // 金
        newQuestion = new Question("以下哪些是我具有的字义？要注意有多个选项哦",
            "恭喜你答对了，那么来书写一遍，将我加入你的字帖吧！", "看来你选错了，再仔细想想？",
            "ABC",
            "名词：金属及金属制品的通称", "名词：钱财，货币单位", "形容词：比喻尊贵、贵重", "形容词：埋在地下");
        questionDic.Add("金", newQuestion);

        // 木
        newQuestion = new Question("你可知，我的字义？",
            "正确！让我来助你一臂之力吧！偶尔可以给我浇点水哦", "还不够准确，再来一遍吧。",
            "ABD",
            "名词：木本植物的通称，", "形容词：质朴、呆、反应慢", "形容词：长在土上的", "动词：发呆、发愣");
        questionDic.Add("木", newQuestion);

        // 水
        newQuestion = new Question("要成为君子，首先需要具有足够的知识。",
            "来吧，让我加入你的字帖，一起向着君子成长！", "看来你的知识储备还不够，再努努力吧。",
            "ACD",
            "动词：取水用水、浸泡，", "形容词：流动的", "名词：泛指一切水域", "形容词：马虎、不负责任的");
        questionDic.Add("水", newQuestion);

        // 土
        newQuestion = new Question("首先，告诉我，我的字义吧。",
            "所谓天公地母，土能滋养万物。不妨试试在我身上种下树木，以水润之，从而到达更高的地方！", "坚持非常重要，但是知识也要跟得上，不然只会做无用功啊。",
            "AC",
            "名词：土地，领土，故乡", "形容词：脚下的", "形容词：不合潮流的", "动词：耕作");
        questionDic.Add("土", newQuestion);

        // 烟
        newQuestion = new Question("那你可知我具有什么字义？",
            "答对了！但使用我的力量时可要注意安全，一旦遇到火可能会发生难以预料的事。", "没有全部选对哦，再试试吧。",
            "AB",
            "名词：物体燃烧时产生的气体", "形容词：像烟的", "形容词：轻的", "形容词：大的");
        questionDic.Add("烟", newQuestion);

        // 坎
        newQuestion = new Question("告诉我我具有的其他字义吧。",
            "答对了，看来你是个诚信的人，让我也加入你的字帖吧。", "不对哦，再思考一下吧。",
            "BD",
            "名词：土堆", "动词：挖洞，陷落", "动词：水中的土", "名词：坑、穴、凹陷");
        questionDic.Add("坎", newQuestion);

        // 巽
        newQuestion = new Question("你可知我具有什么字义？",
            "看来你具有谦虚的品格，那让我跟着你吧。", "再努力进修一下吧。",
            "AC",
            "动词：消散", "形容词：天上的", "方位词：巽地（东南方）", "名词：在外的");
        questionDic.Add("巽", newQuestion);

        // 震
        newQuestion = new Question("告诉我我的字义吧！",
            "恭喜你答对了，来让我们一起开始冒险吧。", "很遗憾，答错了。",
            "BD",
            "形容词：很大的", "名词：响雷，威势", "名词：下雨", "动词：剧烈颤动，威慑");
        questionDic.Add("震", newQuestion);

        // 辰
        newQuestion = new Question("你可知我还有其他什么含义吗？",
            "看来你已经完全了解了！", "看来了解的还不够全面啊。",
            "BCD",
            "动词：日出", "名词：十二地支第五位", "名词：时间，泛指日月星辰", "形容词：美善的模样");
        questionDic.Add("辰", newQuestion);


        // 飞
        newQuestion = new Question("那你还知道我其他的字义吗？",
            "恭喜答对了！", "选错了哦。",
            "BC",
            "名词：天空", "形容词：传播迅速，以外的", "动词：上升，传播迅速", "动词：抛起");
        questionDic.Add("飞", newQuestion);


        // 雨
        newQuestion = new Question("那你知道我其他字义吗？",
            "恭喜你答对了！", "不对，再多尝试一下吧。",
            "CD",
            "动词：哭", "形容词：悲伤的", "名词：从空中落向地面的水", "动词：灌溉，天上降下");
        questionDic.Add("雨", newQuestion);

        // 云
        newQuestion = new Question("你来猜猜我的字义吧",
            "真不错，答对了", "不对，再试试吧。",
            "AB",
            "名词：有水滴、冰晶等的聚合体", "形容词：形状像云的", "形容词：漂浮的", "形容词：一无所知的");
        questionDic.Add("云", newQuestion);
    }
}
