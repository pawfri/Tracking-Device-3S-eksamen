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
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        // Get user from DB using case-insensitive search
        var user = _repo.GetByUsername(username.Trim());
        if (user == null) return null;

        // Simple password check
        if (user.Password != password.Trim()) return null;

        return user;
    }
}
