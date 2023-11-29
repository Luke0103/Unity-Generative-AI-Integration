using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class StableDiffusionConfiguration : MonoBehaviour
{
    [System.Serializable]
    private struct Model
    {
        public string title;
        public string model_name;
        public string hash;
        public string sha256;
        public string filename;
        public string config;
    }

    [SerializeField]
    private StableDiffusionSettings settings;

    [SerializeField]
    private string[] samplers = new string[]
    {
        "Euler a", "Euler", "LMS", "Heun", "DPM2", "DPM2 a", "DPM++ 2S a", "DPM++ 2M", "DPM++ SDE", "DPM fast", "DPM adaptive",
        "LMS Karras", "DPM2 Karras", "DPM2 a Karras", "DPM++ 2S a Karras", "DPM++ 2M Karras", "DPM++ SDE Karras", "DDIM", "PLMS"
    };

    [SerializeField]
    private string[] modelNames;

    public string[] ModelNames => modelNames;
    public string[] Samplers => samplers;
    public StableDiffusionSettings Settings => settings;

    private void Start()
    {
        ListModels();
    }

    public void ListModels()
    {
        StartCoroutine(ListModelsAsync());
        if (modelNames != null)
        {
            for (int i = 0; i < modelNames.Length; i++)
                Debug.Log(modelNames[i]);
        }
    }

    private IEnumerator ListModelsAsync()
    {
        string url = settings.serverURL + settings.ModelAPI;

        HttpRequestManager.HttpRequestParams httpParams = new HttpRequestManager.HttpRequestParams();
        if (settings.useAuth && !string.IsNullOrEmpty(settings.user) && !string.IsNullOrEmpty(settings.pass))
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes($"{settings.user}:{settings.pass}");
            httpParams.HeaderPairs.Add(new KeyValuePair<string, string>("Authorization", $"Basic {System.Convert.ToBase64String(bytesToEncode)}"));
        }
        var task = HttpRequestManager.GetHttpRequest(url, httpParams);

        while (!task.IsCompleted)
            yield return null;

        try
        {
            Model[] models = JsonConvert.DeserializeObject<Model[]>(task.Result);
            List<string> names = new List<string>();
            foreach (Model m in models)
                names.Add(m.model_name);
            modelNames = names.ToArray();
        }
        catch
        {
            Debug.LogError(task.Result);
            Debug.LogError("Server needs API key authentication");
        }
    }

    public IEnumerator SetModelAsync(string modelName)
    {
        string url = settings.serverURL + settings.OptionAPI;
        if (modelNames == null || modelNames.Length <= 0)
            yield return ListModelsAsync();

        HttpRequestManager.HttpRequestParams httpParams = new HttpRequestManager.HttpRequestParams();
        if (settings.useAuth && !string.IsNullOrEmpty(settings.user) && !string.IsNullOrEmpty(settings.pass))
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes($"{settings.user}:{settings.pass}");
            httpParams.HeaderPairs.Add(new KeyValuePair<string, string>("Authorization", $"Basic {System.Convert.ToBase64String(bytesToEncode)}"));
        }

        SDOption option = new SDOption();
        option.sd_model_checkpoint = modelName;
        httpParams.Content = JsonConvert.SerializeObject(option);

        var task = HttpRequestManager.PostHttpRequest(url, httpParams);
        while (!task.IsCompleted)
            yield return null;

        string result = task.Result;
        yield break;
    }
}
