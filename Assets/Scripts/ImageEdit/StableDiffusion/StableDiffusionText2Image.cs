using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class StableDiffusionText2Image : StableDiffusionGenerator
{
    protected override IEnumerator GenerateAsync()
    {
        isGenerating = true;
        SetupFolders();
        yield return config.SetModelAsync(ModelList[selectedModel]);
        string url = config.Settings.serverURL + config.Settings.TextToImageAPI;

        HttpRequestManager.HttpRequestParams httpParams = new HttpRequestManager.HttpRequestParams();
        if (config.Settings.useAuth && !config.Settings.user.Equals("") && !config.Settings.pass.Equals(""))
        {
            httpParams.PreAuthenticate = true;
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(config.Settings.user + ":" + config.Settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            httpParams.HeaderPairs.Add(new KeyValuePair<string, string>("Authorization", "Basic " + encodedCredentials));
        }
        httpParams.Content = JsonConvert.SerializeObject((SDParamsInTxt2Img)GetCurrentSDParamsIn()); //POST Body

        var task = HttpRequestManager.PostHttpRequest(url, httpParams); //POST Request
        while (!task.IsCompleted)
            yield return null;

        string result = task.Result;
        SDResponseTxt2Img json = JsonConvert.DeserializeObject<SDResponseTxt2Img>(result);
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
        SDParamsInTxt2Img sdParams = new SDParamsInTxt2Img();
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
