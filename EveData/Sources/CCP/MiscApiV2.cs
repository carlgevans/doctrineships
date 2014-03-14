namespace EveData.Sources.CCP
{
    using EveData.Entities;
    using Tools;

    /// <summary>
    /// Version 2 Api methods from other CCP locations.
    /// </summary>
    internal sealed class MiscApiV2
    {
        private readonly ISystemLogger logger;

        internal MiscApiV2(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// <para>Retrieve an image from the Eve Api for a given entity.</para>
        /// <para>Possible entities and their available image sizes are:</para>
        /// <para>-Character(30,32,64,128,200,256,512,1024)</para>
        /// <para>-Corporation (30,32,64,128,256)</para>
        /// <para>-Alliance (30,32,64,128)</para>
        /// <para>-InventoryType (32,64)</para>
        /// <para>-Render (32,64,128,256,512)</para>
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="imageType">An enum value of the entity type (character, corporation, etc..).</param>
        /// <param name="size">An optional size parameter, defaulting to 64.</param>
        /// <returns>Returns a url to the image.</returns>
        internal string MiscImage(int id, EveDataImageType imageType, int size = 64)
        {
            string imageUrl = string.Empty;

            switch (imageType)
            {
                case EveDataImageType.Character:
                    imageUrl = "http://image.eveonline.com/" + EveDataImageType.Character + "/" + id + "_" + size + ".jpg";
                    break;
                case EveDataImageType.Corporation:
                    imageUrl = "http://image.eveonline.com/" + EveDataImageType.Corporation + "/" + id + "_" + size + ".png";
                    break;
                case EveDataImageType.Alliance:
                    imageUrl = "http://image.eveonline.com/" + EveDataImageType.Alliance + "/" + id + "_" + size + ".png";
                    break;
                case EveDataImageType.InventoryType:
                    imageUrl = "http://image.eveonline.com/" + EveDataImageType.InventoryType + "/" + id + "_" + size + ".png";
                    break;
                case EveDataImageType.Render:
                    imageUrl = "http://image.eveonline.com/" + EveDataImageType.Render + "/" + id + "_" + size + ".png";
                    break;
                default:
                    imageUrl = "http://image.eveonline.com/Alliance/1_128.png";
                    break;
            }

            return imageUrl;
        }
    }
}
