using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class RumbleManager : Singleton<RumbleManager>
{
    private Gamepad pad;
    [SerializeField] readonly float timeBetweenBlinks;

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        pad = Gamepad.current;

        if (pad != null)
        {
            if (pad is DualShock4GamepadHID dualShock)
            {
                dualShock.SetMotorSpeedsAndLightBarColor(lowFrequency, highFrequency, Color.clear);
                StartCoroutine(BlinkLightbar(dualShock, Color.white, Color.clear, 0.10f, 5));
            }
            else
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

    IEnumerator BlinkLightbar(DualShock4GamepadHID ds, Color color1, Color color2, float interval, float maxTimes)
    {
        float currentBlinks = 0f;

        while (currentBlinks < maxTimes)
        {
            ds.SetLightBarColor(color1);
            yield return new WaitForSeconds(interval);

            ds.SetLightBarColor(color2);
            yield return new WaitForSeconds(interval);

            currentBlinks++;
        }


        ds.SetLightBarColor(Color.clear);
    }
}
