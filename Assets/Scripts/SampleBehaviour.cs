using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Misc/SampleBehaviour")]
public class SampleBehaviour : MonoBehaviour
{
    [Tooltip("Number")]
    [ContextMenuItem("Random", "RandomNumber")]
    [SerializeField] private int num;

    [ColorUsage(true, true)]
    [SerializeField] private Color color;

    [ContextMenu("RandomNumber")]
    private void RandomNumber()
    {
        num = Random.Range(1, 100);
    }
}
