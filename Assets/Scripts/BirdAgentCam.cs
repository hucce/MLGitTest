using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAgentCam : MonoBehaviour
{
    [SerializeField]
    private GameObject birdAgent = null;

    private void Awake()
    {
        gameObject.transform.position = new Vector3(birdAgent.transform.position.x, birdAgent.transform.position.y, gameObject.transform.position.z);
    }

    private void LateUpdate()
    {
        gameObject.transform.position = new Vector3(birdAgent.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}