using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeatAlertController : MonoBehaviour
{
    public Image alertOverlay;
    public Text statusText;

    private Coroutine activeRoutine;

    public void OnHelpButtonPressed()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }
        activeRoutine = StartCoroutine(AlertRoutine());
    }

    private IEnumerator AlertRoutine()
    {
        if (statusText != null)
        {
            statusText.text = "ALERT: Help Requested - Contacting Caregiver";
            statusText.color = new Color(0.8f, 0.1f, 0.1f);
        }

        float duration = 3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float pulse = Mathf.PingPong(Time.time * 3f, 1f) * 0.45f;
            if (alertOverlay != null)
            {
                Color c = alertOverlay.color;
                c.a = pulse;
                alertOverlay.color = c;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (alertOverlay != null)
        {
            Color c = alertOverlay.color;
            c.a = 0f;
            alertOverlay.color = c;
        }

        if (statusText != null)
        {
            statusText.text = "Status: Normal";
            statusText.color = Color.black;
        }

        activeRoutine = null;
    }
}
