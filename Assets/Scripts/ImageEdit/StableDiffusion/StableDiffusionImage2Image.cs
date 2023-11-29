using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class StableDiffusionImage2Image : StableDiffusionGenerator
{
    [SerializeField] private Image originTexture;
    [SerializeField] private Texture2D inputTex;
   
    protected override IEnumerator GenerateAsync()
    {
        isGenerating = true;
        SetupFolders();
        yield return config.SetModelAsync(ModelList[selectedModel]);
        
        string url = config.Settings.serverURL + config.Settings.ImageToImageAPI;
        HttpRequestManager.HttpRequestParams httpParams = new HttpRequestManager.HttpRequestParams();
        if (config.Settings.useAuth && !config.Settings.user.Equals("") && !config.Settings.pass.Equals(""))
        {
            httpParams.PreAuthenticate = true;
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(config.Settings.user + ":" + config.Settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            httpParams.HeaderPairs.Add(new KeyValuePair<string, string>("Authorization", "Basic " + encodedCredentials));
        }
        httpParams.Content = JsonConvert.SerializeObject((SDParamsInImg2Img)GetCurrentSDParamsIn());
        
        var task = HttpRequestManager.PostHttpRequest(url, httpParams); //POST Request
        while (!task.IsCompleted)
            yield return null;
        
        string result = task.Result;
        SDResponseImg2Img json = JsonConvert.DeserializeObject<SDResponseImg2Img>(result);
        if (json.images == null || json.images.Length == 0)
        {
            Debug.LogError("반환된 이미지 없음");
            isGenerating = false;
            yield break;
        }
        
        byte[] imageData = Convert.FromBase64String(json.images[0]);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        texture.Apply();
        ShowGeneratedImage(texture);
        if (json.info != "")
        {
            SDParamsOutTxt2Img info = JsonConvert.DeserializeObject<SDParamsOutTxt2Img>(json.info);
            generatedSeed = info.seed;
        }
        isGenerating = false;
        yield return null;
    }

    protected override SDParamsIn GetCurrentSDParamsIn()
    {
        //Stable Diffusion Parameters Initialization
        byte[] originImgBytes = inputTex.EncodeToPNG();
        Texture2D resizedTex = new Texture2D(2, 2);
        resizedTex.LoadImage(originImgBytes);
        resizedTex.Resize(width, height);
        resizedTex.Apply();
        
        string originImgString = Convert.ToBase64String(resizedTex.EncodeToPNG());

        SDParamsInImg2Img sdParams = new SDParamsInImg2Img();
        sdParams.init_images = new string[] { originImgString };
        sdParams.prompt = prompt;
        sdParams.negative_prompt = negativePrompt;
        sdParams.steps = steps;
        sdParams.cfg_scale = cfgScale;
        sdParams.width = width;
        sdParams.height = height;
        sdParams.seed = seed;
        sdParams.tiling = false;

        if (selectedSampler >= 0 && selectedSampler < SamplersList.Length)
            sdParams.sampler_name = SamplersList[selectedSampler];

        return sdParams;
    }
}
