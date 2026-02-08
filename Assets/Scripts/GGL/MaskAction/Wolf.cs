using System.Collections;
using System.Collections.Generic;
using KNXL;
using UnityEngine;

public class Wolf : BaseAction
{
    protected override void PlayControlSound()
    {
        base.PlayControlSound();
        ResLoadMgr.Instance.LoadRes<AudioClip>(40011,(clip)=>
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.loop = true;
        });
    }
}
