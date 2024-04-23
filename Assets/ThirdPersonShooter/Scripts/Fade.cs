using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;


public class Fade : MonoBehaviour
{

    private static int instanceCount;

    private RawImage rawImage;

    private float fadeTime = 0.3f;
    private Color color = Color.black;

    private float startTime;

    private Mode mode = Mode.Wait;
    private List<Action<Fade>> onFadeOutCompleteList = new List<Action<Fade>>();
    private List<Action<Fade>> onFadeInCompleteList = new List<Action<Fade>>();
    private Texture2D texture2D;

    private enum Mode
    {
        FadeIn,
        FadeOut,
        Sleep,
        Wait,
        End,
    }

    void OnDestroy()
    {
        if (texture2D != null)
        {
            Destroy(texture2D);
        }
    }

    public static Fade LoadScene(string sceneName)
    {
        return OutIn().AddOnFadeOutComplete((Fade fade) => {
            SceneManager.LoadScene(sceneName);
        });
    }

    public static Fade LoadScene(int sceneBuildIndex)
    {
        return OutIn().AddOnFadeOutComplete((Fade fade) => {
            SceneManager.LoadScene(sceneBuildIndex);
        });
    }

    public static Fade OutIn()
    {
        return Out().AddOnFadeOutComplete((Fade fade) => {
            fade.In();
        });
    }

    public static Fade Out()
    {
        Fade fade = createInstance();
        fade.mode = Mode.FadeOut;
        fade.startTime = 0.0f;
        return fade;
    }

    public static bool IsFading()
    {
        return instanceCount > 0;
    }


    private static Fade createInstance()
    {
        GameObject canvasGO = new GameObject("~Fade");
        Fade fade = canvasGO.AddComponent<Fade>();
        fade.initComponents();

        DontDestroyOnLoad(canvasGO);
        instanceCount++;

        return fade;
    }


    public Fade AddOnFadeOutComplete(Action<Fade> onFadeOutComplete)
    {
        this.onFadeOutCompleteList.Add(onFadeOutComplete);
        return this;
    }

    public Fade AddOnFadeInComplete(Action<Fade> onFadeInComplete)
    {
        this.onFadeInCompleteList.Add(onFadeInComplete);
        return this;
    }

    public Fade SetFadeTime(float fadeTime)
    {
        this.fadeTime = fadeTime;
        return this;
    }

    public Fade SetColor(Color color)
    {
        this.color = color;
        rawImage.color = new Color(color.r, color.g, color.b, 0.0f);
        return this;
    }

    public Fade In()
    {
        this.mode = Mode.FadeIn;
        this.startTime = 0.0f;
        return this;
    }


    // Use this for initialization
    void Start()
    {
        In();
    }

    // Update is called once per frame
    void Update()
    {

        if (startTime == 0.0f)
        {
            startTime = Time.realtimeSinceStartup;
            return;
        }

        switch (mode)
        {
            case Mode.FadeOut:
                {
                    float diffTime = Time.realtimeSinceStartup - startTime;
                    if (diffTime >= fadeTime)
                    {
                        rawImage.color = new Color(color.r, color.g, color.b, 1.0f);
                        mode = Mode.Sleep;

                        foreach (Action<Fade> action in onFadeOutCompleteList)
                        {
                            action.Invoke(this);
                        }

                    }
                    else
                    {
                        rawImage.color = new Color(color.r, color.g, color.b, diffTime / fadeTime);
                    }
                    break;
                }

            case Mode.FadeIn:
                {
                    float diffTime = Time.realtimeSinceStartup - startTime;
                    if (diffTime >= fadeTime)
                    {
                        rawImage.color = new Color(color.r, color.g, color.b, 0.0f);
                        mode = Mode.End;

                        foreach (Action<Fade> action in onFadeInCompleteList)
                        {
                            action.Invoke(this);
                        }
                    }
                    else
                    {
                        rawImage.color = new Color(color.r, color.g, color.b, 1.0f - diffTime / fadeTime);
                    }
                }
                break;

            case Mode.End:
                Destroy(gameObject);
                instanceCount--;
                break;

            case Mode.Sleep:
                break;
        }
    }


    private void initComponents()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        gameObject.AddComponent<GraphicRaycaster>();

        GameObject imageGO = new GameObject("~Fade_Image");
        rawImage = imageGO.AddComponent<RawImage>();
        imageGO.transform.SetParent(transform, false);
        RectTransform rtf = imageGO.GetComponent<RectTransform>();
        rtf.anchorMin = Vector2.zero;
        rtf.anchorMax = Vector2.one;
        rtf.offsetMin = Vector2.zero;
        rtf.offsetMax = Vector2.zero;

        texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture2D.SetPixels(0, 0, 1, 1, new Color[] { Color.white });
        texture2D.Apply(true, true);
        rawImage.texture = texture2D;
        rawImage.color = new Color(color.r, color.g, color.b, 0.0f);
    }

}