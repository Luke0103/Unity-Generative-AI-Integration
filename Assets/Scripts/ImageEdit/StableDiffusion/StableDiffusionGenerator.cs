using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public abstract class StableDiffusionGenerator : MonoBehaviour
{
    protected static StableDiffusionConfiguration config = null;
    private Coroutine updateProgressCoroutine = null;
    [SerializeField] protected Image targetImage;
    [SerializeField] protected TMP_InputField promptInputField;
    [SerializeField] protected TMP_InputField negativePromptInputField;

    protected readonly string sdImagePath = "SDImages";
    protected readonly string pngFormat = ".png";

    protected string prompt;
    protected string negativePrompt;
    [SerializeField]
    protected string[] SamplersList
    {
        get
        {
            if (config == null)
                config = FindObjectOfType<StableDiffusionConfiguration>();
            return config.Samplers;
        }
    }
    [SerializeField]
    protected string[] ModelList
    {
        get
        {
            if (config == null)
                config = FindObjectOfType<StableDiffusionConfiguration>();
            return config.ModelNames;
        }
    }

    protected int selectedSampler = 0;
    protected int selectedModel = 0;
    protected int width = 512;
    protected int height = 512;
    protected int steps = 90;
    protected float cfgScale = 7;
    protected long seed = -1;
    protected long generatedSeed = -1;
    protected bool isGenerating = false;
    protected string fileName = "";

    public void Generate()
    {
        if (!string.IsNullOrEmpty(prompt))
            SetPromptByInputField();
        if (!isGenerating)
            StartCoroutine(GenerateAsync());
    }

    protected void SetupFolders()
    {
        if (config == null)
            config = FindObjectOfType<StableDiffusionConfiguration>();

        try
        {
            string root = Application.dataPath + config.Settings.OutputFolder;
            if (root == "" || !Directory.Exists(root))
                root = Application.streamingAssetsPath;
            string mat = Path.Combine(root, sdImagePath);
            fileName = mat + pngFormat;

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            if (!Directory.Exists(mat))
                Directory.CreateDirectory(mat);

            if (File.Exists(fileName))
                File.Delete(fileName);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n\n{e.StackTrace}");
        }
    }

    protected void ShowGeneratedImage(Texture2D texture)
    {
        try
        {
            if (targetImage == null)
            {
                if (!TryGetComponent(out targetImage))
                {
                    if (TryGetComponent(out RawImage rimg))
                        rimg.texture = texture;
                    return;
                }
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            targetImage.sprite = sprite;
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n\n{e.StackTrace}");
        }
    }

    public virtual void SetPromptByInputField()
    {
        prompt = promptInputField.text;
        negativePrompt = negativePromptInputField.text;
    }

    protected abstract SDParamsIn GetCurrentSDParamsIn();
    protected abstract IEnumerator GenerateAsync();
}
