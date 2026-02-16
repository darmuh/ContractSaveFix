using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using YAPYAP;

namespace ContractSaveFix
{
    [BepInAutoPlugin]
    internal partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"Plugin {Name} is loaded, yay!");
        }

        [HarmonyPatch(typeof(DungeonTasks), nameof(DungeonTasks.OnStartServer))]
        internal class DungeonTasksOnStartServerHook
        {
            public static void Prefix(DungeonTasks __instance)
            {
                Log.LogDebug($"{__instance} OnStartServer");

                Log.LogInfo("Creating delayed instance information");
                if (DungeonTasks.Instance == null)
                {
                    DungeonTasks.Instance = __instance;
                    __instance.allTasks = [.. __instance.constantTasks, .. __instance.randomTasks, .. __instance.collectableTasks];
                    for (int i = 0; i < __instance.allTasks.Length; i++)
                    {
                        GameplayTaskSO gameplayTaskSO = __instance.allTasks[i];
                        if (gameplayTaskSO != null)
                        {
                            gameplayTaskSO.TaskId = i;
                        }
                    }
                }
                else
                {
                    UnityEngine.Object.Destroy(__instance.gameObject);
                }

                Log.LogMessage("Creating dungeon tasks at session start");
                __instance.CreateTasks(DungeonManager.Instance.Generator);
            }
        }

        [HarmonyPatch(typeof(DungeonTasks), nameof(DungeonTasks.Awake))]
        internal class DungeonTasksAwakeDelete
        {
            public static bool Prefix(DungeonTasks __instance)
            {
                Log.LogInfo($"{__instance}, skipping Awake");
                return false;
            }
        }
    }
}
