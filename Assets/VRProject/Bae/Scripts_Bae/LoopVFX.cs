using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopVFX : MonoBehaviour
{
    public ParticleSystem vfx;
    public float interval = 15f;

    private void Start()
    {
        InvokeRepeating(nameof(PlayVFX), 0f, interval);
    }

    void PlayVFX()
    {
        vfx.Play();  // 다시 실행
    }
}
