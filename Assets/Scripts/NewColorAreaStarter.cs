using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewColorAreaStarter : MonoBehaviour
{
    [SerializeField]
    public Color color;
    [SerializeField]
    private ParticleSystem particleSys;
    public int colorIndex;
    public bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SnakeTail") 
        {
            other.gameObject.GetComponent<Renderer>().material.color = color;
            Debug.Log("tail");
        }
        if (other.tag == "Player")
        {
            Debug.Log("player");
        }
    }

    public void SetColor()
    {
        var main = particleSys.main;
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(color.r,color.g,color.b));
    }

    // Start is called before the first frame update
    void Start()
    {
        SetColor();
    }
}
