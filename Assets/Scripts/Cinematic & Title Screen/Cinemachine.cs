using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public CinemachineBasicMultiChannelPerlin noise;
    void Start()
    {
        CinemachineVirtualCamera cinemachine = GetComponent<CinemachineVirtualCamera>();
        noise = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void lightShake()
    {
        noise.m_AmplitudeGain = 0.5f;
        noise.m_FrequencyGain = 1f;
        StartCoroutine(ResetLight(0.5f));
    }
    IEnumerator ResetLight(float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
    public void heavyShake()
    {
        noise.m_AmplitudeGain = 2f;
        noise.m_FrequencyGain = 2f;
        StartCoroutine(ResetHeavy(0.5f));
    }
    IEnumerator ResetHeavy(float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}