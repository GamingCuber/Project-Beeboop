using System.Collections;
using UnityEngine;

public class AbilityCooldownIcon : MonoBehaviour
{
    public GameObject cover;

    private bool shouldReveal = true;

    public void upgradeOwned() 
    {
        shouldReveal = false;
    }

    public void reveal()
    {
        if (!shouldReveal) return;
        this.transform.localPosition = this.transform.localPosition + Vector3.down * 200;
        StartCoroutine(moveIcon(this.transform.localPosition + Vector3.up * 200));
    }

    public void showCover() 
    {
        cover.SetActive(true);
    }

    public void hideCover() 
    {
        cover.SetActive(false);
    }

    private IEnumerator moveIcon(Vector3 targetPos)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Vector3 initPos = this.transform.localPosition;

        float timer = 0;
        float moveTime = 1;

        while(timer < moveTime)
        {
            timer += Time.deltaTime;

            this.transform.localPosition = Vector2.Lerp(initPos, targetPos, timer/moveTime);

            yield return wait;
        }
    }
}
