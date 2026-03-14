using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;
using CraftDailyCorner.Seed.Demo.Loaders;
using CraftDailyCorner.Seed.Demo.Seeders;

namespace CraftDailyCorner.Seed.Demo
{
    public class DemoSeedRunner
    {
        private readonly DemoSeedMembers _demoSeedMembers;
        private readonly DemoSeedPrivacies _demoSeedPrivacies;
        private readonly DemoSeedMemberRoles _demoSeedMemberRoles;
        private readonly DemoSeedMemberRoleHistories _demoSeedMemberRoleHistories;
        private readonly DemoSeedCreatorApplications _demoSeedCreatorApplications;
        private readonly DemoSeedCreatorProfiles _demoSeedCreatorProfiles;
        private readonly DemoSeedProducts _demoSeedProducts;
        private readonly DemoSeedProductImages _demoSeedProductImages;
        private readonly DemoSeedProductRelations _demoSeedProductRelations;
        private readonly DemoSeedInventories _demoSeedInventories;
        private readonly DemoSeedCarts _demoSeedCarts;
        private readonly DemoSeedNotificationPreferences _demoSeedNotificationPreferences;

        private readonly MemberSeedLoader _memberSeedLoader;
        private readonly CreatorSeedLoader _creatorSeedLoader;
        private readonly ProductSeedLoader _productSeedLoader;
        private readonly ProductImageSeedLoader _productImageSeedLoader;

        public DemoSeedRunner(
            DemoSeedMembers demoSeedMembers,
            DemoSeedPrivacies demoSeedPrivacies,
            DemoSeedMemberRoles demoSeedMemberRoles,
            DemoSeedMemberRoleHistories demoSeedMemberRoleHistories,
            DemoSeedCreatorApplications demoSeedCreatorApplications,
            DemoSeedCreatorProfiles demoSeedCreatorProfiles,
            DemoSeedProducts demoSeedProducts,
            DemoSeedProductImages demoSeedProductImages,
            DemoSeedProductRelations demoSeedProductRelations,
            DemoSeedInventories demoSeedInventories,
            DemoSeedCarts demoSeedCarts,
            DemoSeedNotificationPreferences demoSeedNotificationPreferences,
            MemberSeedLoader memberSeedLoader,
            CreatorSeedLoader creatorSeedLoader,
            ProductSeedLoader productSeedLoader,
            ProductImageSeedLoader productImageSeedLoader)
        {
            _demoSeedMembers = demoSeedMembers;
            _demoSeedPrivacies = demoSeedPrivacies;
            _demoSeedMemberRoles = demoSeedMemberRoles;
            _demoSeedMemberRoleHistories = demoSeedMemberRoleHistories;
            _demoSeedCreatorApplications = demoSeedCreatorApplications;
            _demoSeedCreatorProfiles = demoSeedCreatorProfiles;
            _demoSeedProducts = demoSeedProducts;
            _demoSeedProductImages = demoSeedProductImages;
            _demoSeedProductRelations = demoSeedProductRelations;
            _demoSeedInventories = demoSeedInventories;
            _demoSeedCarts = demoSeedCarts;
            _demoSeedNotificationPreferences = demoSeedNotificationPreferences;

            _memberSeedLoader = memberSeedLoader;
            _creatorSeedLoader = creatorSeedLoader;
            _productSeedLoader = productSeedLoader;
            _productImageSeedLoader = productImageSeedLoader;
        }

        public void Run()
        {
            var seedContext = BuildContext();

            _demoSeedMembers.Run(seedContext);
            _demoSeedPrivacies.Run(seedContext);
            _demoSeedMemberRoles.Run(seedContext);
            _demoSeedMemberRoleHistories.Run(seedContext);
            _demoSeedCreatorApplications.Run(seedContext);
            _demoSeedCreatorProfiles.Run(seedContext);
            _demoSeedProducts.Run(seedContext);
            _demoSeedProductImages.Run(seedContext);
            _demoSeedProductRelations.Run(seedContext);
            _demoSeedInventories.Run(seedContext);
            _demoSeedCarts.Run(seedContext);
            _demoSeedNotificationPreferences.Run(seedContext);
        }

        private DemoSeedContext BuildContext()
        {
            var seedContext = new DemoSeedContext
            {
                Members = _memberSeedLoader.Load(DemoSeedPaths.MembersCsv),
                Creators = _creatorSeedLoader.Load(DemoSeedPaths.CreatorsCsv),
                Products = _productSeedLoader.Load(DemoSeedPaths.ProductsCsv),
                ProductImages = _productImageSeedLoader.Load(DemoSeedPaths.ProductImagesCsv)
            };

            return seedContext;
        }
    }
}