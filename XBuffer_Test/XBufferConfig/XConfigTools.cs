/**************************************************************
 *  Filename:    XConfigTools.cs
 *  Copyright:   Microsoft Co., Ltd.
 *
 *  Description: XConfigTools ClassFile.
 *
 *  @author:     xiaobai
 *  @version     2018/6/5 10:51:46  @Reviser  Initial Version
 **************************************************************/

using UnityEngine;
using System.Collections;

public static class XConfigTools
{
    public static int GetIconId(IconType iconType, int itemId)
    {
        var iconConfigs = XConfigMgr.Instance.Config.IconConfig;
        var iconConfig = iconConfigs.GetConfigById((int)iconType);
        if (iconConfig == null)
        {
            return -1;
        }

        for (int i = 0; i < iconConfig.IdList.Length; i++)
        {
            if (iconConfig.IdList[i].id == itemId)
            {
                return iconConfig.IdList[i].value;
            }
        }

        if (Global.DEBUG) Debug.LogWarning(string.Format("Failed to find icon {0}, {1} config.", iconType, itemId));
        return -1;
    }

    static xbuffer.SkillAnimationConfig _defaultConfig = new xbuffer.SkillAnimationConfig();
    public static xbuffer.SkillAnimationConfig GetSkillAnimation(int skillId)
    {
        xbuffer.SkillAnimationConfig config = XConfigMgr.Instance.Config.SkillAnimationConfig.FindConfigById(skillId);
        if (config == null)
        {
            if (Global.DEBUG) Debug.LogWarning("Failed to find SkillAnimationConfig config for: " + skillId + " |use default");
            if (_defaultConfig.Phases == null)
            {
                _defaultConfig.Phases = new xbuffer.PhaseConfig[0];
            }
            config = _defaultConfig;
        }
        return config;
    }

    //扩展方法
    public static float GetValue(this xbuffer.AttributeTypeValueEx attr, int level, float enhance = 1f)
    {
        if (level <= 0)
            return 0;
        if (attr.growthType == 0)
        {
            return (attr.value + attr.growth * (level - 1)) * enhance;
        }
        else
        {
            return (attr.value + (level * level - level) / 2 * attr.growth) * enhance;
        }
    }
}
