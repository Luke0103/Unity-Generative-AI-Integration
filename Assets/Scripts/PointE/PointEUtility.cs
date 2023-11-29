using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointEUtility : MonoBehaviour
{
    private const string URL = "https://openai-point-e.hf.space/run/predict";

    [SerializeField] private InputField inputField;
    [SerializeField] private Text outputField;
    [SerializeField] private GameObject loadingObject;
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private float sizeAmplifier = 1f;

    [Serializable]
    private struct Request
    {
        public string[] data;
    }

    [Serializable]
    private struct ColorObject
    {
        public string[] color;
        public int size;
    }

    [Serializable]
    private struct Data
    {
        public ColorObject marker;
        public string markers;
        public float[] x;
        public float[] y;
        public float[] z;
    }

    [Serializable]
    private struct Plot
    {
        public Data[] data;
    }

    [Serializable]
    private struct ResponseItem
    {
        public string type;
        public string plot;
    }

    [Serializable]
    private struct Response
    {
        public ResponseItem[] data;
        public float duration;
    }

    private Color GetColorFromData(string context)
    {
        //"rgb(0.0,0.0,0.0)"
        string numerics = context.Replace("rgb(", "").Replace(")", "");
        string[] coords = numerics.Split(',');

        float r = float.Parse(coords[0]);
        float g = float.Parse(coords[1]);
        float b = float.Parse(coords[2]);

        return new Color(r, g, b, 1f);
    }

    private void CreateObject(Plot plot)
    {
        int childCount = 0;
        if ((childCount = pivot.childCount) > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Destroy(pivot.GetChild(i).gameObject);
            }
        }

        int markerSize = plot.data[0].marker.color.Length;

        for (int i = 0; i < markerSize; i++)
        {
            GameObject marker = Instantiate(markerPrefab, pivot);
            Vector3 localPos = new Vector3(plot.data[0].x[i], plot.data[0].y[i], plot.data[0].z[i]);
            localPos *= sizeAmplifier;
            Color color = GetColorFromData(plot.data[0].marker.color[i]);

            MeshRenderer renderer = marker.GetComponent<MeshRenderer>();
            renderer.material.color = color;

            marker.transform.localPosition = localPos;
        }
    }

    private IEnumerator GetResponse(string prompt)
    {
        Request request;
        request.data = new string[] { prompt };

        var post = HttpRequestManager.PostHttpRequest(URL, JsonUtility.ToJson(request));

        loadingObject.SetActive(true);
        while (!post.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }
        loadingObject.SetActive(false);

        string json = post.Result;
        Response data = JsonUtility.FromJson<Response>(json);
        Plot plot = JsonUtility.FromJson<Plot>(data.data[0].plot);
        Debug.Log(plot.data[0].x[0]);

        outputField.text = json;
        CreateObject(plot);
    }

    public void InvokeModelCreation()
    {
        StartCoroutine(GetResponse(inputField.text));
    }
}
