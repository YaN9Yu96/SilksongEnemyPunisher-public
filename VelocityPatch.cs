using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace SilksongEnemyPunisher
{
    [HarmonyPatch]
    internal class VelocityPatch
    {
        public static float VelocityPunishThreshold => Main.VelocityPunishThreshold.Value;
        public static float VelocityPunishWindow => Main.VelocityPunishWindow.Value;
        public static float VelocityMaxPenalty => Main.VelocityMaxPenalty.Value;
        public static float VelocityPerHitPenalty => Main.VelocityPerHitPenalty.Value;
        public static bool VelocityPunishAffectsBoss => Main.VelocityPunishAffectsBoss.Value;

        private static Dictionary<GameObject, List<float>> _hitTimes = new Dictionary<GameObject, List<float>>();

        public static void UpdatePenalty(GameObject go)
        {
            if (!_hitTimes.TryGetValue(go, out var times))
            {
                times = new List<float>();
                _hitTimes[go] = times;
            }
            else
            {
                // 移除过期记录
                float now = Time.time;
                times.RemoveAll(t => now - t > VelocityPunishWindow);
            }
        }

        public static void AddPenalty(GameObject go)
        {
            UpdatePenalty(go);
            // 添加触发计数
            float now = Time.time;
            _hitTimes[go].Add(now);
        }

        public static float GetPenalty(GameObject go)
        {
            UpdatePenalty(go);

            int count = _hitTimes[go].Count;
            if (count > 0)
                count -= 1;

            float penalty = count * VelocityPerHitPenalty;
            penalty = Mathf.Min(penalty, VelocityMaxPenalty);
            return 1f - penalty;
        }

        // ==================== SetVelocity2d ====================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SetVelocity2d), nameof(SetVelocity2d.OnEnter))]
        public static void SetVelocity2d_OnEnter_Prefix(SetVelocity2d __instance)
        {
            GameObject go = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
            if (go == null || !go.IsEnemy())
                return;
            if (!VelocityPunishAffectsBoss && go.IsBoss())
                return;

            var vel = __instance.GetVelocity();
            if (vel.magnitude >= VelocityPunishThreshold)
            {
                AddPenalty(go);
                //Main.Log.LogInfo($"[位移惩罚添加][{go.name}] 速度: {vel} > 阈值: {VelocityPunishThreshold} 当前记录: {_hitTimes[go].Count}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SetVelocity2d), "DoSetVelocity")]
        public static void SetVelocity2d_DoSetVelocity_Postfix(SetVelocity2d __instance)
        {
            GameObject go = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
            if (go == null || !go.IsEnemy())
                return;
            if (!VelocityPunishAffectsBoss && go.IsBoss())
                return;

            var rb = go.GetRigidbody2D();
            if (rb == null) return;

            var penalty = GetPenalty(go);
            var oriVel = __instance.GetVelocity();
            var finalVel = oriVel * penalty;

            if (Mathf.Approximately(finalVel.x, 0))
                finalVel.x = rb.linearVelocity.x;
            if (Mathf.Approximately(finalVel.y, 0))
                finalVel.y = rb.linearVelocity.y;

            rb.linearVelocity = finalVel;

            //if (penalty < 1f)
            //    Main.Log.LogInfo($"[位移惩罚中][{go.name}] ×{penalty} 速度: {oriVel} -> 新速度: {finalVel}");
        }

        // ==================== SetVelocityByScale ====================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SetVelocityByScale), nameof(SetVelocityByScale.OnEnter))]
        public static void SetVelocityByScale_OnEnter_Prefix(SetVelocityByScale __instance)
        {
            GameObject go = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
            if (go == null || !go.IsEnemy())
                return;
            if (!VelocityPunishAffectsBoss && go.IsBoss())
                return;

            var vel = __instance.GetVelocity();
            if (vel.magnitude >= VelocityPunishThreshold)
            {
                AddPenalty(go);
                //Main.Log.LogInfo($"[位移惩罚添加][{go.name}] 速度: {vel} > 阈值: {VelocityPunishThreshold} 当前记录: {_hitTimes[go].Count}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SetVelocityByScale), "DoSetVelocity")]
        public static void SetVelocityByScale_DoSetVelocity_Postfix(SetVelocityByScale __instance)
        {
            GameObject go = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
            if (go == null || !go.IsEnemy())
                return;
            if (!VelocityPunishAffectsBoss && go.IsBoss())
                return;

            var rb = go.GetRigidbody2D();
            if (rb == null) return;

            var penalty = GetPenalty(go);
            var oriVel = __instance.GetVelocity();
            var finalVel = oriVel * penalty;

            if (Mathf.Approximately(finalVel.x, 0))
                finalVel.x = rb.linearVelocity.x;
            if (Mathf.Approximately(finalVel.y, 0))
                finalVel.y = rb.linearVelocity.y;

            rb.linearVelocity = finalVel;

            //if (penalty < 1f)
            //    Main.Log.LogInfo($"[位移惩罚中][{go.name}] ×{penalty} 速度: {oriVel} -> 新速度: {finalVel}");
        }
    }
}
