using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private float slowMoExitDistance = 4f;

    private float playerInitialZ;
    private PlayerMover playerMover;
    private bool levelFailed;
    private bool inSlowMotion;
    private TextMeshPro text;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;

        GameManager.Instance.OnStageFail.AddListener(() => levelFailed = true);
        text = GetComponent<TextMeshPro>();
        text.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inSlowMotion && other.TryGetComponent<PlayerMover>(out PlayerMover _playerMover))
        {
            playerMover = _playerMover;
            playerMover.Sensitivity *= 10;
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            playerInitialZ = playerMover.transform.position.z;
            text.enabled = true;
            inSlowMotion = true;
        }
    }

    private void Update()
    {
        if (playerMover != null && inSlowMotion &&((playerMover.transform.position.z > playerInitialZ + slowMoExitDistance) || levelFailed))
        {
            playerMover.Sensitivity /= 10;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            text.enabled = false;
            inSlowMotion = false;
        }
    }
}
