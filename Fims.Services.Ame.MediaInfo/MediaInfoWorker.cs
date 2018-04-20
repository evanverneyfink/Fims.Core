using System;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Fims.Server.Data;
using Fims.Server.Files;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Services.Ame.MediaInfo
{
    public class MediaInfoWorker : Worker<AmeJob>
    {
        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="MediaInfoWorker"/>
        /// </summary>
        /// <param name="dataHandler"></param>
        /// <param name="accessibleUrlProvider"></param>
        /// <param name="processRunner"></param>
        /// <param name="mediaInfoOutputConverter"></param>
        /// <param name="fileStorage"></param>
        /// <param name="resourceSerializer"></param>
        /// <param name="processLocator"></param>
        public MediaInfoWorker(IResourceDataHandler dataHandler,
                               IMediaInfoAccessibleUrlProvider accessibleUrlProvider,
                               IProcessRunner processRunner,
                               IMediaInfoOutputConverter mediaInfoOutputConverter,
                               IFileStorage fileStorage,
                               IResourceSerializer resourceSerializer,
                               IMediaInfoProcessLocator processLocator)
            : base(dataHandler)
        {
            AccessibleUrlProvider = accessibleUrlProvider;
            ProcessRunner = processRunner;
            MediaInfoOutputConverter = mediaInfoOutputConverter;
            FileStorage = fileStorage;
            ResourceSerializer = resourceSerializer;
            ProcessLocator = processLocator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the accessible url provider
        /// </summary>
        private IMediaInfoAccessibleUrlProvider AccessibleUrlProvider { get; }

        /// <summary>
        /// Gets the process runner
        /// </summary>
        private IProcessRunner ProcessRunner { get; }

        /// <summary>
        /// Gets the MediaInfo output converter
        /// </summary>
        private IMediaInfoOutputConverter MediaInfoOutputConverter { get; }

        /// <summary>
        /// Gets file storage
        /// </summary>
        private IFileStorage FileStorage { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        private IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Gets the process locator
        /// </summary>
        private IMediaInfoProcessLocator ProcessLocator { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Runs media info and stores the output to a file
        /// </summary>
        /// <returns></returns>
        public override async Task Execute(AmeJob job)
        {
            // ensure this is a valid profile for this worker
            if (job.JobProfile.Label != "ExtractTechnicalMetadata")
                throw new Exception($"JobProfile '{job.JobProfile.Label}' not accepted");

            // ensure the job specifies input and output locations
            if (job.JobInput["inputFile"] == null)
                throw new Exception("Job does not specify an input location.");
            // ensure the job specifies an output location
            if (job.JobInput["outputLocation"] == null)
                throw new Exception("Job does not specify an output location.");

            var serializedOutputLocation = job.JobInput["outputLocation"]?.ToString();
            if (serializedOutputLocation == null)
                throw new Exception("Failed to resolve jobInput[\"outputLocation\"]");

            // get output locator
            var outputLocation = await ResourceSerializer.Deserialize<Locator>(serializedOutputLocation);

            var serializedInputFile = job.JobInput["inputFile"]?.ToString();
            if (serializedInputFile == null)
                throw new Exception("Failed to resolve jobInput[\"inputFile\"]");

            // get input locator
            var inputFile = await ResourceSerializer.Deserialize<Locator>(serializedInputFile);

            // get the url of the file MediaInfo should use (could be local, S3, etc)
            var accessibleUrl = await AccessibleUrlProvider.GetMediaInfoAccessibleUrl(inputFile);
            if (accessibleUrl == null)
                throw new Exception("Input file is not accessible to MediaInfo.");

            // run the media info process
            var result = await ProcessRunner.RunProcess(ProcessLocator.GetMediaInfoLocation(), "--Output=EBUCore", accessibleUrl);
            if (!string.IsNullOrWhiteSpace(result.StdErr))
                throw new Exception($"MediaInfo returned one or more errors: {result.StdErr}.");

            // convert output to JSON
            var mediaInfoJson = MediaInfoOutputConverter.GetJson(result.StdOut);

            // save JSON to file and store the resulting locator in the job output
            job.JobOutput["outputFile"] = await FileStorage.SaveFile(outputLocation, Guid.NewGuid().ToString(), mediaInfoJson);
        }

        #endregion
    }
}
