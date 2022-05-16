using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject playerPre;
    public int enemyCount;
    [HideInInspector]
    public int time = 180;

    private int levelCount = 0;
    private float timer = 0f;

    private MapController mapController;
    private GameObject player;
    private PlayerCtrl playerCtrl;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mapController = GetComponent<MapController>();
        LevelCtrl();
    }
    /// <summary>
    /// 关卡计时器
    /// </summary>
    private void LevelTimer()
    {
        //时间用完了，游戏结束
        if (time <= 0)
        {
            if (playerCtrl.HP > 0)
            {
                playerCtrl.HP--;
                time = 200;
                return;
            }
            playerCtrl.PlayDieAni();
            return;
        }
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            time--;
            timer = 0;
        }
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    public void Gameover()
    {
        //显示游戏结束界面
        UIController.Instance.ShowGameoverPanel();
    }
    private void Update()
    {
        LevelTimer();
        UIController.Instance.Refresh(playerCtrl.HP, levelCount, time, enemyCount);

        if (Input.GetKeyDown(KeyCode.N)) LevelCtrl();
    }
    /// <summary>
    /// 加载下一关卡
    /// </summary>
    public void LoadNextLevel()
    {
        if (enemyCount <= 0)
            LevelCtrl();
    }
    /// <summary>
    /// 关卡控制器
    /// </summary>
    private void LevelCtrl()
    {
        time = levelCount * 50 + 130;

        int x = 6 + 2 * (levelCount / 3);
        int y = 3 + 2 * (levelCount / 3);
        if (x > 18) x = 18;
        if (y > 15) y = 15;

        enemyCount = (int)(levelCount * 1.5f) + 1;
        if (enemyCount > 40) enemyCount = 40;
        mapController.InitMap(x, y, x * y, enemyCount);
        if (player == null)
        {
            player = Instantiate(playerPre);
            playerCtrl = player.GetComponent<PlayerCtrl>();
            playerCtrl.Init(1, 3, 2);
        }
        playerCtrl.ResetPlayer();
        player.transform.position = mapController.GetPlayerPos();

        //回收场景中残留的爆炸特效
        GameObject[] effects = GameObject.FindGameObjectsWithTag(Tags.BombEffect);
        foreach (var item in effects)
        {
            ObjectPool.Instance.Add(ObjectType.BombEffect, item);
        }

        Camera.main.GetComponent<CameraFollow>().Init(player.transform, x, y);

        levelCount++;
        UIController.Instance.PlayLevelFade(levelCount);
    }
    public bool IsSuperWall(Vector2 pos)
    {
        return mapController.IsSuperWall(pos);
    }
}
