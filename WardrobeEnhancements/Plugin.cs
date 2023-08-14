using BepInEx;
using Bepinject;
using HarmonyLib;
using System.Reflection;

namespace WardrobeEnhancements
{
    [BepInDependency("dev.auros.bepinex.bepinject"), BepInIncompatibility("org.legoandmars.gorillatag.gorillacosmetics")]
    [BepInPlugin("dev.wardrobeenhancements", "WardrobeEnhancements", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("dev.wardrobeenhancements").PatchAll(Assembly.GetExecutingAssembly());
            Zenjector.Install<MainInstaller>().OnProject().WithConfig(Config).WithLog(Logger);
        }
    }
}
