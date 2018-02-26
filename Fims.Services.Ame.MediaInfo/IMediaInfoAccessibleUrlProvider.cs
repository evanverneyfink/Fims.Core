using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Services.Ame.MediaInfo
{
    public class ProcessOutput
    {
        /// <summary>
        /// Instantiates a <see cref="ProcessOutput"/>
        /// </summary>
        /// <param name="stdOut"></param>
        /// <param name="stdErr"></param>
        public ProcessOutput(string stdOut, string stdErr)
        {
            StdOut = stdOut;
            StdErr = stdErr;
        }

        /// <summary>
        /// Gets the text written to stdout of the process
        /// </summary>
        public string StdOut { get; }

        /// <summary>
        /// Gets the text written to stderr of the process
        /// </summary>
        public string StdErr { get; }
    }

    public interface IProcessRunner
    {
        /// <summary>
        /// Runs a process with the given args
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<ProcessOutput> RunProcess(string path, params string[] args);
    }

    public interface IMediaInfoAccessibleUrlProvider
    {
        /// <summary>
        /// Gets a url for a given <see cref="Locator"/> that's accessible by MediaInfo in the given context
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        Task<string> GetMediaInfoAccessibleUrl(Locator locator);
    }
}