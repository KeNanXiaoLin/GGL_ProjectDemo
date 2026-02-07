using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KNXL
{
    public class ResLoadMgr : BaseManager<ResLoadMgr>
    {
        // 获取全局配置
        private static ResLoadSettings Settings => ResLoadSettings.Instance;

        private ResLoadMgr() { }

        /// <summary>
        /// 真正加载资源的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath"></param>
        /// <param name="callBack"></param>
        /// <param name="isSync"></param>
        private void ReallyLoadRes<T>(string resPath, UnityAction<T> callBack, bool isSync = true) where T : Object
        {
            T res = null;
            if (isSync)
            {
                switch (Settings.resLoadType)
                {
                    case E_ResLoadType.Editor:
                        res = EditorResMgr.Instance.LoadEditorResWithoutSuffix<T>(resPath);
                        callBack?.Invoke(res);
                        break;
                    // 因为AB包不支持同步加载，所以这里使用Resource的方式进行加载
                    case E_ResLoadType.AB:
                        var tmp = resPath.Split("/");
                        string abName = tmp[0];
                        string resName = tmp[1];
                        ABMgr.Instance.LoadResAsync<T>(abName,resName,callBack,isSync);
                        break;
                    case E_ResLoadType.Resources:
                        res = ResMgr.Instance.Load<T>(resPath);
                        callBack?.Invoke(res);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Settings.resLoadType)
                {
                    case E_ResLoadType.Editor:
                        res = EditorResMgr.Instance.LoadEditorResWithoutSuffix<T>(resPath);
                        callBack?.Invoke(res);
                        break;
                    // 因为AB包不支持同步加载，所以这里使用Resource的方式进行加载
                    case E_ResLoadType.AB:
                        var tmp = resPath.Split("/");
                        string abName = tmp[0];
                        string resName = tmp[1];
                        ABMgr.Instance.LoadResAsync<T>(abName,resName,callBack,isSync);
                        break;
                    case E_ResLoadType.Resources:
                        ResMgr.Instance.LoadAsync<T>(resPath, callBack);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 根据配置表的主键进行资源加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">配置表的主键</param>
        /// <param name="callBack">加载完成后的回调</param>
        /// <param name="isSync">是否同步加载</param>
        public void LoadRes<T>(int primaryKey,UnityAction<T> callBack, bool isSync = false) where T : Object
        {
            string resPath = ResConfigManager.Instance.GetResLoadPath(primaryKey);
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError($"资源ID{primaryKey}对应的配置不存在");
                return;
            }
            ReallyLoadRes<T>(resPath, callBack, isSync);
        }

        /// <summary>
        /// 根据配置表的资源名称进行资源加载,这里基本只会用来记载UI资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <param name="callBack"></param>
        /// <param name="isSync"></param>
        public void LoadRes<T>(string resName,UnityAction<T> callBack, bool isSync = false) where T : Object
        {
            string resPath = ResConfigManager.Instance.GetResLoadPath(resName);
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError($"资源名称{resName}对应的配置不存在");
                return;
            }
            ReallyLoadRes<T>(resPath, callBack, isSync);
        }
    }
}
