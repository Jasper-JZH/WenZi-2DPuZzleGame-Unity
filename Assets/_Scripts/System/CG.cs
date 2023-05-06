using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CG : MonoBehaviour
{
    /// <summary>
    /// 按住指定按键累积一定时间则跳过cg
    /// </summary>
    [SerializeField] private float accTime = 0f;
    public void LoadMenuScene()
    {
        Debug.Log("LoadMenuScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            accTime += Time.deltaTime;
            if (accTime > 1.5f)
                LoadMenuScene();
        }
    }
}
