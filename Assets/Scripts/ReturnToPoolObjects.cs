using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolObjects : MonoBehaviour
{
    [SerializeField]
    GameObject snake;
    [SerializeField]
    int deactivateDistance = 20;

    private void Awake()
    {
        snake = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (transform.position.z < snake.transform.position.z - deactivateDistance)
        {
            gameObject.SetActive(false);
        }
    }
}
