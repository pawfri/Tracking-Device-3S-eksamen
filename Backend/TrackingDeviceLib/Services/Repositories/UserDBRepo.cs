using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Data;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services.Interfaces;

namespace TrackingDeviceLib.Services.Repositories;

public class UserDBRepo : IUserDBRepo
{
	private readonly TrackingDeviceContext _context;
	public User? GetByUsername(string username)
	{
		return _context.Users.Find(username);
	}
}
