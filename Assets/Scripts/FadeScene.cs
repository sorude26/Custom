using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeScene : MonoBehaviour
{
    public bool firstFadeInEnd;
    public Image fadeScene = null;
    private int frameCount = 0;
    private float timer = 0.0f;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool endFadeIn = false;
    private bool endFadeOut = false;

    public void StartFadeIn()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        endFadeIn = false;
        timer = 0.0f;
        fadeScene.color = new Color(1, 1, 1, 1);
        fadeScene.fillAmount = 1;
        fadeScene.raycastTarget = true;
    }
    public bool IsFadeInEnd()
    {
        return endFadeIn;
    }
    public void StartFadeOut()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        endFadeOut = false;
        timer = 0.0f;
        fadeScene.color = new Color(1, 1, 1, 0);
        fadeScene.fillAmount = 0;
        fadeScene.raycastTarget = true;
    }
    public bool IsFadeOutEnd()
    {
        return endFadeOut;
    }
    void Start()
    {
        fadeScene = GetComponent<Image>();
        if (firstFadeInEnd)
        {
            FadeInEnd();
        }
        else
        {
            StartFadeIn();
        }
    }

    void Update()
    {
        if (frameCount > 5)
        {
            if (fadeIn)
            {
                FadeInUpdate();
            }
            else if (fadeOut)
            {
                FadeOutUpdate();
            }
        }
        frameCount++;
    }

    private void FadeInUpdate()
    {
        if (timer < 1f)
        {
            fadeScene.color = new Color(1, 1, 1, 1 - timer);
            fadeScene.fillAmount = 1 - timer;
        }
        else
        {
            FadeInEnd();
        }
        timer += Time.deltaTime;
    }

    private void FadeOutUpdate()
    {
        if (timer < 1f)
        {
            fadeScene.color = new Color(1, 1, 1, timer);
            fadeScene.fillAmount = timer;
        }
        else
        {
            FadeOutEnd();
        }
        timer += Time.deltaTime;
    }


    private void FadeInEnd()
    {
        fadeScene.color = new Color(1, 1, 1, 0);
        fadeScene.fillAmount = 0;
        fadeScene.raycastTarget = false;
        timer = 0.0f;
        fadeIn = false;
        endFadeIn = true;
    }

    private void FadeOutEnd()
    {
        fadeScene.color = new Color(1, 1, 1, 1);
        fadeScene.fillAmount = 1;
        fadeScene.raycastTarget = false;
        timer = 0.0f;
        fadeOut = false;
        endFadeOut = true;
    }
}
