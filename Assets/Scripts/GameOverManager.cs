using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Quit();

        if (Input.GetButtonDown("Submit"))
            GameManager.Instance.ResetGame();
    }

    private void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}