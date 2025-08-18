using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PartnerFlow.Application.Services;
using PartnerFlow.Application.Validations;
using PartnerFlow.Domain.Interfaces.Broker;
using PartnerFlow.Domain.Interfaces.Cache;
using PartnerFlow.Domain.Interfaces.Repositories;
using PartnerFlow.Domain.Interfaces.Services;
using PartnerFlow.Infrastructure.Broker;
using PartnerFlow.Infrastructure.Cache;
using PartnerFlow.Infrastructure.Config;
using PartnerFlow.Infrastructure.Persistence.Mongo;
using PartnerFlow.Infrastructure.Persistence.Sql;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PartnerFlow API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddAuthorization();

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<MongoContext>();

builder.Services.AddDbContext<PartnerFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IPedidoRepository, PedidoSqlRepository>();

builder.Services.AddScoped<IItemPedidoRepository, ItemPedidoMongoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<PedidoDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();