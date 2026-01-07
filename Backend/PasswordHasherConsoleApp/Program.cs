using Microsoft.AspNetCore.Identity;
using TrackingDeviceLib.Models;

var user = new User { Email = "user" };
var hasher = new PasswordHasher<User>();

string hash = hasher.HashPassword(user, "password");

Console.WriteLine(hash);
