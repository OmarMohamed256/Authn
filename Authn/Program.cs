using Authn.Data;
using Authn.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/denied";
        options.Events = new CookieAuthenticationEvents()
        {
            OnSigningIn = async context =>
            {
                var scheme = context.Properties.Items.Where(k => k.Key == ".AuthScheme").FirstOrDefault();
                var claim = new Claim(scheme.Key, scheme.Value);
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                var userService = context.HttpContext.RequestServices.GetRequiredService(typeof(UserService)) as UserService;
                var nameIdentifier = claimsIdentity.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userService != null && nameIdentifier != null)
                {
                    var appUser = userService.GetUserByExternalProvider(scheme.Value, nameIdentifier);
                    if(appUser == null)
                    {
                       appUser = userService.AddNewUser(scheme.Value, claimsIdentity.Claims.ToList());
                    }
                    foreach(var r in appUser.RoleList)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, r));
                    }
                }
                claimsIdentity.AddClaim(claim);
            }
        };
    }).AddOpenIdConnect("google", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("GoogleOpenId:Authority");
        options.ClientId = builder.Configuration.GetValue<string>("GoogleOpenId:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("GoogleOpenId:ClientSecret");
        options.CallbackPath = builder.Configuration.GetValue<string>("GoogleOpenId:CallBackPath");
        options.SaveTokens = true;
    }).AddOpenIdConnect("okta", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("OktaOpenId:Authority");
        options.ClientId = builder.Configuration.GetValue<string>("OktaOpenId:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("OktaOpenId:ClientSecret");
        options.CallbackPath = builder.Configuration.GetValue<string>("OktaOpenId:CallBackPath");
        options.SignedOutCallbackPath = builder.Configuration.GetValue<string>("OktaOpenId:SignedOutCallbackPath");
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("offline_access");
    });
    //.AddGoogle(options =>
    //{
    //    options.ClientId = builder.Configuration.GetValue<string>("ClientId");
    //    options.ClientSecret = builder.Configuration.GetValue<string>("ClientSecret");
    //    options.CallbackPath = "/auth";
    //    options.AuthorizationEndpoint += "?prompt=consent";
    //});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
