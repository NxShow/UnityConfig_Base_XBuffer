/**************************************************************
 *  Filename:    XConfigMgr.cs
 *  Copyright:   Microsoft Co., Ltd.
 *
 *  Description: XConfigMgr ClassFile.
 *               未启用 ， 因为改动太多 可以在下个项目中使用这玩意
 *  @author:     xiaobai
 *  @version     2018/6/4 14:58:50  @Reviser  Initial Version
 **************************************************************/

using UnityEngine;
using System.Collections;
using System.Threading;

public class XConfigMgr : Singleton<XConfigMgr>
{
    private static readonly string CONFIG_NAME = "ConfigData";

    private xbuffer.Config _config;

    public xbuffer.Config Config
    {
        get
        {
            return _config;
        }
    }

    #region Async load
    int _asyncTaskId = 0;

    public int AsyncTaskId
    {
        get { return _asyncTaskId; }
        set
        {
            Interlocked.Exchange(ref _asyncTaskId, value);
        }
    }

    public void LoadConfigAsync()
    {
        var data = ResourceManager.Instance.LoadPrefab<TextAsset>(CONFIG_NAME);
        if (data != null)
        {
            AsyncTaskId = GameInitializer.Instance.AddAsyncTask(LoadConfigCallback, data.bytes);
            ResourceManager.Instance.UnloadPrefab(CONFIG_NAME);
        }
    }

    void LoadConfigCallback(object threadContext)
    {
        var json = threadContext as byte[];
        OnLoadFinished(json);
        AsyncTaskId = 0;
    }
    #endregion Async load

    public void LoadConfig()
    {
        var data = ResourceManager.Instance.LoadPrefab<TextAsset>(CONFIG_NAME);
        if (data != null)
        {
            OnLoadFinished(data.bytes);
            ResourceManager.Instance.UnloadPrefab(CONFIG_NAME);
        }
    }

    private void OnLoadFinished(byte[] data)
    {
        var bytes = Ionic.Zlib.GZipStream.UncompressBuffer(data);
        uint offset = 0;
        _config = xbuffer.ConfigBuffer.deserialize(bytes, ref offset);
        Debuger.Log(LogType.Log, DebugType.Log, " >>>> Load Config Success!!! ");
    }
}
