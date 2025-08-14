using UnityEngine;
using System.Collections;

public class CameraZoomManager : MonoBehaviour
{
    public static CameraZoomManager Instance;

    private Camera cam;

    private bool zoomed = false;

    private float defaultZoom = 10f;

    public float zoomTime;

    private bool zoomOnCD = false;

    private Coroutine zoomCo;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void doZoom(float size)
    {
        if (!zoomOnCD)
        {
            zoomOnCD = true;

            if (!zoomed)
            {
                zoomCo = StartCoroutine(zoomCamera(defaultZoom, size));
            }
            else
            {
                zoomCo = StartCoroutine(zoomCamera(size, defaultZoom));
            }

            zoomed = !zoomed;

            Invoke(nameof(toggleCD), zoomTime + 0.1f);
        }
    }

    public void resetZoom()
    {
        if (zoomCo != null)
        {
            StopCoroutine(zoomCo);
        }
        zoomCo = null;

        zoomed = false;
        cam.orthographicSize = defaultZoom;
    }

    private void toggleCD()
    {
        zoomOnCD = false;
    }

    private IEnumerator zoomCamera(float start, float end)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        while (timer < zoomTime)
        {
            timer += Time.deltaTime;

            float size = Mathf.Lerp(start, end, timer / zoomTime);

            cam.orthographicSize = size;

            yield return wait;
        }

        yield break;
    }
}
