using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace SilksongEnemyPunisher
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<float> ReactionSpeed;
        public static ConfigEntry<float> ReactionMinTime;

        public static ConfigEntry<float> VelocityPunishThreshold;
        public static ConfigEntry<float> VelocityPunishWindow;
        public static ConfigEntry<float> VelocityMaxPenalty;
        public static ConfigEntry<float> VelocityPerHitPenalty;

        public static Main Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log => Instance.Logger;

        private static Harmony _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        private void Awake()
        {
            LoadConfig();
            Instance = this;
            _harmony.PatchAll();
            Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} 已启动");
        }

        private void OnDestroy()
        {
            _harmony.UnpatchSelf();
        }

        private void LoadConfig()
        {
            ReactionSpeed = Config.Bind(
                "Reaction",
                "ReactionSpeed",
                0.9f,
                "敌人准备动画播放速度\nEnemy prepare animation speed"
            );

            ReactionMinTime = Config.Bind(
                "Reaction",
                "ReactionMinTime",
                0.3f,
                "敌人准备动画最小时间（秒）\nMinimum time for enemy prepare animation (seconds)"
            );

            VelocityPunishThreshold = Config.Bind(
                "Velocity",
                "VelocityPunishThreshold",
                10f,
                "位移速度超过该值时，会触发惩罚\nVelocity above this threshold triggers penalty"
            );

            VelocityPunishWindow = Config.Bind(
                "Velocity",
                "VelocityPunishWindow",
                2f,
                "位移惩罚统计时间窗口（秒）\nTime window for velocity penalty calculation (seconds)"
            );

            VelocityMaxPenalty = Config.Bind(
                "Velocity",
                "VelocityMaxPenalty",
                0.6f,
                "最大速度减少比例，0：不减速，1：完全不动\nMaximum speed reduction ratio, 0: no reduction, 1: full stop"
            );

            VelocityPerHitPenalty = Config.Bind(
                "Velocity",
                "VelocityPerHitPenalty",
                0.2f,
                "每次惩罚扣除速度比例\nSpeed reduction per hit"
            );
        }
    }
}
