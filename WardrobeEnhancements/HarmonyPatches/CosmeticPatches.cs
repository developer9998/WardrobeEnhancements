using GorillaNetworking;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using WardrobeEnhancements.Behaviours;
using static GorillaNetworking.CosmeticsController;

namespace WardrobeEnhancements.HarmonyPatches
{
    [HarmonyPatch]
    public class CosmeticPatches
    {
        [HarmonyPatch(typeof(CosmeticsController), "UpdateShoppingCart"), HarmonyPostfix]
        public static void CC_GetUserCosmeticsAllowedPatch(CosmeticsController __instance) => Main.Instance.FilterCosmetics(__instance);

        [HarmonyPatch(typeof(CosmeticsController), "PressWardrobeFunctionButton"), HarmonyPrefix]
        public static bool CC_PressWardrobeFunctionButtonPatch(string function)
        {
            if (function == Constants.LeftFunction)
            {
                Main.Instance?.PreviousPage();
                return false;
            }

            if (function == Constants.RightFunction)
            {
                Main.Instance?.NextPage();
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(CosmeticsController), "PressWardrobeItemButton"), HarmonyPrefix]
        public static bool CC_PressWardrobeItemButtonPatch(CosmeticsController.CosmeticItem cosmeticItem, CosmeticsController __instance)
        {
            if (cosmeticItem.isNullItem) return false;
            if (Main.Instance._currentWardrobeItem != 0) return true;

            CosmeticStand myStand = __instance.cosmeticStands.FirstOrDefault(a => a.thisCosmeticItem.itemName == cosmeticItem.itemName);
            __instance.UpdateWardrobeModelsAndButtons(); __instance.PressCosmeticStandButton(myStand);

            Main.Instance?.UpdateInfo(string.IsNullOrEmpty(cosmeticItem.overrideDisplayName) ? cosmeticItem.displayName : cosmeticItem.overrideDisplayName, cosmeticItem);
            return false;
        }

        [HarmonyPatch(typeof(CosmeticsController), "UpdateWardrobeModelsAndButtons"), HarmonyPrefix]
        public static void CC_UpdateWardrobeModelsAndButtonsPrefix(CosmeticsController __instance)
        {
            bool isViewingCart = Main.Instance?._currentWardrobeItem == 0;
            foreach (var wardrobe in __instance.wardrobes)
            {
                int iterator = 0;
                while (iterator < wardrobe.wardrobeItemButtons.Length)
                {
                    wardrobe.wardrobeItemButtons[iterator].offText = isViewingCart ? "ADD TO CART" : Constants.DefaultOffText;
                    wardrobe.wardrobeItemButtons[iterator].onText = isViewingCart ? "REMOVE FROM CART" : Constants.DefaultOnText;
                    iterator++;
                }
            }
        }

        [HarmonyPatch(typeof(CosmeticsController), "UpdateWardrobeModelsAndButtons"), HarmonyPostfix]
        public static void CC_UpdateWardrobeModelsAndButtonsPostfix(CosmeticsController __instance)
        {
            var itemDictionary = __instance.allCosmeticsItemIDsfromDisplayNamesDict; // Yeah I'm not referencing that long ass name fuck that
            bool isViewingCart = Main.Instance?._currentWardrobeItem == 0;
            foreach (var wardrobe in __instance.wardrobes)
            {
                int iterator = 0;
                while (iterator < wardrobe.wardrobeItemButtons.Length)
                {
                    wardrobe.wardrobeItemButtons[iterator].isOn = isViewingCart && __instance.currentCart.Contains(wardrobe.wardrobeItemButtons[iterator].currentCosmeticItem) || wardrobe.wardrobeItemButtons[iterator].isOn;
                    wardrobe.wardrobeItemButtons[iterator].UpdateColor();
                    iterator++;
                }

                var firstItem = wardrobe.wardrobeItemButtons[0].currentCosmeticItem;
                if (firstItem.itemCategory == CosmeticCategory.Set)
                {
                    List<string> betterItemList = new();
                    foreach (var itemName in firstItem.bundledItems)
                    {
                        if (!itemDictionary.ContainsKey(itemName) && itemDictionary.ContainsValue(itemName))
                        {
                            betterItemList.Add(itemDictionary.FirstOrDefault(x => x.Value == itemName).Key);
                            continue;
                        }
                        betterItemList.Add(itemName);
                    }
                    wardrobe.wardrobeItemButtons[0].controlledModel.SetCosmeticActiveArray(betterItemList.ToArray(), Enumerable.Repeat(false, firstItem.bundledItems.Length).ToArray());
                }

                var secondItem = wardrobe.wardrobeItemButtons[1].currentCosmeticItem;
                if (secondItem.itemCategory == CosmeticCategory.Set)
                {
                    List<string> betterItemList = new();
                    foreach (var itemName in secondItem.bundledItems)
                    {
                        if (!itemDictionary.ContainsKey(itemName) && itemDictionary.ContainsValue(itemName))
                        {
                            betterItemList.Add(itemDictionary.FirstOrDefault(x => x.Value == itemName).Key);
                            continue;
                        }
                        betterItemList.Add(itemName);
                    }
                    wardrobe.wardrobeItemButtons[1].controlledModel.SetCosmeticActiveArray(betterItemList.ToArray(), Enumerable.Repeat(false, secondItem.bundledItems.Length).ToArray());
                }

                var thirdItem = wardrobe.wardrobeItemButtons[2].currentCosmeticItem;
                if (thirdItem.itemCategory == CosmeticCategory.Set)
                {
                    List<string> betterItemList = new();
                    foreach (var itemName in thirdItem.bundledItems)
                    {
                        if (!itemDictionary.ContainsKey(itemName) && itemDictionary.ContainsValue(itemName))
                        {
                            betterItemList.Add(itemDictionary.FirstOrDefault(x => x.Value == itemName).Key);
                            continue;
                        }
                        betterItemList.Add(itemName);
                    }
                    wardrobe.wardrobeItemButtons[2].controlledModel.SetCosmeticActiveArray(betterItemList.ToArray(), Enumerable.Repeat(false, thirdItem.bundledItems.Length).ToArray());
                }
            }
            Main.Instance?.UpdatePage(Main.Instance._currentWardrobeItem);
        }

        [HarmonyPatch(typeof(CosmeticsController), "ApplyCosmeticItemToSet"), HarmonyPrefix]
        public static void CC_ApplyCosmeticItemToSet(CosmeticsController.CosmeticItem newItem)
        {
            if (Main.Instance == null || newItem.isNullItem) return;
            Main.Instance?.UpdateInfo(string.IsNullOrEmpty(newItem.overrideDisplayName) ? newItem.displayName : newItem.overrideDisplayName, newItem);
        }
    }
}
