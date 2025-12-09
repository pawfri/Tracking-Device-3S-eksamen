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

    public UserDBRepo(TrackingDeviceContext context)
    {
        _context = context;
    }

    public User? GetByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        // Case-insensitive search with trimmed input
        return _context.Users
                       .FirstOrDefault(u => u.Email.ToLower() == username.Trim().ToLower());
    }
}
