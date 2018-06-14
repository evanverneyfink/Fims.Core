using System;
using Microsoft.Azure.WebJobs.Description;

namespace Fims.Azure.DependencyInjection
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}
