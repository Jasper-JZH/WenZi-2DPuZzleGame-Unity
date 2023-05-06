using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

/// <summary>
/// ʵ���첽���س���
/// </summary>
public class AsyncLoadingScene : MonoBehaviour
{
    #region private ��Ա
    /// <summary>
    /// �첽���س������̿���
    /// </summary>
    private AsyncOperation asyncOperation;
    /// <summary>
    /// ��һ������������
    /// </summary>
    private int nextSceneIndex;
    /// <summary>
    /// �������ٷֱ�
    /// </summary>
    private float processBarPercentage = 0f;
    /// <summary>
    /// ��ʾ����
    /// </summary>
    [SerializeField]private TextMeshProUGUI SignText;
    /// <summary>
    /// �����������ٶ�
    /// </summary>
    [Range(1.0f, 10.0f)]
    [SerializeField] private float barSpeed;

    /// <summary>
    /// ����Ȧ
    /// </summary>
    [SerializeField] private Image ring;


    private SceneController sceneController;

    #endregion

    #region public ��Ա

    #endregion

    #region private ����
    private void Start()
    {
        sceneController = SceneController.Instance;
        nextSceneIndex = sceneController.nextSceneIndex;
        StartCoroutine("LoadScene");  // ����Э���첽���س���
        SignText.fontMaterial.SetColor("_FaceColor", new Color(255f, 255f, 255f, 0f));
    }

    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncOperation.allowSceneActivation = false;    //�رա��������ֱ����ת��

        while (!asyncOperation.isDone)
        {
            processBarPercentage = asyncOperation.progress;
            if (asyncOperation.progress >= 0.89f) processBarPercentage = 1.0f;

            //תȦ����Ч��
            ring.transform.localRotation = Quaternion.Euler(Vector3.Lerp(ring.transform.localRotation.eulerAngles, new Vector3(0.0f, 0.0f, processBarPercentage * 220), barSpeed * Time.deltaTime));
            //ring.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(ring.transform.rotation.z, processBarPercentage * -220, barSpeed * Time.deltaTime));

            //ring.transform.eulerAngles = Vector3.Lerp(ring.transform.eulerAngles, new Vector3(0f, 0f, processBarPercentage * -220), barSpeed * Time.deltaTime);
            if (ring.transform.eulerAngles.z > 215)
            {
                //ring.transform.rotation = Quaternion.Euler(0, 0, -220);

                //����ʾ���ֽ���
                SignText.fontMaterial.DOColor(new Color(255f, 255f, 255f, 255f), "_FaceColor", 3f).SetEase(Ease.InQuad);
                
                if (Input.anyKey)    //�������ⰴ������
                {
                    asyncOperation.allowSceneActivation = true;
                    yield return StartCoroutine(sceneController.LoadSceneCallBack(nextSceneIndex));
                }
            }
            yield return null;
        }
    }

    #endregion

    #region public ����

    #endregion
}
