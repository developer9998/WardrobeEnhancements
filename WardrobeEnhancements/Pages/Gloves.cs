using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Gloves : WardrobePage
    {
        public override string DisplayName => "GLOVES";
        public override bool OverrideItems => false;

        public override PageType Type => PageType.Category;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.Gloves;
    }
}
