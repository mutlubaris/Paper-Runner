using AdvanceUI;
using TMPro;

public class PaperCounterPanel : AdvancePanel
{
    public TextMeshProUGUI paperAmountText;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.AddListener(ResetPanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.RemoveListener(ResetPanel);
    }

    private void ResetPanel()
    {
        paperAmountText.SetText("---");
    }
}