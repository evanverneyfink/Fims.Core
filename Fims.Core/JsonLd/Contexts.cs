using JsonLD.Core;
using Newtonsoft.Json.Linq;

namespace Fims.Core.JsonLd
{
    public static class Contexts
    {
        public static class Minimal
        {
            /// <summary>
            /// Gets the url for the minimal context
            /// </summary>
            public const string Url = "http://fims.tv/context/minimal";

            /// <summary>
            /// Gets the minimal context
            /// </summary>
            public static readonly Context Context =
                new Context(
                    new JObject
                    {
                        ["@context"] = new JObject
                        {
                            ["dc"] = "http://purl.org/dc/elements/1.1/",
                            ["@default"] = "urn:ebu:metadata-schema:ebuCore_2012",
                            ["ebu"] = "http://ebu.org/nar-extensions/",
                            ["ebucore"] = "http://www.ebu.ch/metadata/ontologies/ebucore/ebucore#",
                            ["fims"] = "http://fims.tv#",
                            ["owl"] = "http://www.w3.org/2002/07/owl#",
                            ["rdf"] = "http://www.w3.org/1999/02/22-rdf-syntax-ns#",
                            ["rdfs"] = "http://www.w3.org/2000/01/rdf-schema#",
                            ["skos"] = "http://www.w3.org/2004/02/skos/core#",
                            ["xsd"] = "http://www.w3.org/2001/XMLSchema#",
                            ["xsi"] = "http://www.w3.org/2001/XMLSchema-instance",

                            ["id"] = "@id",
                            ["type"] = "@type"
                        }
                    });
        }

        public static class Default
        {
            /// <summary>
            /// Gets the url for the default context
            /// </summary>
            public const string Url = "http://fims.tv/context/default";

