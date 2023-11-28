var builder = WebApplication.CreateBuilder(args);

//注入微服务默认服务
builder.AddServiceDefaults();
//注入控制器相关服务
builder.Services.AddControllers();
//注入NSwag
builder.Services.ConfigureNSwag();
//注入FreeSql
builder.ConfigureFreeSql();
//注入OpenIddict相关
//builder.ConfigureOpenIddictWithEfCore();
builder.ConfigureOpenIddictWithFreeSql();

//注入应用服务
builder.Services.AddAppServices();

var app = builder.Build();

//输出程序启动日志
app.Logger.StartingApp(DateTime.Now);
//注册程序生命周期事件
app.RegisterApplicationLifeTimeEvents();
//执行FreeSql的一些初始化操作
app.InitFreeSql();

//注册NSwag中间件
app.UseNSwag();

app.MapDefaultEndpoints();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
