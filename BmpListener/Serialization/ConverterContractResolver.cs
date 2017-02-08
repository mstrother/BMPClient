using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace BmpListener.Serialization
{
    public class ConverterContractResolver : DefaultContractResolver
    {
        public static readonly ConverterContractResolver Instance = new ConverterContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            // this will only be called once and then cached
            if (objectType == typeof(DateTime) || objectType == typeof(DateTimeOffset))
            {
                contract.Converter = new JavaScriptDateTimeConverter();
            }

            return contract;
        }
    }
}
