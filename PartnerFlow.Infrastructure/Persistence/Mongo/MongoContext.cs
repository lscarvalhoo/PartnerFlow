using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PartnerFlow.Domain.Entities;
using PartnerFlow.Infrastructure.Config;

namespace PartnerFlow.Infrastructure.Persistence.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<ItemPedido> ItensPedido => _database.GetCollection<ItemPedido>("ItensPedido");
}