using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private string timerStr;

    private void Update()
    {
        if (Managers.Instance == null || !LevelManager.Instance.IsLevelStarted) return;

        timer += Time.deltaTime;
        timerStr = Mathf.Round(timer).ToString();
    }

    private void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 100;
        GUI.Label(new Rect(50, 50, 600, 300), timerStr, gUIStyle);
    }
}