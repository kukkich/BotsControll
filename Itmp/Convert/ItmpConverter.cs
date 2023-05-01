using System.Reflection;
using System.Text.Json;
using Itmp.Convert.Attributes;
using Itmp.Convert.TypeSource;
using Itmp.Encoding;
using Itmp.Messages;

namespace Itmp.Convert;

public class ItmpConverter
{
    private readonly IMessageTypesSource _messageTypesSource;

    public ItmpConverter(IMessageTypesSource messageTypesSource)
    {
        _messageTypesSource = messageTypesSource;
    }

    public byte[] Serialize(ItmpMessage message)
    {
        var array = message.ToTransferFormat();
        return Cbor.Encode(array);
    }

    public ItmpMessage Deserialize(byte[] bytes)
    {
        var decoded = Cbor.Decode(bytes);
        if (!IsValidArray(decoded, out var payloads) || !IsValidPayloadsType(payloads))
            throw new Exception(); //Todo Сделать нормальное исключение

        var context = new ItmpDeserializationContext(payloads);

        Type? classType = TryFindMessageClassType(context.Type);
        if (classType == null)
            throw new Exception($"No message types with type {context.Type}");

        var constructor = classType.GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConverterConstructorAttribute>().Any())
                ?? throw new Exception($"No constructor with {nameof(ConverterConstructorAttribute)} attribute on class {classType.FullName}");

        if (!IsConstructorParametersValid(constructor, context, out var expectedArgumentType))
            throw new Exception("Invalid constructor parameters");

        // Попытаться десериализовать аргумент если он есть и вызвать конструктор

        return CreateInstance(constructor, context, expectedArgumentType);
    }

    private bool IsValidArray(object? @object, out object[] result)
    {
        if (@object is object[] { Length: 4 or 3 } array)
        {
            result = array;
            return true;
        }

        result = null;
        return false;
    }

    private bool IsValidPayloadsType(object[] payloads)
    {
        return payloads[0] is long && payloads[1] is long && payloads[2] is string;
    }

    private Type? TryFindMessageClassType(ItmpMessageType type)
    {
        var x = _messageTypesSource.GetTypes();

        return _messageTypesSource.GetTypes()
            .FirstOrDefault(t =>
            {
                var attribute = t.GetCustomAttributes<ItmpTypeAttribute>().First();
                return attribute.Type == type;
            });
    }

    private bool IsConstructorParametersValid(
        ConstructorInfo constructor,
        ItmpDeserializationContext context,
        out Type? expectedArgumentType
        )
    {
        expectedArgumentType = null;

        var parameters = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

        switch (context.HasArgument)
        {
            case true when parameters.Length != 3:
            case false when parameters.Length != 2:
                return false;
        }

        if (!(parameters[0] == context.MessageId.GetType())
            || !(parameters[1] == context.Topic.GetType()))
            return false;

        expectedArgumentType = parameters.Length == 3 ? parameters[2] : null;
        return true;
    }

    private ItmpMessage CreateInstance(
        ConstructorInfo constructor,
        ItmpDeserializationContext context,
        Type? expectedArgumentType
    )
    {
        object[] constructorParameters;
        if (expectedArgumentType != null)
        {
            var bytes = JsonSerializer.Serialize(context.Argument!);
            object typedArgument = JsonSerializer.Deserialize(bytes, expectedArgumentType, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            constructorParameters = new[] {context.MessageId, context.Topic, typedArgument};
        }
        else
        {
            constructorParameters = new object[] { context.MessageId, context.Topic };
        }

        return (ItmpMessage)constructor.Invoke(constructorParameters);
    }
}