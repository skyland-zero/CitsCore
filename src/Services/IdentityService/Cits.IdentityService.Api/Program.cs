var builder = WebApplication.CreateBuilder(args);

//ע��΢����Ĭ�Ϸ���
builder.AddServiceDefaults();
//ע���������ط���
builder.Services.AddControllers();
//ע��NSwag
builder.Services.ConfigureNSwag();
//ע��FreeSql
builder.ConfigureFreeSql();
//ע��OpenIddict���
//builder.ConfigureOpenIddictWithEfCore();
builder.ConfigureOpenIddictWithFreeSql();

//ע��Ӧ�÷���
builder.Services.AddAppServices();

var app = builder.Build();

//�������������־
app.Logger.StartingApp(DateTime.Now);
//ע��������������¼�
app.RegisterApplicationLifeTimeEvents();
//ִ��FreeSql��һЩ��ʼ������
app.InitFreeSql();

//ע��NSwag�м��
app.UseNSwag();

app.MapDefaultEndpoints();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
