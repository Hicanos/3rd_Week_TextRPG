using Newtonsoft.Json;
using System;
using TextRPG.SkillManagement;

public class SkillJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Skill).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var skill = (Skill)value;
        var skillData = new
        {
            Type = skill.GetType().AssemblyQualifiedName, // 클래스 타입 정보 포함
            Data = skill
        };
        serializer.Serialize(writer, skillData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jsonObject = serializer.Deserialize<dynamic>(reader);
        string typeName = jsonObject.Type;
        Type type = Type.GetType(typeName);
        
        if (type == null)
        {
            throw new JsonSerializationException($"Unknown type: {typeName}");
        }

        var data = jsonObject.Data.ToString();
        return JsonConvert.DeserializeObject(data, type);
    }
}
