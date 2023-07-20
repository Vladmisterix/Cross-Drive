using Unity.VisualScripting;
using UnityEngine;

public class SlowTimeObject : MonoBehaviour
{
    private float slowMoValue = 0.01f;
    private float slowDuration = 8f;
    public AudioSource slowMoAudio;

    private void Update()
    {
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);
        Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0.0f, 0.02f);
        Time.timeScale += (1f / slowDuration) * Time.unscaledDeltaTime;
        Time.fixedDeltaTime += (0.02f / slowDuration) * Time.unscaledDeltaTime;

    }

    public void SlowMotion()
    {
        Time.timeScale = slowMoValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        slowMoAudio.Play();
    }
}
