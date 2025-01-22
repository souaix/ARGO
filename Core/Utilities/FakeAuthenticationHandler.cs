﻿using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class FakeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public FakeAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock) : base(options, logger, encoder, clock)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new[] { new Claim(ClaimTypes.Name, "TestUser") };
		var identity = new ClaimsIdentity(claims, "FakeScheme");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, "FakeScheme");

		return Task.FromResult(AuthenticateResult.Success(ticket));
	}
}
