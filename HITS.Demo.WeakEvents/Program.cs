using HITS.Demo.WeakEvents.Shared;

var builder = WebApplication.CreateBuilder(args);
InitConfiguration(builder);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HITS.Demo.WeakEvents.Services.SessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


partial class Program
{
    public static AppSettingsModel AppSettings { get; set; }

    public static void InitConfiguration(WebApplicationBuilder builder)
    {
        IoHelper.ExecutingDirectory = IoHelper.GetExecutingDirectory().FullName;
        LogDataManager.Instance.Subscribe();        

        AppSettings = new AppSettingsModel();
        AppSettings.GetIpLocation = builder.Configuration["Features:GetIpLocation"];
        AppSettings.FilterForUsOnly = builder.Configuration["Features:FilterForUsOnly"];
        //My secret ipapikey
        //https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
        //replace with your own ipapi key
        AppSettings.ipapikey = builder.Configuration["Keys:ipapikey"];  
    }
}
