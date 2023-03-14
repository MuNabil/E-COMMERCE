namespace API.Extentions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> GetUserWithAddressFromClaimsAsync(this UserManager<AppUser> userManager,
             ClaimsPrincipal user)
            => await userManager.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));


        public static async Task<AppUser> GetUserByEmailFromClaimsAsync(this UserManager<AppUser> userManager,
             ClaimsPrincipal user)
            => await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
    }
}