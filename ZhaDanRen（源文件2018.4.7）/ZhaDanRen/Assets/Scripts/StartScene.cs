using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Button btn_Start, btn_Exit;

    private void Awake()
    {
        btn_Start.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Game");
            //SceneManager.LoadScene(1);
        });
        btn_Exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
