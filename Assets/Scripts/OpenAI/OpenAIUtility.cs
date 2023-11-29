using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OpenAIUtility : MonoBehaviour
{
    private const string URL = "https://api.openai.com/v1/chat/completions";

    [Serializable]
    private struct ResponseMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    private struct ResponseChoice
    {
        public int index;
        public ResponseMessage message;
    }

    [Serializable]
    private struct Response
    {
        public string id;
        public ResponseChoice[] choices;
    }

    [Serializable]
    private struct RequestMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    private struct Request
    {
        public string model;
        public RequestMessage[] messages;
    }

    [SerializeField] private InputField inputField;
    [SerializeField] private Text outputField;
    [SerializeField] private GameObject loadingObject;

    private string CreateRequestBody(string prompt)
    {
        RequestMessage msg;
        msg.role = "user";
        msg.content = prompt;

        Request req;
        req.model = "gpt-3.5-turbo";
        req.messages = new[] { msg };

        return JsonUtility.ToJson(req);
    }

    private IEnumerator GetResponse(string prompt)
    {
        OpenAISetup settings = OpenAISetup.instance;

        HttpRequestManager.HttpRequestParams httpParams = new HttpRequestManager.HttpRequestParams();
        List<KeyValuePair<string, string>> headerPairs = new List<KeyValuePair<string, string>>();
        headerPairs.Add(new KeyValuePair<string, string>("Authorization", "Bearer " + settings.apiKey));
        httpParams.HeaderPairs = headerPairs;
        httpParams.Content = CreateRequestBody(prompt);

        var post = HttpRequestManager.PostHttpRequest(URL, httpParams);

        loadingObject.SetActive(true);
        while (!post.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }
        loadingObject.SetActive(false);

        string json = post.Result;
        Response data = JsonUtility.FromJson<Response>(json);

        outputField.text = data.choices[0].message.content;
    }

    public void InvokeChat()
    {
        StartCoroutine(GetResponse(inputField.text));
    }
}
