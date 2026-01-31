using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Axia.Veiculos.WebApi.Configurations;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        var enumValues = Enum.GetNames(context.Type);
        var enumDescriptions = string.Join(" | ", enumValues.Select((name, index) =>
            $"{index + 1} = {name}"));

        schema.Description = $"Valores: {enumDescriptions}";
        schema.Enum.Clear();

        foreach (var name in enumValues)
        {
            schema.Enum.Add(new OpenApiString(name));
        }
    }
}
