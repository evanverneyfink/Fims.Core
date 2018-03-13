using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Server.Api;
using Fims.Server.Data;
using Fims.Server.Files;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Services.Ame.MediaInfo
{
    public class MediaInfoWorker : Worker<JobAssignment>
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
        /// <param name="resourceDescriptorHelper"></param>
        public MediaInfoWorker(IResourceDataHandler dataHandler,
                               IMediaInfoAccessibleUrlProvider accessibleUrlProvider,
                               IProcessRunner processRunner,
                               IMediaInfoOutputConverter mediaInfoOutputConverter,
                               IFileStorage fileStorage,
                               IResourceDescriptorHelper resourceDescriptorHelper)
        {
            DataHandler = dataHandler;
            AccessibleUrlProvider = accessibleUrlProvider;
            ProcessRunner = processRunner;
            MediaInfoOutputConverter = mediaInfoOutputConverter;
            FileStorage = fileStorage;
            ResourceDescriptorHelper = resourceDescriptorHelper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data handler
        /// </summary>
        private IResourceDataHandler DataHandler { get; }

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
        /// Gets the resource url helper
        /// </summary>
        private IResourceDescriptorHelper ResourceDescriptorHelper { get; }

        #endregion

        #region Methods

        private async Task<Locator> GetLocator(string url)
        {
            var uri = new Uri(url, UriKind.Absolute);

            var resourceDescriptor = ResourceDescriptorHelper.GetResourceDescriptor(uri);

            return (Locator)await DataHandler.Get(resourceDescriptor);
        }

        /// <summary>
        /// Runs media info and stores the output to a file
        /// </summary>
        /// <returns></returns>
        public override async Task Execute(JobAssignment jobAssignment)
        {
            // update status and modified date/time
            jobAssignment.JobProcessStatus = "Running";
            jobAssignment.DateModified = DateTime.UtcNow;

            // persist the updated job assignment
            await DataHandler.Update(jobAssignment);

            // get the job process from the job assignment
            var jobProcess = await DataHandler.Get<JobProcess>(jobAssignment.JobProcess);
            if (jobProcess == null)
                throw new Exception("Failed to resolve jobAssignment.jobProcess");
            
            // get the job from the job process
            var job = await DataHandler.Get<Job>(jobProcess.Job.Id);
            if (job == null)
                throw new Exception("Failed to resolve jobProcess.job");

            // load the job profile
            var jobProfile = await DataHandler.Get<JobProfile>(job.JobProfile.Id);
            if (jobProfile == null)
                throw new Exception("Failed to resolve job.jobProfile");

            // ensure this is a valid profile for this worker
            if (jobProfile.Label != "ExtractTechnicalMetadata")
                throw new Exception($"JobProfile '{jobProfile.Label}' not accepted");

            // get the job input
            var jobInput = await DataHandler.Get<JobParameterBag>(job.JobInput.Id);
            if (jobInput == null)
                throw new Exception("Failed to resolve job.jobInput");

            // ensure the job specifies input and output locations
            if (jobInput["fims:inputFile"] == null)
                throw new Exception("Job does not specify an input location.");
            // ensure the job specifies an output location
            if (jobInput["fims:outputLocation"] == null)
                throw new Exception("Job does not specify an output location.");
            
            // get output locator
            var outputLocation = await GetLocator(jobInput["fims:outputLocation"]?.ToString());
            if (outputLocation == null)
                throw new Exception("Failed to resolve jobInput[\"fims:outputLocation\"]");

            // get input locator
            var inputFile = await GetLocator(jobInput["fims:inputFile"]?.ToString());
            if (inputFile == null)
                throw new Exception("Failed to resolve jobInput[\"fims:inputFile\"]");

            // get the url of the file MediaInfo should use (could be local, S3, etc)
            var accessibleUrl = await AccessibleUrlProvider.GetMediaInfoAccessibleUrl(inputFile);
            if (accessibleUrl == null)
                throw new Exception("Input file is not accessible to MediaInfo.");

            var result = await ProcessRunner.RunProcess(Path.Combine(Assembly.GetExecutingAssembly().Location, "bin/mediainfo"),
                                                        "--Output=EBUCore",
                                                        accessibleUrl);

            if (!string.IsNullOrWhiteSpace(result.StdErr))
                throw new Exception($"MediaInfo returned one or more errors: {result.StdErr}.");

            // convert output to JSON
            var mediaInfoJson = MediaInfoOutputConverter.GetJson(result.StdOut);

            // save JSON to file
            await FileStorage.SaveFile(outputLocation, Guid.NewGuid().ToString(), mediaInfoJson);
        }

        #endregion
    }
}
