// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.ServiceLayer.EntityFramework;
using Iris.Web.IrisMembership;
using StructureMap.Web;

namespace IAUNSportsSystem.Web.DependencyResolution
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            //this.Forward<IViewEngine,RazorViewEngine>();

            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));

            //For<IViewEngine>().Use(() => new RazorViewEngine());


            For<IPrincipalService>().Use<IrisSupportPrincipalService>();
            For<IFormsAuthenticationService>().Use<FormsAuthenticationService>();

            For<IDbContext>().HybridHttpOrThreadLocalScoped().Use<SportsSystemDbContext>();


            For<ISportService>().Use<SportService>();
            For<IPresentedSportService>().Use<PresentedSportService>();
            For<ICompetitionService>().Use<CompetitionService>();
            For<IUniversityService>().Use<UniversityService>();
            For<IRepresentativeUserService>().Use<RepresentativeUserService>();
            For<IParticipationService>().Use<ParticipationService>();
            For<IStudyFieldService>().Use<StudyFieldService>();
            For<IStudyFieldDegreeService>().Use<StudyFieldDegreeService>();
            For<ICompetitorService>().Use<CompetitorService>();
            For<ICompetitionSportService>().Use<CompetitionSportService>();
            For<IUserService>().Use<UserService>();
            For<ISlideShowService>().Use<SlideShowService>();
            For<ITechnicalStaffService>().Use<TechnicalStaffService>();
            For<ITechnicalStaffRoleService>().Use<TechnicalStaffRoleService>();
            For<ICommonTechnicalStaffService>().Use<CommonTechnicalStaffService>();
            For<ITeamColorService>().Use<TeamColorService>();
            For<IDormService>().Use<DormService>();
            For<ICompetitionRepresentativeUserService>().Use<CompetitionRepresentativeUserService>();
            For<IAnnouncementService>().Use<AnnouncementService>();
            For<INewsService>().Use<NewsService>();
        }

        #endregion
    }
}