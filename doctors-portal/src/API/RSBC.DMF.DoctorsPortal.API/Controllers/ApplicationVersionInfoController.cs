﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RSBC.DMF.DoctorsPortal.API.ViewModels;
using System;
using System.Diagnostics;
using System.Reflection;

namespace RSBC.DMF.DoctorsPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // public API
    public class ApplicationVersionInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApplicationVersionInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Return the version of the running application
        /// </summary>
        /// <returns>The version of the running application</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetApplicationVersionInfo()
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            DateTime creationTime = System.IO.File.GetLastWriteTimeUtc(assembly.Location);
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = "!" + fvi.ProductVersion;
            
            /*
            SourceCommit = _configuration["OPENSHIFT_BUILD_COMMIT"],
            SourceRepository = _configuration["OPENSHIFT_BUILD_SOURCE"],
            SourceReference = _configuration["OPENSHIFT_BUILD_REFERENCE"],
            */

            ApplicationVersionInfo avi = new ApplicationVersionInfo
            {
                BaseUri = _configuration["BASE_URI"],
                BasePath = _configuration["BASE_PATH"],
                Environment = _configuration["ASPNETCORE_ENVIRONMENT"],
                SourceCommit = _configuration["OPENSHIFT_BUILD_COMMIT"],
                SourceRepository = _configuration["OPENSHIFT_BUILD_SOURCE"],
                SourceReference = _configuration["OPENSHIFT_BUILD_REFERENCE"],
                FileCreationTime = creationTime.ToString("O"), // Use the round trip format as it includes the time zone.
                FileVersion = fileVersion
            };

            return new JsonResult(avi);
        }

    }

}