#if UNITY_EDITOR
using BoatAttack;
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using Console = BoatAttack.Console;
using Object = UnityEngine.Object;

public static class Utility
{
    public static event Action<int, int> QualityLevelChange;
    private static int lastQualityLevel = -1;

    [Console.ConsoleCmd]
    public static void SetRenderer(int index)
    {
        var cam = Camera.main;
        if (cam)
        {
            var data = cam.GetUniversalAdditionalCameraData();
            data.SetRenderer(index);
        }
    }
    
    public static void CheckQualityLevel()
    {
        var curLevel = QualitySettings.GetQualityLevel();
        if (lastQualityLevel == curLevel) return;

        if(Debug.isDebugBuild)
            Debug.Log($"Quality level changed:{lastQualityLevel} to {curLevel}");
        var realIndex = GetTrueQualityLevel(curLevel);
        QualityLevelChange?.Invoke(curLevel, realIndex);
        lastQualityLevel = curLevel;
    }

    [Console.ConsoleCmd]
    public static void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    [Console.ConsoleCmd]
    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    [Console.ConsoleCmd]
    public static void WaterDebug(int mode)
    {
        WaterSystem.Ocean.SetDebugMode((WaterSystem.Ocean.DebugShading)mode);
    }

    public static int GetTrueQualityLevel()
    {
        return GetTrueQualityLevel(QualitySettings.GetQualityLevel());
    }

    public static int GetTrueQualityLevel(int level)
    {
        return QualityLevels.IndexOf(QualitySettings.names[level]);
    }

    public static string RemoveWhitespace(string input)
    {
        return Regex.Replace(input, @"\s+", "");
    }

    public static void SafeDestroyChildren(GameObject obj)
    {
        SafeDestroyChildren(obj.transform);
    }

    public static void SafeDestroyChildren(Transform obj)
    {
        foreach (Transform child in obj)
        {
            SafeDestroy(child.gameObject);
        }
    }

    public static void UnloadAssetReference(AssetReference asset)
    {
        if(asset != null && asset.IsValid())
        {
            asset.ReleaseAsset();
        }
    }

    public static void ParentAndFillRectTransform(Transform child, Transform parent)
    {
        var tableTrans = child.transform as RectTransform;
        tableTrans.SetParent(parent, false);
        tableTrans.anchorMin = Vector2.zero;
        tableTrans.anchorMax = Vector2.one;
        tableTrans.offsetMin = tableTrans.offsetMax = Vector2.zero;
    }

    public static void SafeDestroy(Object obj, bool immediate = false)
    {
        if (obj != null)
        {
#if UNITY_EDITOR
            if (immediate)
            {
                Object.DestroyImmediate(obj);
            }
            else
            {
                EditorApplication.delayCall += () => Object.DestroyImmediate(obj);
            }
            return;
#else
            Object.Destroy(obj);
            return;
#endif
        }
    }

    private static readonly List<string> QualityLevels = new List<string> {"Low", "Medium", "High"};

    public static void StaticObjects()
    {
        // remove the noise on Cinemachine cameras
        var cameras = Object.FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var cam in cameras)
        {
            var comp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if(comp)
                comp.m_AmplitudeGain = 0.0f;
        }
        // make the cinemachine carts static
        var carts = GameObject.FindObjectsOfType<CinemachineDollyCart>();
        foreach (var cart in carts)
        {
            cart.m_Speed = 0.0f;
        }
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
internal class UtilityScheduler
{
    static UtilityScheduler()
    {
        // setup the things
        if(Debug.isDebugBuild)
            Debug.Log("Setting up utilities");
        EditorApplication.update += Utility.CheckQualityLevel;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LoadAppSettings()
    {
        var appSettings = Resources.Load<GameObject>("AppSettings");
        Object.Instantiate(appSettings);
    }
}
#endif