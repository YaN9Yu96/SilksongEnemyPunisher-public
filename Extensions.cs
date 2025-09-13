using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SilksongEnemyPunisher
{
    internal static class Extensions
    {
        public static Vector2 GetVelocity(this SetVelocity2d __instance)
        {
            Vector2 baseVel = __instance.vector.IsNone ? Vector2.zero : __instance.vector.Value;
            float x = __instance.x.IsNone ? baseVel.x : __instance.x.Value;
            float y = __instance.y.IsNone ? baseVel.y : __instance.y.Value;
            return new Vector2(x, y);
        }

        public static Vector2 GetVelocity(this SetVelocityByScale __instance)
        {
            GameObject go = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);

            float x = __instance.speed.IsNone ? 0f : __instance.speed.Value;
            float y = __instance.ySpeed.IsNone ? 0f : __instance.ySpeed.Value;
            if (go.transform.localScale.x <= 0f)
                x *= -1f;

            return new Vector2(x, y);
        }

        public static bool IsEnemy(this GameObject go)
        {
            if (LayerMask.LayerToName(go.layer).ToLower() != "enemies")
                return false;
            return true;
        }


        #region 缓存
        private static int _accessCount = 0;
        private const int _cleanupThreshold = 100;

        private static Dictionary<GameObject, bool> _bossCache = new Dictionary<GameObject, bool>();
        private static Dictionary<GameObject, Rigidbody2D> _rbCache = new Dictionary<GameObject, Rigidbody2D>();

        public static bool IsBoss(this GameObject go)
        {
            if (go == null) return false;

            if (_bossCache.TryGetValue(go, out bool isBoss))
                return isBoss;

            IncrementAndCleanup();

            var healthMgr = go.GetComponent<HealthManager>();
            isBoss = healthMgr != null && healthMgr.IsBoss();
            _bossCache[go] = isBoss;
            return isBoss;
        }

        private static bool IsBoss(this HealthManager mgr)
        {
            if (mgr == null) return false;

            var field = mgr.GetType().GetField("initHp", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(mgr) is int initHp)
                return initHp > 150;    // 没找到啥办法，只能用hp来判断boss了

            return false;
        }
        
        public static Rigidbody2D GetRigidbody2D(this GameObject go)
        {
            if (go == null) return null;

            if (_rbCache.TryGetValue(go, out Rigidbody2D rb))
                return rb;

            IncrementAndCleanup();

            rb = go.GetComponent<Rigidbody2D>();
            _rbCache[go] = rb;
            return rb;
        }

        private static void IncrementAndCleanup()
        {
            _accessCount++;
            if (_accessCount < _cleanupThreshold)
                return;

            _accessCount = 0;
            CleanupDictionary(_rbCache);
            CleanupDictionary(_bossCache);
        }

        private static void CleanupDictionary<T>(Dictionary<GameObject, T> dict)
        {
            var keysToRemove = new List<GameObject>();
            foreach (var kvp in dict)
            {
                if (kvp.Key == null)
                    keysToRemove.Add(kvp.Key);
            }
            foreach (var key in keysToRemove)
                dict.Remove(key);
        }

        #endregion
    }
}
