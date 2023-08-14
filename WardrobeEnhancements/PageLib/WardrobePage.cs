using GorillaNetworking;

namespace WardrobeEnhancements.PageLib
{
    public class WardrobePage
    {
        public virtual string DisplayName { get; }

        public virtual PageType Type { get; }
        public virtual CosmeticsController.CosmeticCategory ItemCategory { get; }
    }
}
