using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace NeosDesktopToolShortcutRemapper
{
    public class NeosDesktopToolShortcutRemapper : NeosMod
    {
        public override string Name => "NeosDesktopToolShortcutRemapper";
        public override string Author => "runtime";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/zkxs/NeosDesktopToolShortcutRemapper";

        private static Dictionary<Uri, Uri> toolRemapping = new Dictionary<Uri, Uri>();

        static NeosDesktopToolShortcutRemapper() {
            // rebind 2 to DevTip with BlueInspector V1.5.7
            toolRemapping.Add(
                new Uri("neosrec:///G-Neos/Inventory/SpawnObjects/ShortcutTooltips/DevToolTip"),
                new Uri("neosrec:///U-yoshi1123-/R-e4560205-4a81-4704-8b9c-4d6342fc1637")
            );
            // rebind 3 to Desktop LogixTip + LogixNodeMenuInjector V1.0
            toolRemapping.Add(
                new Uri("neosrec:///G-Neos/Inventory/SpawnObjects/ShortcutTooltips/LogixTip"),
                new Uri("neosrec:///U-yoshi1123-/R-80adbf5d-c6bc-496a-9c44-e849c1126f3b")
            );
            // rebind 5 to component clone
            toolRemapping.Add(
                new Uri("neosrec:///G-Neos/Inventory/SpawnObjects/ShortcutTooltips/ShapeTip"), 
                new Uri("neosrec:///G-Neos/Inventory/Essential Tools/ComponentCloneTip")
            );
        }

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.michaelripley.NeosDesktopToolShortcutRemapper");
            MethodInfo original = AccessTools.DeclaredMethod(typeof(CommonTool), nameof(CommonTool.SpawnAndEquip));
            MethodInfo prefix = AccessTools.DeclaredMethod(typeof(NeosDesktopToolShortcutRemapper), nameof(NeosDesktopToolShortcutRemapper.SpawnAndEquipPrefix));
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            Msg("Patched successfully");
        }

        private static void SpawnAndEquipPrefix(ref Uri uri)
        {
            if (toolRemapping.TryGetValue(uri, out Uri remappedUri))
            {
                Debug($"converted \"{uri}\" into \"{remappedUri}\"");
                uri = remappedUri;
            }
        }
    }
}
