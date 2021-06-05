using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoAndCrystalsController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objects;

    public void Settings()
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
