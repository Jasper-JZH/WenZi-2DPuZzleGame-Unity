using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音频管理器，存储所有音频并可以控制播放和停止
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region 单例实现
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    /// <summary>
    /// 单个音频信息类
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        [Header("音频文件")]
        public AudioClip clip;

        [Header("音频类型")]
        public AudioMixerGroup mixerGroup;

        [Header("音频音量")]
        [Range(0, 1)]
        public float volume = 1f;

        [Header("自动播放")]
        public bool playOnAwake;

        [Header("循环播放")]
        public bool loop;

        public Sound(AudioClip _clip, AudioMixerGroup _group, float _volume, bool _playOnAwake, bool _loop)
        {
            clip = _clip;
            mixerGroup = _group;
            volume = _volume;
            playOnAwake = _playOnAwake;
            loop = _loop;
        }
    }
    /// <summary>
    /// 存储所有音频
    /// </summary>
    [SerializeField] private List<Sound> soundsList;
    /// <summary>
    /// 每个clip的名称对应一个AudioSource
    /// </summary>
    private Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();

    private void Start()
    {
        foreach(var sound in soundsList)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = sound.playOnAwake;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.volume = sound.volume;

            if (source.playOnAwake) //自动播放
                source.Play();

            audioSourceDic.Add(source.clip.name, source);
        }
    }
    /// <summary>
    /// 播放指定音频
    /// </summary>
    /// <param name="_name">音频名称</param>
    /// <param name="_isWait">若已有同一段音频正在播放，是否等待音频播放完</param>
    public static void PlayerAudio(string _name, bool _isWait)
    {
        if(!Instance.audioSourceDic.ContainsKey(_name))
        {
            Debug.LogWarning($"指定音频{_name}不存在");
            return;
        }
        if(_isWait && Instance.audioSourceDic[_name].isPlaying) return;
        else Instance.audioSourceDic[_name].Play(); 
    }

    /// <summary>
    /// 停止指定音频的播放
    /// </summary>
    /// <param name="_name"></param>
    public static void StopAudio(string _name)
    {
        if (!Instance.audioSourceDic.ContainsKey(_name))
        {
            Debug.LogWarning($"指定音频{_name}不存在");
            return;
        }
        else Instance.audioSourceDic[_name].Stop();
    }
}
