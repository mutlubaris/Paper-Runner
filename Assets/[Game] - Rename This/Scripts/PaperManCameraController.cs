using DG.Tweening;
using UnityEngine;

public class PaperManCameraController : MonoBehaviour
{
    private float initialPosX;

    private void Start()
    {
        initialPosX = transform.position.x;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(initialPosX, transform.position.y, transform.position.z);
    }

    public void ShakeCamera(float power = 2f, float duration = 0.2f, int vibro = 2, float elasitcy = 0.5f)
    {
        transform.DOPunchRotation(Vector3.forward * power, duration, vibro, elasitcy);
        HapticManager.Haptic(HapticTypes.HeavyImpact);
    }
}
