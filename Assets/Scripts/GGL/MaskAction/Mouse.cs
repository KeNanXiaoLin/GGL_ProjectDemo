using System.Collections;
using System.Collections.Generic;
using KNXL;
using UnityEngine;

public class Mouse : BaseAction
{
    protected override void PlayControlSound()
    {
        base.PlayControlSound();
        ResLoadMgr.Instance.LoadRes<AudioClip>(40012,(clip)=>
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.loop = true;
        });
    }
}
