﻿using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class CaptureJob : Job
    {
        public CaptureJob()
        {
        }

        public CaptureJob(JToken jobProfileId, JToken jobInput, JToken asyncEndpointToken)
            : base(jobProfileId, jobInput, asyncEndpointToken)
        {
        }
    }
}