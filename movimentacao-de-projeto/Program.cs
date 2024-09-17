using movimentacao_de_projeto.Profiles;
using movimentacao_de_projeto.Services;
using Org.BouncyCastle.Asn1.X509.Qualified;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProdutoInterface, ProdutoServices>();
builder.Services.AddAutoMapper(typeof(ProfileAutoMapper).Assembly);
builder.Services.AddScoped<IMovimentacaoService, MovimentacaoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
