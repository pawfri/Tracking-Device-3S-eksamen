using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services.Interfaces;

namespace TrackingDeviceLib.Services;

public class AuthService
{
    private readonly IUserDBRepo _repo;

    public AuthService(IUserDBRepo repo)
    {
        _repo = repo;
    }

    // Returner user eller null
    public User? ValidateCredentials(string username, string password)
    {
        var user = _repo.GetByUsername(username);
        if (user == null) return null;
        if (user.Password != password) return null;

        return user;
    }
}
