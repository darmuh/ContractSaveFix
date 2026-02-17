using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DungeonGeneration;
using HarmonyLib;
using YAPYAP;

namespace ContractSaveFix
{
    [BepInAutoPlugin]
    internal partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }
        private static bool ServerStarted = false;

        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"Plugin {Name} is loaded, fixing your contracts!!!");
        }

        [HarmonyPatch(typeof(DungeonTasks), nameof(DungeonTasks.Awake))]
        internal class DungeonTasksAwakeHook
        {
            public static void Prefix(DungeonTasks __instance)
            {
                Log.LogDebug($"{__instance} Awake");
                ServerStarted = false;
            }
        }

        [HarmonyPatch(typeof(DungeonTasks), nameof(DungeonTasks.OnStartServer))]
        internal class DungeonTasksOnStartServerHook
        {
            public static void Prefix(DungeonTasks __instance)
            {
                Log.LogDebug($"{__instance} OnStartServer");
                ServerStarted = true;
                Log.LogMessage($"Creating tasks now!");
                __instance.CreateTasks(DungeonManager.Instance.generator);
            }
        }

        [HarmonyPatch(typeof(DungeonTasks), nameof(DungeonTasks.CreateTasks))]
        internal class DungeonTasksCreateTasksHook
        {
            public static bool Prefix(DungeonTasks __instance, DungeonGenerator generator, bool forceNewGeneration = false)
            {
                Log.LogDebug($"{__instance} CreateTasks");
                if(!ServerStarted)
                {
                    Log.LogWarning("Skipping CreateTasks that is too early!");
                    return false;
                }

                return true;
            }
        }
    }
}
