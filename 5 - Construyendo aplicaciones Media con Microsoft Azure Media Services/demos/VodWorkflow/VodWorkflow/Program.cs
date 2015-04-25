namespace VodWorkflow
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using Microsoft.WindowsAzure.MediaServices.Client;
    using Microsoft.WindowsAzure.MediaServices.Client.ContentKeyAuthorization;
    using Microsoft.WindowsAzure.MediaServices.Client.DynamicEncryption;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string accountName = ConfigurationManager.AppSettings["MediaServicesAccountName"];
                string accountKey = ConfigurationManager.AppSettings["MediaServicesAccountKey"];
                MediaServicesCredentials credentials = new MediaServicesCredentials(accountName, accountKey);
                MediaContextBase context = new CloudMediaContext(credentials);

                Console.WriteLine("Creating new asset from local file...");

                // 1. Create a new asset by uploading a mezzanine file from a local path.
                IAsset inputAsset = context.Assets.CreateFromFile(
                    "video.mp4",
                    AssetCreationOptions.None,
                    (af, p) =>
                    {
                        Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                    });

                Console.WriteLine("Asset created.");

                // 2. Prepare a job with two tasks:
                //    - One to transcode the previous mezzanine asset into a multi-bitrate asset
                //    - One to generate a closed captioning TTML file from the audio
                IJob job = context.Jobs.Create("Demo Job");

                IMediaProcessor azureMediaEncoder = context.MediaProcessors.GetLatestMediaProcessorByName("Azure Media Encoder");
                ITask encodingTask = job.Tasks.AddNew("Encoding Task", azureMediaEncoder, MediaEncoderTaskPresetStrings.H264AdaptiveBitrateMP4Set720p, TaskOptions.None);
                encodingTask.InputAssets.Add(inputAsset);
                encodingTask.OutputAssets.AddNew("Multibitrate Asset", AssetCreationOptions.None);

                IMediaProcessor azureMediaIndexer = context.MediaProcessors.GetLatestMediaProcessorByName("Azure Media Indexer");
                ITask indexingTask = job.Tasks.AddNew("Indexing Task", azureMediaIndexer, Resources.IndexerPreset, TaskOptions.None);
                indexingTask.InputAssets.Add(inputAsset);
                indexingTask.OutputAssets.AddNew("Indexer Result Asset", AssetCreationOptions.None);

                Console.WriteLine("Submitting transcoding job...");

                // 3. Submit the job and wait until it is completed.
                job.Submit();
                job = job.StartExecutionProgressTask(
                    j =>
                    {
                        Console.WriteLine("Job state: {0}", j.State);
                        Console.WriteLine("Job progress: {0:0.##}%", j.GetOverallProgress());
                    },
                    CancellationToken.None).Result;

                Console.WriteLine("Transcoding job finished.");

                IAsset multibitrateOutputAsset = job.OutputMediaAssets[0];
                IAsset indexerResultOutputAsset = job.OutputMediaAssets[1];

                Console.WriteLine("Creating key authorization policy and delivery policy...");

                // 4. [Dynamic Encryption] Configure Key Authorization Policy and Asset Delivery Policy
                IContentKey key = CreateEnvelopeTypeContentKey(context, multibitrateOutputAsset);
                AddOpenRestrictedAuthorizationPolicy(context, key);
                CreateAssetDeliveryPolicy(context, multibitrateOutputAsset, key);

                Console.WriteLine("Publishing output asset...");

                // 5. Publish the output asset by creating an Origin locator for adaptive streaming, and a SAS locator for progressive download.
                context.Locators.Create(LocatorType.OnDemandOrigin, multibitrateOutputAsset, AccessPermissions.Read, TimeSpan.FromDays(30));
                context.Locators.Create(LocatorType.Sas, multibitrateOutputAsset, AccessPermissions.Read, TimeSpan.FromDays(30));

                IEnumerable<IAssetFile> mp4AssetFiles = multibitrateOutputAsset
                        .AssetFiles
                        .ToList()
                        .Where(af => af.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase));

                // 6. [Dynamic Packaging] Generate the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming
                Uri smoothStreamingUri = multibitrateOutputAsset.GetSmoothStreamingUri();
                Uri hlsv4Uri = multibitrateOutputAsset.GetHlsUri();
                Uri hlsv3Uri = multibitrateOutputAsset.GetHlsv3Uri();
                Uri mpegDashUri = multibitrateOutputAsset.GetMpegDashUri();

                // 7. Generate SAS URL for progressive download.
                List<Uri> mp4ProgressiveDownloadUris = mp4AssetFiles.Select(af => af.GetSasUri()).ToList();

                // 8. Get the asset URLs.
                Console.WriteLine(smoothStreamingUri);
                Console.WriteLine(hlsv4Uri);
                Console.WriteLine(hlsv3Uri);
                Console.WriteLine(mpegDashUri);
                mp4ProgressiveDownloadUris.ForEach(uri => Console.WriteLine(uri));

                Console.WriteLine("Output asset available for adaptive streaming and progressive download.");

                string outputFolder = "indexer-job-output";
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                Console.WriteLine("Downloading output asset files to local folder...");

                // 9. Download the indexer result output asset to a local folder.
                indexerResultOutputAsset.DownloadToFolder(
                    outputFolder,
                    (af, p) =>
                    {
                        Console.WriteLine("Downloading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                    });

                Console.WriteLine("Indexer output files available at '{0}'.", Path.GetFullPath(outputFolder));

                Console.WriteLine("VOD workflow finished.");
            }
            catch (Exception exception)
            {
                // Parse the XML error message in the Media Services response and create a new 
                // exception with its content.
                exception = MediaServicesExceptionParser.Parse(exception);

                Console.Error.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static IContentKey CreateEnvelopeTypeContentKey(MediaContextBase context, IAsset asset)
        {
            // Create envelope encryption content key
            Guid keyId = Guid.NewGuid();
            byte[] contentKey = GetRandomBuffer(16);

            IContentKey key = context.ContentKeys.Create(keyId, contentKey, "AES Content Key", ContentKeyType.EnvelopeEncryption);

            // Associate the key with the asset.
            asset.ContentKeys.Add(key);

            return key;
        }

        private static byte[] GetRandomBuffer(int size)
        {
            byte[] randomBytes = new byte[size];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            return randomBytes;
        }

        private static void AddOpenRestrictedAuthorizationPolicy(MediaContextBase context, IContentKey contentKey)
        {
            // Create ContentKeyAuthorizationPolicy with Open restrictions and create authorization policy             
            IContentKeyAuthorizationPolicy policy = context.ContentKeyAuthorizationPolicies.CreateAsync("Open Authorization Policy").Result;
            List<ContentKeyAuthorizationPolicyRestriction> restrictions = new List<ContentKeyAuthorizationPolicyRestriction>
                {
                    new ContentKeyAuthorizationPolicyRestriction
                    {
                        Name = "Open Authorization Policy",
                        KeyRestrictionType = (int)ContentKeyRestrictionType.Open,
                        Requirements = null // no requirements needed
                    }
                };
            IContentKeyAuthorizationPolicyOption policyOption = context.ContentKeyAuthorizationPolicyOptions.Create("Option", ContentKeyDeliveryType.BaselineHttp, restrictions, string.Empty);

            policy.Options.Add(policyOption);

            // Add ContentKeyAutorizationPolicy to ContentKey
            contentKey.AuthorizationPolicyId = policy.Id;
            IContentKey updatedKey = contentKey.UpdateAsync().Result;
        }

        private static void CreateAssetDeliveryPolicy(MediaContextBase context, IAsset asset, IContentKey key)
        {
            Uri keyAcquisitionUri = key.GetKeyDeliveryUrl(ContentKeyDeliveryType.BaselineHttp);

            string envelopeEncryptionIV = Convert.ToBase64String(GetRandomBuffer(16));

            // The following policy configuration specifies: 
            //   key url that will have KID=<Guid> appended to the envelope and
            //   the Initialization Vector (IV) to use for the envelope encryption.
            Dictionary<AssetDeliveryPolicyConfigurationKey, string> assetDeliveryPolicyConfiguration = new Dictionary<AssetDeliveryPolicyConfigurationKey, string> 
            {
                { AssetDeliveryPolicyConfigurationKey.EnvelopeKeyAcquisitionUrl, keyAcquisitionUri.ToString() },
                { AssetDeliveryPolicyConfigurationKey.EnvelopeEncryptionIVAsBase64, envelopeEncryptionIV }
            };

            IAssetDeliveryPolicy assetDeliveryPolicy = context.AssetDeliveryPolicies.Create(
                "AssetDeliveryPolicy",
                AssetDeliveryPolicyType.DynamicEnvelopeEncryption,
                AssetDeliveryProtocol.SmoothStreaming | AssetDeliveryProtocol.HLS | AssetDeliveryProtocol.Dash,
                assetDeliveryPolicyConfiguration);

            // Add AssetDelivery Policy to the asset
            asset.DeliveryPolicies.Add(assetDeliveryPolicy);
        }
    }
}
