using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Enums;

namespace TaskProcessor.Infrastructure.Data.MongoDb.Configurations
{
    public class TarefaConfiguration
    {
        public static void Configure()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Tarefa)))
                return;

            BsonClassMap.RegisterClassMap<Tarefa>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);

                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(CombGuidGenerator.Instance);

                cm.MapMember(c => c.Status)
                  .SetSerializer(new EnumSerializer<StatusTarefaEnum>(BsonType.String));

                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
