using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fims.Core.JsonLd;
using Fims.Core.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using JsonLdProcessor = Fims.Core.JsonLd.JsonLdProcessor;

namespace Fims.Core.Tests
{
    [TestClass]
    public class JsonLdResourceTests
    {
        private IJsonLdContextManager JsonLdContextManager { get; set; }

        private IJsonLdResourceHelper JsonLdResourceHelper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            JsonLdContextManager = new JsonLdContextManager(null);
            JsonLdResourceHelper =
                new JsonLdResourceHelper(JsonLdContextManager, new JsonLdProcessor(null, new CachedDocumentLoader(JsonLdContextManager)));
        }

        [TestMethod]
        public async Task AllowsCreatingAJobProfile()
        {
            var jobProfile = new JobProfile("ExtractThumbnail",
                                            new JArray
                                            {
                                                new JobParameter("fims:inputFile", "fims:Locator"),
                                                new JobParameter("fims:outputLocation", "fims:Locator")
                                            },
                                            new JArray
                                            {
                                                new JobParameter("fims:outputFile", "fims:Locator")
                                            },
                                            new JArray
                                            {
                                                new JobParameter("ebucore:width"),
                                                new JobParameter("ebucore:height")
                                            });

            var output = await JsonLdResourceHelper.GetJsonFromResource(jobProfile, new JObject());

            output.Should().NotBeNull();

            output["@type"].Value<string>().Should().Be("http://fims.tv#JobProfile");
            output["http://www.w3.org/2000/01/rdf-schema#label"].Value<string>().Should().Be("ExtractThumbnail");

            output["http://fims.tv#hasInputParameter"].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"].Count().Should().Be(2);
            output["http://fims.tv#hasInputParameter"][0].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][0]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameter");
            output["http://fims.tv#hasInputParameter"][0]["http://fims.tv#jobParameterType"].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][0]["http://fims.tv#jobParameterType"]["@id"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasInputParameter"][0]["http://fims.tv#jobProperty"].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][0]["http://fims.tv#jobProperty"]["@id"].Value<string>().Should().Be("http://fims.tv#inputFile");
            output["http://fims.tv#hasInputParameter"][1].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][1]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameter");
            output["http://fims.tv#hasInputParameter"][1]["http://fims.tv#jobParameterType"].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][1]["http://fims.tv#jobParameterType"]["@id"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasInputParameter"][1]["http://fims.tv#jobProperty"].Should().NotBeNull();
            output["http://fims.tv#hasInputParameter"][1]["http://fims.tv#jobProperty"]["@id"].Value<string>().Should().Be("http://fims.tv#outputLocation");

            output["http://fims.tv#hasOptionalInputParameter"].Should().NotBeNull();
            output["http://fims.tv#hasOptionalInputParameter"].Count().Should().Be(2);
            output["http://fims.tv#hasOptionalInputParameter"][0].Should().NotBeNull();
            output["http://fims.tv#hasOptionalInputParameter"][0]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameter");
            output["http://fims.tv#hasOptionalInputParameter"][0]["http://fims.tv#jobProperty"].Should().NotBeNull();
            output["http://fims.tv#hasOptionalInputParameter"][0]["http://fims.tv#jobProperty"]["@id"].Value<string>().Should().Be("http://www.ebu.ch/metadata/ontologies/ebucore/ebucore#width");
            output["http://fims.tv#hasOptionalInputParameter"][1].Should().NotBeNull();
            output["http://fims.tv#hasOptionalInputParameter"][1]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameter");
            output["http://fims.tv#hasOptionalInputParameter"][1]["http://fims.tv#jobProperty"].Should().NotBeNull();
            output["http://fims.tv#hasOptionalInputParameter"][1]["http://fims.tv#jobProperty"]["@id"].Value<string>().Should().Be("http://www.ebu.ch/metadata/ontologies/ebucore/ebucore#height");

            output["http://fims.tv#hasOutputParameter"].Should().NotBeNull();
            output["http://fims.tv#hasOutputParameter"]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameter");
            output["http://fims.tv#hasOutputParameter"]["http://fims.tv#jobParameterType"].Should().NotBeNull();
            output["http://fims.tv#hasOutputParameter"]["http://fims.tv#jobParameterType"]["@id"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasOutputParameter"]["http://fims.tv#jobProperty"].Should().NotBeNull();
            output["http://fims.tv#hasOutputParameter"]["http://fims.tv#jobProperty"]["@id"].Value<string>().Should().Be("http://fims.tv#outputFile");
        }

        [TestMethod]
        public async Task AllowsCreatingAService()
        {
            var service = new Service("FFmpeg TransformService",
                                      new JArray
                                      {
                                          new ServiceResource("fims:JobAssignment", "http://transformServiceUrl/JobAssignment")
                                      },
                                      "fims:TransformJob",
                                      new JArray
                                      {
                                          "http://urlToExtractThumbnailJobProfile/",
                                          "http://urlToCreateProxyJobProfile/"
                                      },
                                      new JArray
                                      {
                                          new Locator(new JObject {["awsS3Bucket"] = "private-repo.fims.tv"})
                                      },
                                      new JArray
                                      {
                                          new Locator(new JObject {["awsS3Bucket"] = "private-repo.fims.tv"})
                                      });

            var output = await JsonLdResourceHelper.GetJsonFromResource(service, new JObject());
            
            output["@type"].Value<string>().Should().Be("http://fims.tv#Service");
            output["http://www.w3.org/2000/01/rdf-schema#label"].Value<string>().Should().Be("FFmpeg TransformService");

            output["http://fims.tv#hasServiceResource"].Should().NotBeNull();
            output["http://fims.tv#hasServiceResource"]["@type"].Value<string>().Should().Be("http://fims.tv#ServiceResource");
            output["http://fims.tv#hasServiceResource"]["http://fims.tv#resourceType"].Should().NotBeNull();
            output["http://fims.tv#hasServiceResource"]["http://fims.tv#resourceType"]["@id"].Value<string>().Should().Be("http://fims.tv#JobAssignment");
            output["http://fims.tv#hasServiceResource"]["http://fims.tv#httpEndpoint"].Should().NotBeNull();
            output["http://fims.tv#hasServiceResource"]["http://fims.tv#httpEndpoint"]["@type"].Value<string>().Should().Be("http://www.w3.org/2001/XMLSchema#anyURI");
            output["http://fims.tv#hasServiceResource"]["http://fims.tv#httpEndpoint"]["@value"].Value<string>().Should().Be("http://transformServiceUrl/JobAssignment");

            output["http://fims.tv#acceptsJobProfile"].Should().NotBeNull();
            output["http://fims.tv#acceptsJobProfile"].Count().Should().Be(2);
            output["http://fims.tv#acceptsJobProfile"][0].Should().NotBeNull();
            output["http://fims.tv#acceptsJobProfile"][0]["@id"].Value<string>().Should().Be("http://urlToExtractThumbnailJobProfile/");
            output["http://fims.tv#acceptsJobProfile"][1].Should().NotBeNull();
            output["http://fims.tv#acceptsJobProfile"][1]["@id"].Value<string>().Should().Be("http://urlToCreateProxyJobProfile/");

            output["http://fims.tv#acceptsJobType"].Should().NotBeNull();
            output["http://fims.tv#acceptsJobType"]["@id"].Value<string>().Should().Be("http://fims.tv#TransformJob");

            output["http://fims.tv#hasJobInputLocation"].Should().NotBeNull();
            output["http://fims.tv#hasJobInputLocation"]["@type"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasJobInputLocation"]["http://fims.tv#amazonWebServicesS3Bucket"].Value<string>().Should().Be("private-repo.fims.tv");

            output["http://fims.tv#hasJobOutputLocation"].Should().NotBeNull();
            output["http://fims.tv#hasJobOutputLocation"]["@type"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasJobOutputLocation"]["http://fims.tv#amazonWebServicesS3Bucket"].Value<string>().Should().Be("private-repo.fims.tv");
        }

        [TestMethod]
        public async Task AllowsCreatingATransformJob()
        {
            var transformJob = new TransformJob("http://urlToExtractThumbnailJobProfile/",
                                                new JobParameterBag(
                                                    new JObject
                                                    {
                                                        ["fims:inputFile"] = new JObject
                                                        {
                                                            ["type"] = "fims:Locator",
                                                            ["awsS3Bucket"] = "private-repo.fims.tv",
                                                            ["awsS3Key"] = "media-file.mp4"
                                                        },
                                                        ["fims:outputLocation"] = new JObject
                                                        {
                                                            ["type"] = "fims:Locator",
                                                            ["awsS3Bucket"] = "private-repo.fims.tv",
                                                            ["awsS3Key"] = "thumbnails/"
                                                        }
                                                    }),
                                                new AsyncEndpoint("http://urlForJobSuccess", "http://urlForJobFailed"));

            var output = await JsonLdResourceHelper.GetJsonFromResource(transformJob, new JObject());

            output["@type"].Value<string>().Should().Be("http://fims.tv#TransformJob");

            output["http://fims.tv#hasJobProfile"].Should().NotBeNull();
            output["http://fims.tv#hasJobProfile"]["@id"].Value<string>().Should().Be("http://urlToExtractThumbnailJobProfile/");

            output["http://fims.tv#hasJobInput"].Should().NotBeNull();
            output["http://fims.tv#hasJobInput"]["@type"].Value<string>().Should().Be("http://fims.tv#JobParameterBag");
            output["http://fims.tv#hasJobInput"]["http://fims.tv#inputFile"].Should().NotBeNull();
            output["http://fims.tv#hasJobInput"]["http://fims.tv#inputFile"]["@type"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasJobInput"]["http://fims.tv#inputFile"]["http://fims.tv#amazonWebServicesS3Bucket"]
                .Value<string>().Should().Be("private-repo.fims.tv");
            output["http://fims.tv#hasJobInput"]["http://fims.tv#inputFile"]["http://fims.tv#amazonWebServicesS3Key"]
                .Value<string>().Should().Be("media-file.mp4");

            output["http://fims.tv#hasJobInput"]["http://fims.tv#outputLocation"].Should().NotBeNull();
            output["http://fims.tv#hasJobInput"]["http://fims.tv#outputLocation"]["@type"].Value<string>().Should().Be("http://fims.tv#Locator");
            output["http://fims.tv#hasJobInput"]["http://fims.tv#outputLocation"]["http://fims.tv#amazonWebServicesS3Bucket"]
                .Value<string>().Should().Be("private-repo.fims.tv");
            output["http://fims.tv#hasJobInput"]["http://fims.tv#outputLocation"]["http://fims.tv#amazonWebServicesS3Key"]
                .Value<string>().Should().Be("thumbnails/");

            output["http://fims.tv#hasJobStatus"].Should().NotBeNull();
            output["http://fims.tv#hasJobStatus"]["@type"].Value<string>().Should().Be("http://fims.tv#JobStatus");
            output["http://fims.tv#hasJobStatus"]["@value"].Value<string>().Should().Be("New");

            output["http://fims.tv#hasAsyncEndpoint"].Should().NotBeNull();
            output["http://fims.tv#hasAsyncEndpoint"]["@type"].Value<string>().Should().Be("http://fims.tv#AsyncEndpoint");
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointFailure"].Should().NotBeNull();
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointFailure"]["@type"]
                .Value<string>().Should().Be("http://www.w3.org/2001/XMLSchema#anyURI");
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointFailure"]["@value"]
                .Value<string>().Should().Be("http://urlForJobFailed");
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointSuccess"].Should().NotBeNull();
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointSuccess"]["@type"]
                .Value<string>().Should().Be("http://www.w3.org/2001/XMLSchema#anyURI");
            output["http://fims.tv#hasAsyncEndpoint"]["http://fims.tv#asyncEndpointSuccess"]["@value"]
                .Value<string>().Should().Be("http://urlForJobSuccess");
        }

        [TestMethod]
        public async Task AllowsCreatingAJobProcess()
        {
            var jobProcess = new JobProcess("http://urlToTransformJob");

            var output = await JsonLdResourceHelper.GetJsonFromResource(jobProcess, new JObject());

            output["@type"].Value<string>().Should().Be("http://fims.tv#JobProcess");

            output["http://fims.tv#hasJob"].Should().NotBeNull();
            output["http://fims.tv#hasJob"]["@id"].Value<string>().Should().Be("http://urlToTransformJob");

            output["http://fims.tv#hasJobProcessStatus"].Should().NotBeNull();
            output["http://fims.tv#hasJobProcessStatus"]["@type"].Value<string>().Should().Be("http://fims.tv#JobProcessStatus");
            output["http://fims.tv#hasJobProcessStatus"]["@value"].Value<string>().Should().Be("New");
        }

        [TestMethod]
        public async Task AllowsCreatingAJobAssignment()
        {
            var jobAssignment = new JobAssignment("http://urlToJobProcess");

            var output = await JsonLdResourceHelper.GetJsonFromResource(jobAssignment, new JObject());

            output["@type"].Value<string>().Should().Be("http://fims.tv#JobAssignment");

            output["http://fims.tv#hasJobProcess"].Should().NotBeNull();
            output["http://fims.tv#hasJobProcess"]["@id"].Value<string>().Should().Be("http://urlToJobProcess");

            output["http://fims.tv#hasJobProcessStatus"].Should().NotBeNull();
            output["http://fims.tv#hasJobProcessStatus"]["@type"].Value<string>().Should().Be("http://fims.tv#JobProcessStatus");
            output["http://fims.tv#hasJobProcessStatus"]["@value"].Value<string>().Should().Be("New");
        }

        [TestMethod]
        public async Task AllowsGettingAService()
        {
            var resource =
                await JsonLdResourceHelper.GetResourceFromJson(JToken.Parse(File.ReadAllText("service.json")),
                                                               typeof(Service));

            resource.Should().NotBeNull();
            resource.Should().BeOfType<Service>();
        }
    }
}
