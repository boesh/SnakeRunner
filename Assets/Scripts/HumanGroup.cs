using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanGroup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<HumanController> humans;

    public void setHumanGroup(Color color, int _colorIndex)
    {
        foreach (HumanController human in humans)
        {
            human.GetComponent<Renderer>().material.color = color;
            human.colorIndex = _colorIndex;
            human.gameObject.SetActive(true);
        }
    }
}
