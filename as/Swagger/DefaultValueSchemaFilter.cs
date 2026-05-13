using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace sa.Swagger;

/// <summary>
/// Makes Swashbuckle use [DefaultValue(...)] as the example value in the schema,
/// so booleans decorated with [DefaultValue(false)] show false instead of true.
/// </summary>
public class DefaultValueSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo == null) return;

        var attr = context.MemberInfo.GetCustomAttribute<DefaultValueAttribute>();
        if (attr?.Value is bool boolVal)
            schema.Example = new OpenApiBoolean(boolVal);
    }
}
