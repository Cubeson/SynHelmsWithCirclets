using System;
using System.Threading.Tasks;

using Mutagen.Bethesda;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

using Noggog;

namespace SynHelmsWithCirlcets
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SynHelmsAndCirclet.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            state.LoadOrder.PriorityOrder.Armor().WinningOverrides().ForEach(armor =>
            {
                if (!string.IsNullOrEmpty(armor.Name?.String ?? "") && armor.BodyTemplate != null && armor.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Circlet) && !armor.HasKeyword(Skyrim.Keyword.ArmorJewelry) && armor.BodyTemplate.FirstPersonFlags.Length() != 1)
                {
                    var na = state.PatchMod.Armors.GetOrAddAsOverride(armor);
                    Console.WriteLine($"Patching {na.Name?.String}");
                    if (na.BodyTemplate != null)
                    {
                        na.BodyTemplate.FirstPersonFlags &= ~BipedObjectFlag.Circlet;
                    }
                }
            });
        }
    }
}
