using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fims.Core.Model;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Jobs
{
    public static class JobValidationExtensions
    {
        /// <summary>
        /// Validates a job before processing
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static void Validate(this Job job)
        {
            // ensure profile is set
            if (job.JobProfile == null)
                throw new Exception("Missing JobProfile");

            if (job.Profile.InputParameters != null && job.Profile.InputParameters.Any())
            {
                foreach (var inputParam in job.Profile.InputParameters)
                {
                    if (inputParam.JobProperty == null)
                        throw new Exception("Invalid JobProfile: inputParameter without 'fims:jobProperty' detected");
                    if (string.IsNullOrWhiteSpace(inputParam.JobPropertyId))
                        throw new Exception("Invalid JobProfile: inputParameter with wrongly defined 'fims:jobProperty' detected");

                    var inputPropertyName = inputParam.JobPropertyId;

                    if (job.JobInput[inputPropertyName] == null)
                        throw new Exception($"Invalid Job: Missing required input parameter '{inputPropertyName}'");

                    if (inputParam.ParameterType != null &&
                        !string.IsNullOrWhiteSpace(inputParam.ParameterTypeId) &&
                        job.JobInput[inputPropertyName]?["type"].Value<string>() != inputParam.ParameterTypeId)
                        throw new Exception($"Invalid Job: Required input parameter '{inputPropertyName}' has wrong type");
                }
            }
        }

        public static bool CanAcceptJob(this Service service, Job job)
        {
            return
                service.JobTypes != null &&
                service.JobTypes.Any(jt => job.Type == jt.Id) &&
                service.JobProfiles != null &&
                service.JobProfiles.Any(jp => job.Profile.Type == jp.Id);
        }
    }
}
