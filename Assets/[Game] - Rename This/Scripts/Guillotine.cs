using UnityEngine;

public class Guillotine : MonoBehaviour
{
    [SerializeField] private GameObject cutter;
    [SerializeField] private float cutterMinY = 1f;
    [SerializeField] private float cutterMaxY = 4.5f;
    [SerializeField] private float cutterSpeed = 2f;

    private int cutDirection = -1;

    private void Update()
    {
        cutter.transform.localPosition += Vector3.up * cutDirection * cutterSpeed * Time.deltaTime;
        if (cutter.transform.localPosition.y > cutterMaxY)
        {
            cutter.transform.localPosition = new Vector3(cutter.transform.localPosition.x, cutterMaxY, cutter.transform.localPosition.z);
            cutDirection *= -1;
        }
        if (cutter.transform.localPosition.y < cutterMinY)
        {
            cutter.transform.localPosition = new Vector3(cutter.transform.localPosition.x, cutterMinY, cutter.transform.localPosition.z);
            cutDirection *= -1;
        }
    }
}