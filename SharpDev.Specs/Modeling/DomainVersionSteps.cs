using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpDev.Modeling
{
    [Binding]
    public sealed class DomainVersionSteps
    {
        [Given(@"A new major version is created with (.*)")]
        public void GivenANewMajorVersionIsCreatedWith(int major)
        {
            ScenarioContext.Current.Set(new DomainVersion(major));
        }
        [Given(@"A new major\.minor version is created with (.*) and (.*)")]
        public void GivenANewMajor_MinorVersionIsCreatedWithAnd(int major, int minor)
        {
            ScenarioContext.Current.Set(new DomainVersion(major, minor));
        }
        [Given(@"A new major\.minor\.revision version is created with (.*), (.*) and (.*)")]
        public void GivenANewMajor_Minor_RevisionVersionIsCreatedWithAnd(int major, int minor, int revision)
        {
            ScenarioContext.Current.Set(new DomainVersion(major, minor, revision));
        }
        [Given(@"A new major\.minor\.revision\.build version is created with (.*), (.*), (.*) and (.*)")]
        public void GivenANewMajor_Minor_Revision_BuildVersionIsCreatedWithAnd(int major, int minor, int revision, int build)
        {
            ScenarioContext.Current.Set(new DomainVersion(major, minor, revision, build));
        }

        [When(@"The version is printed")]
        public void WhenTheVersionIsPrinted()
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            var printed = version.ToString();
            ScenarioContext.Current.Set(printed, "printed");
        }

        [Then(@"The printed version looks like ""(.*)""")]
        public void ThenThePrintedVersionLooksLike(string like)
        {
            var printed = ScenarioContext.Current.Get<string>("printed");

            Assert.AreEqual(like, printed);
        }
    }
}
