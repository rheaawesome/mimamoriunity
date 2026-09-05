using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProductTourController : MonoBehaviour
{
    public Camera tourCamera;
    public Text captionText;
    public Image captionPanel;
    public HeatAlertController alertController;

    public Transform[] highlightTargets;
    public Color highlightColor = new Color(1f, 0.45f, 0.1f);
    public float highlightIntensity = 2.5f;

    private Vector3[] camPositions;
    private Vector3[] lookAtTargets;
    private string[] captions;

    private Renderer[] highlightRenderers;
    private Color[] originalColors;

    public float travelTime = 1.8f;
    public float holdTime = 2.6f;

    private Vector3 startPos;
    private Quaternion startRot;

    void Awake()
    {
        camPositions = new Vector3[] {
            new Vector3(13.6f, 2.3f, -3.2f),
            new Vector3(10.6f, 2.1f, -3.0f),
            new Vector3(11.6291f, 1.6f, 6.0f),
            new Vector3(11.6291f, -1.5f, -3.0f),
            new Vector3(16.2f, 2.0f, -3.3f),
            new Vector3(14.3f, 1.8f, -3.0f)
        };

        lookAtTargets = new Vector3[] {
            new Vector3(12.6f, 0.95f, 1.31f),
            new Vector3(10.9f, 0.95f, 1.31f),
            new Vector3(11.6291f, 0.9f, 2.6f),
            new Vector3(11.6291f, 0.3f, 1.31f),
            new Vector3(16.2f, 0.6f, 1.31f),
            new Vector3(14.3f, 0.6f, 1.31f)
        };

        captions = new string[] {
            "Speaker",
            "Help button",
            "Ambient sensor vents",
            "PPG and skin-temp sensor plus charging contacts",
            "Stainless steel buckle",
            "Strap keeper loop"
        };

        highlightRenderers = new Renderer[highlightTargets.Length];
        originalColors = new Color[highlightTargets.Length];
        for (int idx = 0; idx < highlightTargets.Length; idx++)
        {
            if (highlightTargets[idx] != null)
            {
                Renderer rend = highlightTargets[idx].GetComponentInChildren<Renderer>();
                highlightRenderers[idx] = rend;
                if (rend != null)
                {
                    Material mat = rend.material;
                    originalColors[idx] = mat.HasProperty("_BaseColor") ? mat.GetColor("_BaseColor") : mat.color;
                }
            }
        }
    }

    void Start()
    {
        if (tourCamera == null)
        {
            tourCamera = Camera.main;
        }
        startPos = tourCamera.transform.position;
        startRot = tourCamera.transform.rotation;
        StartCoroutine(RunTour());
    }

    public void PlayTour()
    {
        StopAllCoroutines();
        tourCamera.transform.position = startPos;
        tourCamera.transform.rotation = startRot;
        StartCoroutine(RunTour());
    }

    private IEnumerator RunTour()
    {
        yield return new WaitForSeconds(1.0f);

        for (int idx = 0; idx < camPositions.Length; idx++)
        {
            yield return MoveCamera(camPositions[idx], lookAtTargets[idx]);

            if (captionText != null)
            {
                captionText.text = captions[idx];
            }

            SetHighlight(idx, true);

            yield return FadePanel(0.75f, 0.4f);
            yield return new WaitForSeconds(holdTime);
            yield return FadePanel(0f, 0.3f);

            SetHighlight(idx, false);
        }

        if (captionText != null)
        {
            captionText.text = "Watch it in action";
        }
        yield return FadePanel(0.75f, 0.4f);
        yield return new WaitForSeconds(1.2f);

        if (alertController != null)
        {
            alertController.OnHelpButtonPressed();
        }

        yield return new WaitForSeconds(4.0f);
        yield return FadePanel(0f, 0.4f);
        if (captionText != null)
        {
            captionText.text = "";
        }

        yield return MoveCamera(startPos, startPos + Vector3.forward * 10f);
    }

    private void SetHighlight(int idx, bool on)
    {
        if (idx < 0 || idx >= highlightRenderers.Length)
        {
            return;
        }
        Renderer rend = highlightRenderers[idx];
        if (rend == null)
        {
            return;
        }
        Material mat = rend.material;
        if (on)
        {
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", highlightColor);
            }
            else
            {
                mat.color = highlightColor;
            }
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", highlightColor * highlightIntensity);
        }
        else
        {
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", originalColors[idx]);
            }
            else
            {
                mat.color = originalColors[idx];
            }
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }

    private IEnumerator MoveCamera(Vector3 toPos, Vector3 lookAt)
    {
        Vector3 fromPos = tourCamera.transform.position;
        Quaternion fromRot = tourCamera.transform.rotation;
        Vector3 heading = lookAt - toPos;
        if (heading.sqrMagnitude < 0.0001f)
        {
            heading = tourCamera.transform.forward;
        }
        Quaternion toRot = Quaternion.LookRotation(heading.normalized, Vector3.up);

        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float pct = Mathf.SmoothStep(0f, 1f, elapsed / travelTime);
            tourCamera.transform.position = Vector3.Lerp(fromPos, toPos, pct);
            tourCamera.transform.rotation = Quaternion.Slerp(fromRot, toRot, pct);
            yield return null;
        }
        tourCamera.transform.position = toPos;
        tourCamera.transform.rotation = toRot;
    }

    private IEnumerator FadePanel(float targetAlpha, float duration)
    {
        if (captionPanel == null)
        {
            yield break;
        }
        float startAlpha = captionPanel.color.a;
        float elapsed2 = 0f;
        while ( elapsed2 < duration)
        {
            elapsed2 += Time.deltaTime;
            Color c = captionPanel.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed2 / duration);
            captionPanel.color = c;
            yield return null;
        }
        Color c2 = captionPanel.color;
        c2.a = targetAlpha;
        captionPanel.color = c2;
    }
}
