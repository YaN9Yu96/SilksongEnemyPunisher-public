using UnityEngine;
using System;
using HarmonyLib;

namespace SilksongEnemyPunisher
{
    [HarmonyPatch]
    internal class ReactionPatch
    {
        public static float ReactionSpeed => Main.ReactionSpeed.Value;
        public static float ReactionMinTime => Main.ReactionMinTime.Value;
        public static bool AffectsBoss => Main.ReactionAffectsBoss.Value;

        [HarmonyPrefix]
        [HarmonyPatch(
            typeof(tk2dSpriteAnimator),
            nameof(tk2dSpriteAnimator.Play),
            new Type[] { typeof(tk2dSpriteAnimationClip), typeof(float), typeof(float) })]
        public static void Animator_PlayWithFps_Prefix(
            ref tk2dSpriteAnimationClip clip,
            ref float clipStartTime,
            ref float overrideFps,
            tk2dSpriteAnimator __instance)
        {
            if (clip == null) 
                return;
            var go = __instance.gameObject;
            if (go == null || !go.IsEnemy())
                return;
            if (!AffectsBoss && go.IsBoss())
                return;

            string clipName = clip.name.ToLower();
            if (clipName.Contains("antic"))
            {
                float baseFps = (overrideFps > 0f) ? overrideFps : clip.fps;
                float scaledFps = baseFps * ReactionSpeed;

                int totalFrames = clip.frames.Length;
                float totalTime = totalFrames / scaledFps;
                float remainingTime = Mathf.Max(0f, totalTime - clipStartTime);

                // 判断是否低于反应阈值
                if (remainingTime < ReactionMinTime)
                {
                    // 把fps调整到最小
                    scaledFps = totalFrames / (clipStartTime + ReactionMinTime);
                    //Main.Log.LogInfo($"[{__instance.name}] {clip.name} 阈值触发: 剩余 {remainingTime:F0}ms, fps 调整为 {scaledFps:F2}");
                }

                overrideFps = scaledFps;

                //Main.Log.LogInfo($"[{__instance.name}] {clip.name} fps {baseFps:F2} -> {overrideFps:F2} ({remainingTime:F0}ms)");
            }
        }
    }
}