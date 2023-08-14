using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class ShoppingCart : WardrobePage
    {
        public override string DisplayName => "SHOPPING CART";
        public override PageType Type => PageType.Outfit;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.None;
    }
}
