using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

/// <summary>
/// 实现异步加载场景
/// </summary>
public class AsyncLoadingScene : MonoBehaviour
{
    #region private 成员
    /// <summary>
    /// 异步加载场景进程控制
    /// </summary>
    private AsyncOperation asyncOperation;
    /// <summary>
    /// 下一个场景的索引
    /// </summary>
    private int nextSceneIndex;
    /// <summary>
    /// 进度条百分比
    /// </summary>
    private float processBarPercentage = 0f;
    /// <summary>
    /// 提示文字
    /// </summary>
    [SerializeField]private TextMeshProUGUI SignText;
    /// <summary>
    /// 进度条加载速度
    /// </summary>
    [Range(1.0f, 10.0f)]
    [SerializeField] private float barSpeed;

    /// <summary>
    /// 加载圈
    /// </summary>
    [SerializeField] private Image ring;


    private SceneController sceneController;

    #endregion

    #region public 成员

    #endregion

    #region private 方法
    private void Start()
    {
        sceneController = SceneController.Instance;
        nextSceneIndex = sceneController.nextSceneIndex;
        StartCoroutine("LoadScene");  // 开启协程异步加载场景
        SignText.fontMaterial.SetColor("_FaceColor", new Color(255f, 255f, 255f, 0f));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncOperation.allowSceneActivation = false;    //关闭“加载完毕直接跳转”

        while (!asyncOperation.isDone)
        {
            processBarPercentage = asyncOperation.progress;
            if (asyncOperation.progress >= 0.89f) processBarPercentage = 1.0f;

            //转圈加载效果
            ring.transform.localRotation = Quaternion.Euler(Vector3.Lerp(ring.transform.localRotation.eulerAngles, new Vector3(0.0f, 0.0f, processBarPercentage * 220), barSpeed * Time.deltaTime));
            //ring.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(ring.transform.rotation.z, processBarPercentage * -220, barSpeed * Time.deltaTime));

            //ring.transform.eulerAngles = Vector3.Lerp(ring.transform.eulerAngles, new Vector3(0f, 0f, processBarPercentage * -220), barSpeed * Time.deltaTime);
            if (ring.transform.eulerAngles.z > 215)
            {
                //ring.transform.rotation = Quaternion.Euler(0, 0, -220);

                //让提示的字渐变
                SignText.fontMaterial.DOColor(new Color(255f, 255f, 255f, 255f), "_FaceColor", 3f).SetEase(Ease.InQuad);
                
                if (Input.anyKey)    //按下任意按键继续
                {
                    asyncOperation.allowSceneActivation = true;
                    yield return StartCoroutine(sceneController.LoadSceneCallBack(nextSceneIndex));
                }
            }
            yield return null;
        }
    }

    #endregion

    #region public 方法

    #endregion
}
