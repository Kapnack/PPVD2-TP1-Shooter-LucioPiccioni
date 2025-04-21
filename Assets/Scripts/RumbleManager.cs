using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : Singleton<RumbleManager>
{
    private Gamepad pad;

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        pad = Gamepad.current;

        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);

            StartCoroutine(StartRumble(duration));
        }
    }

    private IEnumerator StartRumble(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
