using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay_API.Database
{// start of namespace
    public class BlobService
    {// start of class

        private readonly BlobContainerClient container;
        public BlobService(IConfiguration config)
        {
            string? connection = config["AzureStorage:ConnectionString"];
            string name = config["AzureStorage:ContainerName"] ?? "raceday-images";

            // without a connection string the app still runs, only image uploads are switched off
            if (string.IsNullOrWhiteSpace(connection))
            {
                container = null;
                return;
            }

            container = new BlobContainerClient(connection, name);
            // images must be viewable in the browser, so the blobs are public
            container.CreateIfNotExists(PublicAccessType.Blob);
        }

        // returns a problem message, or null when the image is fine
        public static string? check_image(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return "please choose an image";
            }
            if (file.Length > 5 * 1024 * 1024)
            {
                return "the image must be smaller than 5 MB";
            }
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".webp")
            {
                return "only jpg, png or webp images are allowed";
            }
            return null;
        }

        // saves the image in Azure and returns its public address
        // saves the image in Azure and returns its public address
        public async Task<string> upload(IFormFile file, string folder)
        {
            if (container == null)
            {
                return "image storage is not available right now";
            }

            string ext = Path.GetExtension(file.FileName).ToLower();
            BlobClient blob = container.GetBlobClient(folder + "/" + Guid.NewGuid() + ext);

            using Stream stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blob.Uri.ToString();
        }

    }// end of class
}// end of namespace