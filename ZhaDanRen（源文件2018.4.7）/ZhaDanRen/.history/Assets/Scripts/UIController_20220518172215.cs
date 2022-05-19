using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Text txt_HP, txt_Level, txt_Time, txt_Enemy;
    public GameObject gameoverPanel;
    public Animator levelFadeAnim;

    private void Awake()
    {
        Instance = this;
        Init();
    }
    private void Init()
    {
        gameoverPanel.transform.Find("btn_Again").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            //重新加载当前正在运行的场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        gameoverPanel.transform.Find("btn_Main").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Start");
        });
    }
    public void Refresh(int hp, int level, int time, int enemy)
    {
        txt_HP.text = "HP：" + hp.ToString();
        txt_Level.text = "Level：" + level.ToString();
        txt_Time.text = "Time：" + time.ToString();
        txt_Enemy.text = "Enemy：" + enemy.ToString();
    }
    public void ShowGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }
    /// <summary>
    /// 播放关卡提示动画
    /// </summary>
    /// <param name="levelIndex"></param>
    public void PlayLevelFade(int levelIndex)
    {
        Time.timeScale = 0;
        levelFadeAnim.transform.Find("txt_Level").GetComponent<Text>().text = "Level " + levelIndex.ToString();
        levelFadeAnim.Play("LevelFade", 0, 0);
        startDealy = true;
    }
    private bool startDealy = false;
    private float timer = 0;
    private void Update()
    {
        if (startDealy)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > 0.7f)
            {
                startDealy = false;
                Time.timeScale = 1;
                timer = 0;
            }
        }
    }
}
