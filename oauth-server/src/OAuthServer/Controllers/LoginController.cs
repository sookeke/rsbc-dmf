﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuthServer.Controllers
{
    [AllowAnonymous]
    [Route("")]
    public class LoginController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IProfileService profileService;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider authenticationSchemeProvider;

        public LoginController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            IProfileService profileService,
            IClientStore clientStore,
            IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            this.interaction = interaction;
            this.events = events;
            this.profileService = profileService;
            this.clientStore = clientStore;
            this.authenticationSchemeProvider = authenticationSchemeProvider;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var scheme = await GetAuthenticationSchemeForClient(returnUrl);
            if (string.IsNullOrEmpty(scheme)) return BadRequest($"No is client defined for {returnUrl}");

            return RedirectToAction(nameof(Challenge), new { scheme, returnUrl });
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet("challenge")]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl) && !interaction.IsValidReturnUrl(returnUrl)) return BadRequest($"No client is defined for {returnUrl}");

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
            {
                { "returnUrl", returnUrl },
                { "scheme", scheme },
            }
            };

            return Challenge(props, scheme);
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var externalUser = result.Principal;
            if (externalUser == null)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var userId = externalUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = externalUser.FindFirstValue(JwtClaimTypes.SessionId);
            var scheme = result.Properties.Items["scheme"];

            // retrieve returnUrl
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // use the user information to find your user in your database, or provision a new user
            //var user = FindUserFromExternalProvider(scheme, userId);

            var additionalClaims = new List<Claim>();
            if (sessionId != null) additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sessionId));

            // issue authentication cookie for user
            await HttpContext.SignInAsync(new IdentityServerUser(userId)
            {
                DisplayName = externalUser.FindFirstValue("given_name") + " " + externalUser.FindFirstValue("last_name"),
                IdentityProvider = scheme,
                AuthenticationTime = DateTime.Now,
                AdditionalClaims = additionalClaims
            }.CreatePrincipal());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // return back to protocol processing
            return Redirect(returnUrl);
        }

        private async Task<string> GetAuthenticationSchemeForClient(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            var client = await clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

            var scheme = context?.IdP != null
                ? await authenticationSchemeProvider.GetSchemeAsync(context.IdP)
                : (await authenticationSchemeProvider.GetAllSchemesAsync()).FirstOrDefault(s => client.IdentityProviderRestrictions.Contains(s.Name));

            return scheme.Name;
        }
    }
}