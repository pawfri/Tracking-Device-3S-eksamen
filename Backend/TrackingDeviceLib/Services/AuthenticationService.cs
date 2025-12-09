using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Services.Interfaces;

namespace TrackingDeviceLib.Services;

public class AuthenticationService
{
	private readonly IUserDBRepo _repo;

	public AuthenticationService(IUserDBRepo repo)
	{
		_repo = repo;
	}

	public bool Login(string username, string password)
	{
		var user = _repo.GetByUsername(username);
		if (user == null) return false;
		return user.Password == password;
	}

	public void Logout()
	{
		// token cleanup, session clear osv.
		//HttpContext.Session.Clear();
	}
}
