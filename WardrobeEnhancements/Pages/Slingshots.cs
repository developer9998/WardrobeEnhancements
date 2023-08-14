using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Slingshots : WardrobePage
    {
        public override string DisplayName => "SLINGSHOTS";
        public override PageType Type => PageType.Category;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.Slingshot;
    }
}