            /// <summary>
            /// Gets the default context
            /// </summary>
            public static readonly Context Context =
                new Context(
                    new JObject
                    {
                        ["@context"] = new JObject
                        {

                            // Namespace abbreviations

                            ["ebucore"] = "http://www.ebu.ch/metadata/ontologies/ebucore/ebucore#",
                            ["fims"] = "http://fims.tv#",
                            ["other"] = "http//other#",
                            ["owl"] = "http://www.w3.org/2002/07/owl#",
                            ["rdf"] = "http://www.w3.org/1999/02/22-rdf-syntax-ns#",
                            ["rdfs"] = "http://www.w3.org/2000/01/rdf-schema#",
                            ["xsd"] = "http://www.w3.org/2001/XMLSchema#",

                            // General definition

                            ["id"] = "@id",
                            ["type"] = "@type",

                            ["label"] = "rdfs:label",
                            ["url"] = "xsd:anyURI",

                            // EBUcore definitions

                            ["dateCreated"] = "ebucore:dateCreated",
                            ["dateModified"] = "ebucore:dateModified",

                            // FIMS definitions

                            ["Service"] = "fims:Service",
                            ["hasResource"] = new JObject
                            {
                                ["@id"] = "fims:hasServiceResource",
                                ["@type"] = "@id"
                            },

                            ["acceptsJobType"] = new JObject
                            {
                                ["@id"] = "fims:acceptsJobType",

                                ["@type"] = "@id"
                            },
                            ["acceptsJobProfile"] = new JObject
                            {
                                ["@id"] = "fims:acceptsJobProfile",
                                ["@type"] = "@id"
                            },

                            ["inputLocation"] = new JObject
                            {
                                ["@id"] = "fims:hasJobInputLocation",
                                ["@type"] = "@id"
                            },
                            ["outputLocation"] = new JObject
                            {
                                ["@id"] = "fims:hasJobOutputLocation",
                                ["@type"] = "@id"
                            },

                            ["ServiceResource"] = "fims:ServiceResource",
                            ["resourceType"] = new JObject
                            {
                                ["@id"] = "fims:resourceType",
                                ["@type"] = "@id"
                            },

                            ["JobProfile"] = "fims:JobProfile",
                            ["hasInputParameter"] = new JObject
                            {
                                ["@id"] = "fims:hasInputParameter",
                                ["@type"] = "@id"
                            },
                            ["hasOptionalInputParameter"] = new JObject
                            {
                                ["@id"] = "fims:hasOptionalInputParameter",
                                ["@type"] = "@id"
                            },
                            ["hasOutputParameter"] = new JObject
                            {
                                ["@id"] = "fims:hasOutputParameter",
                                ["@type"] = "@id"
                            },

                            ["JobParameter"] = "fims:JobParameter",
                            ["jobProperty"] = new JObject
                            {
                                ["@id"] = "fims:jobProperty",
                                ["@type"] = "@id"
                            },
                            ["parameterType"] = new JObject
                            {
                                ["@id"] = "fims:jobParameterType",
                                ["@type"] = "@id"
                            },

                            ["Locator"] = "fims:Locator",
                            ["httpEndpoint"] = new JObject
                            {
                                ["@id"] = "fims:httpEndpoint",
                                ["@type"] = "xsd:anyURI"
                            },

                            ["awsS3Bucket"] = "fims:amazonWebServicesS3Bucket",
                            ["awsS3Key"] = "fims:amazonWebServicesS3Key",
                            ["azureBlobStorageAccount"] = "fims:microsoftAzureBlobStorageAccount",
                            ["azureBlobStorageContainer"] = "fims:microsoftAzureBlobStorageContainer",
                            ["azureBlobStorageObjectName"] = "fims:microsoftAzureBlobStorageObjectName",
                            ["googleCloudStorageBucket"] = "fims:googleCloudStorageBucket",
                            ["googleCloudStorageObjectName"] = "fims:googleCloudStorageObjectName",
                            ["uncPath"] = "fims:uncPath",

                            ["AmeJob"] = "fims:AmeJob",
                            ["CaptureJob"] = "fims:CaptureJob",
                            ["QAJob"] = "fims:QAJob",
                            ["TransformJob"] = "fims:TransformJob",
                            ["TransferJob"] = "fims:TransferJob",

                            ["jobProfile"] = new JObject
                            {
                                ["@id"] = "fims:hasJobProfile",
                                ["@type"] = "@id"
                            },

                            ["jobStatus"] = new JObject
                            {
                                ["@id"] = "fims:hasJobStatus",
                                ["@type"] = "fims:JobStatus"
                            },

                            ["jobStatusReason"] = new JObject
                            {
                                ["@id"] = "fims:hasJobStatusReason",
                                ["@type"] = "xsd:string"
                            },

                            ["jobProcess"] = new JObject
                            {
                                ["@id"] = "fims:hasJobProcess",
                                ["@type"] = "@id"
                            },

                            ["jobInput"] = new JObject
                            {
                                ["@id"] = "fims:hasJobInput",
                                ["@type"] = "@id"
                            },

                            ["jobOutput"] = new JObject
                            {
                                ["@id"] = "fims:hasJobOutput",
                                ["@type"] = "@id"
                            },

                            ["JobParameterBag"] = "fims:JobParameterBag",

                            ["JobProcess"] = "fims:JobProcess",

                            ["job"] = new JObject
                            {
                                ["@id"] = "fims:hasJob",
                                ["@type"] = "@id"
                            },

                            ["jobProcessStatus"] = new JObject
                            {
                                ["@id"] = "fims:hasJobProcessStatus",
                                ["@type"] = "fims:JobProcessStatus"
                            },

                            ["jobProcessStatusReason"] = new JObject
                            {
                                ["@id"] = "fims:hasJobProcessStatusReason",
                                ["@type"] = "xsd:string"
                            },

                            ["jobAssignment"] = new JObject
                            {
                                ["@id"] = "fims:hasJobAssignment",
                                ["@type"] = "@id"
                            },

                            ["JobAssignment"] = "fims:JobAssignment",

                            ["asyncEndpoint"] = "fims:hasAsyncEndpoint",
                            ["AsyncEndpoint"] = "fims:AsyncEndpoint",

                            ["asyncSuccess"] = new JObject
                            {
                                ["@id"] = "fims:asyncEndpointSuccess",
                                ["@type"] = "xsd:anyURI"
                            },
                            ["asyncFailure"] = new JObject
                            {
                                ["@id"] = "fims:asyncEndpointFailure",
                                ["@type"] = "xsd:anyURI"
                            },

                            // Default namespace for custom attributes

                            ["@vocab"] = "http://other#"
                        }
                    });
        }
    }
}