﻿using Blog.Api.Data;
using Blog.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Api.Identity;

public class IdentitySeed
{
	public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
	{
		// Seed Roles
		await SeedRolesAsync(roleManager);

		// Seed Admin User
		await SeedAdminUserAsync(userManager);
	}

	private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
	{
		// Seed Roles
		if (!await roleManager.RoleExistsAsync(Roles.Admin))
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
		}
	}

	private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
	{
		// Seed Admin User
		if (await userManager.FindByEmailAsync("thors@vinland.eu") == null)
		{
			AppUser user = new AppUser
			{
				UserName = "Thors",
				Email = "thors@vinland.eu",
			};

			IdentityResult result = userManager.CreateAsync(user, "Passw0rd&321").Result;

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, Roles.Admin);
			}
		}
	}
}
