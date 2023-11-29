using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;

public class ParticleSerializer : MonoBehaviour
{
    public GameObject particlePrefab;
    private ParticleSystem particle;
    private ParticleSystemRenderer particleRenderer;

    private void Start()
    {
        particle = particlePrefab.GetComponent<ParticleSystem>();
        particleRenderer = particlePrefab.GetComponent<ParticleSystemRenderer>();
        Debug.Log(particleRenderer);
    }
}
