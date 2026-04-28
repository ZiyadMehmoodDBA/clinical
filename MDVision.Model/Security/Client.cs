using MDVision.Model.Common;

namespace MDVision.Model.Security
{
    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public Constants.ApplicationType appType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }


        public static Constants.ApplicationType getApplicationTypeById(int id)
        {

            switch (id)
            {
                case 0:
                    return Constants.ApplicationType.WEB;
                case 1:
                    return Constants.ApplicationType.NATIVE;
                default:
                    return Constants.ApplicationType.WEB;

            }


        }

    }
}