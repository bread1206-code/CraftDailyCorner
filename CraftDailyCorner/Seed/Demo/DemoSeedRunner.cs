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
        private readonly DemoSeedAutoReplyTemplates _demoSeedAutoReplyTemplates;
        private readonly DemoSeedProducts _demoSeedProducts;
        private readonly DemoSeedProductImages _demoSeedProductImages;
        private readonly DemoSeedProductRelations _demoSeedProductRelations;
        private readonly DemoSeedInventories _demoSeedInventories;
        private readonly DemoSeedCarts _demoSeedCarts;
        private readonly DemoSeedNotificationPreferences _demoSeedNotificationPreferences;

        private readonly DemoSeedCreatorPosts _demoSeedCreatorPosts;
        private readonly DemoSeedPostComments _demoSeedPostComments;
        private readonly DemoSeedFollowCreators _demoSeedFollowCreators;
        private readonly DemoSeedReactions _demoSeedReactions;

        private readonly DemoSeedOrders _demoSeedOrders;
        private readonly DemoSeedOrderDetails _demoSeedOrderDetails;
        private readonly DemoSeedPayments _demoSeedPayments;
        private readonly DemoSeedShipments _demoSeedShipments;

        private readonly MemberSeedLoader _memberSeedLoader;
        private readonly CreatorSeedLoader _creatorSeedLoader;
        private readonly ProductSeedLoader _productSeedLoader;
        private readonly ProductImageSeedLoader _productImageSeedLoader;
        private readonly CreatorPostSeedLoader _creatorPostSeedLoader;
        private readonly OrderSeedLoader _orderSeedLoader;

        public DemoSeedRunner(
            DemoSeedMembers demoSeedMembers,
            DemoSeedPrivacies demoSeedPrivacies,
            DemoSeedMemberRoles demoSeedMemberRoles,
            DemoSeedMemberRoleHistories demoSeedMemberRoleHistories,
            DemoSeedCreatorApplications demoSeedCreatorApplications,
            DemoSeedCreatorProfiles demoSeedCreatorProfiles,
            DemoSeedAutoReplyTemplates demoSeedAutoReplyTemplates,
            DemoSeedProducts demoSeedProducts,
            DemoSeedProductImages demoSeedProductImages,
            DemoSeedProductRelations demoSeedProductRelations,
            DemoSeedInventories demoSeedInventories,
            DemoSeedCarts demoSeedCarts,
            DemoSeedNotificationPreferences demoSeedNotificationPreferences,
            DemoSeedCreatorPosts demoSeedCreatorPosts,
            DemoSeedPostComments demoSeedPostComments,
            DemoSeedFollowCreators demoSeedFollowCreators,
            DemoSeedReactions demoSeedReactions,
            DemoSeedOrders demoSeedOrders,
            DemoSeedOrderDetails demoSeedOrderDetails,
            DemoSeedPayments demoSeedPayments,
            DemoSeedShipments demoSeedShipments,
            MemberSeedLoader memberSeedLoader,
            CreatorSeedLoader creatorSeedLoader,
            ProductSeedLoader productSeedLoader,
            ProductImageSeedLoader productImageSeedLoader,
            CreatorPostSeedLoader creatorPostSeedLoader,
            OrderSeedLoader orderSeedLoader)
        {
            _demoSeedMembers = demoSeedMembers;
            _demoSeedPrivacies = demoSeedPrivacies;
            _demoSeedMemberRoles = demoSeedMemberRoles;
            _demoSeedMemberRoleHistories = demoSeedMemberRoleHistories;
            _demoSeedCreatorApplications = demoSeedCreatorApplications;
            _demoSeedCreatorProfiles = demoSeedCreatorProfiles;
            _demoSeedAutoReplyTemplates = demoSeedAutoReplyTemplates;
            _demoSeedProducts = demoSeedProducts;
            _demoSeedProductImages = demoSeedProductImages;
            _demoSeedProductRelations = demoSeedProductRelations;
            _demoSeedInventories = demoSeedInventories;
            _demoSeedCarts = demoSeedCarts;
            _demoSeedNotificationPreferences = demoSeedNotificationPreferences;

            _demoSeedCreatorPosts = demoSeedCreatorPosts;
            _demoSeedPostComments = demoSeedPostComments;
            _demoSeedFollowCreators = demoSeedFollowCreators;
            _demoSeedReactions = demoSeedReactions;

            _demoSeedOrders = demoSeedOrders;
            _demoSeedOrderDetails = demoSeedOrderDetails;
            _demoSeedPayments = demoSeedPayments;
            _demoSeedShipments = demoSeedShipments;

            _memberSeedLoader = memberSeedLoader;
            _creatorSeedLoader = creatorSeedLoader;
            _productSeedLoader = productSeedLoader;
            _productImageSeedLoader = productImageSeedLoader;
            _creatorPostSeedLoader = creatorPostSeedLoader;
            _orderSeedLoader = orderSeedLoader;
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
            _demoSeedAutoReplyTemplates.Run(seedContext);
            _demoSeedProducts.Run(seedContext);
            _demoSeedProductImages.Run(seedContext);
            _demoSeedProductRelations.Run(seedContext);
            _demoSeedInventories.Run(seedContext);
            _demoSeedCarts.Run(seedContext);
            _demoSeedNotificationPreferences.Run(seedContext);

            //_demoSeedCreatorPosts.Run(seedContext);
            //_demoSeedPostComments.Run(seedContext);
            //_demoSeedFollowCreators.Run(seedContext);
            //_demoSeedReactions.Run(seedContext);

            _demoSeedOrders.Run(seedContext);
            _demoSeedOrderDetails.Run(seedContext);
            _demoSeedPayments.Run(seedContext);
            _demoSeedShipments.Run(seedContext);
        }

        private DemoSeedContext BuildContext()
        {
            var seedContext = new DemoSeedContext
            {
                Members = _memberSeedLoader.Load(DemoSeedPaths.MembersCsv),
                Creators = _creatorSeedLoader.Load(DemoSeedPaths.CreatorsCsv),
                Products = _productSeedLoader.Load(DemoSeedPaths.ProductsCsv),
                ProductImages = _productImageSeedLoader.Load(DemoSeedPaths.ProductImagesCsv),

                CreatorPosts = _creatorPostSeedLoader.LoadPosts(DemoSeedPaths.CreatorPostsCsv),
                PostComments = _creatorPostSeedLoader.LoadComments(DemoSeedPaths.PostCommentsCsv),
                Reactions = _creatorPostSeedLoader.LoadReactions(DemoSeedPaths.ReactionsCsv),
                Follows = _creatorPostSeedLoader.LoadFollows(DemoSeedPaths.FollowsCsv),

                Orders = _orderSeedLoader.LoadOrders(DemoSeedPaths.OrdersCsv),
                OrderDetails = _orderSeedLoader.LoadOrderDetails(DemoSeedPaths.OrderDetailsCsv),
                Payments = _orderSeedLoader.LoadPayments(DemoSeedPaths.PaymentsCsv),
                Shipments = _orderSeedLoader.LoadShipments(DemoSeedPaths.ShipmentsCsv)
            };

            return seedContext;
        }
    }
}