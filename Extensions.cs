using HutongGames.PlayMaker.Actions;
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
    }
}
