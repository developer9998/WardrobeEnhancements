using GorillaNetworking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Zenject;

namespace WardrobeEnhancements.Behaviours
{
    public class PriceHelper : IInitializable
    {
        public Dictionary<string, int> Prices = new Dictionary<string, int>();
        private Dictionary<CosmeticsController.CosmeticItem, int> SetIndex = new Dictionary<CosmeticsController.CosmeticItem, int>();

        public void Initialize()
        {
            using WebClient webClient = new();
            string Contents = webClient.DownloadString(Constants.PriceLink);

            Prices = JsonConvert.DeserializeObject<Dictionary<string, int>>(Contents);
            Prices = Prices.ToDictionary(k => k.Key.ToUpper(), k => k.Value);
        }

        public int GetPrice(CosmeticsController.CosmeticItem item)
        {
            string itemName = string.IsNullOrEmpty(item.overrideDisplayName) ? item.displayName : item.overrideDisplayName;
            if (Prices.ContainsKey(itemName)) return Prices[itemName];

            var allCosmetics = Object.FindObjectOfType<CosmeticsController>().allCosmetics;
            int setIndex = SetIndex.ContainsKey(item) ? SetIndex[item] : allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => x.itemCategory == CosmeticsController.CosmeticCategory.Set && x.bundledItems.Contains(item.itemName));
            if (setIndex != -1)
            {
                var newSet = allCosmetics[setIndex];
                SetIndex.AddOrUpdate(item, setIndex);
                return GetPrice(newSet);
            }
            return item.cost;
        }
    }
}
