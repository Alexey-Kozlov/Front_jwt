using System;
using System.Collections.Generic;
using Front.Models;
using System.Threading.Tasks;

namespace Front.Services
{
	public interface IAuthenticationService 
	{
		Task<UserSession> RestoreSessionAsync(int sessionId);
	}
}
