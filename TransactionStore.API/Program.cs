using TransactionStore.BLL;
using TransactionStore.Contracts;
using TransactionStore.DAL;
using TransactionStore.Mapper;
using NLog;
using LogManager = NLog.LogManager;
using ILogger = NLog.ILogger;
using TransactionStore.API.Validations;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();
builder.Services.AddSingleton<ILogger>(nlog);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionManager, TransactionManager>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<TransactionValidator>();
builder.Services.AddSingleton<TransferTransactionValidator>();
builder.Services.AddSingleton<Context>();
builder.Services.AddAutoMapper(typeof(TransferProfile));
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
