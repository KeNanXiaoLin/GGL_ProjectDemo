using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KNXL
{
    /// <summary>
    /// 场景切换管理器 主要用于切换场景
    /// </summary>
    public class SceneMgr : BaseManager<SceneMgr>
    {
        private SceneMgr() { }

        //同步切换场景的方法
        public void LoadScene(string name, Action callBack = null)
        {
            //切换场景
            SceneManager.LoadScene(name);
            //调用回调
            callBack?.Invoke();
            callBack = null;
        }

        /// <summary>
        /// 异步切换场景的方法
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="beforeLoad">加载前的回调 可以做一些加载前的操作 淡出当前场景</param>
        /// <param name="callBack">加载完成后的回调 可以做一些加载完成后的操作 淡入新场景</param>
        public void LoadSceneAsyn(string name, Func<IEnumerator> beforeLoad = null, Func<IEnumerator> afterLoad = null)
        {
            MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(name, beforeLoad, afterLoad));
        }

        private IEnumerator ReallyLoadSceneAsyn(string name, Func<IEnumerator> beforeLoad = null, Func<IEnumerator> afterLoad = null)
        {
            yield return beforeLoad?.Invoke();
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            //不停的在协同程序中每帧检测是否加载结束 如果加载结束就不会进这个循环每帧执行了
            while (!ao.isDone)
            {
                //可以在这里利用事件中心 每一帧将进度发送给想要得到的地方
                EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, ao.progress);
                yield return 0;
            }
            //避免最后一帧直接结束了 没有同步1出去
            EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, 1);

            yield return afterLoad?.Invoke();
        }
    }
}