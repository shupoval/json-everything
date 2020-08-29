﻿using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema
{
	[SchemaKeyword(Name)]
	[SchemaDraft(Draft.Draft6)]
	[SchemaDraft(Draft.Draft7)]
	[SchemaDraft(Draft.Draft201909)]
	[Vocabulary(Vocabularies.Validation201909Id)]
	[JsonConverter(typeof(MaxLengthKeywordJsonConverter))]
	public class MaxLengthKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "maxLength";

		public uint Value { get; }

		public MaxLengthKeyword(uint value)
		{
			Value = value;
		}

		public void Validate(ValidationContext context)
		{
			if (context.LocalInstance.ValueKind != JsonValueKind.String)
			{
				context.IsValid = true;
				return;
			}

			var length = new StringInfo(context.LocalInstance.GetString()).LengthInTextElements;
			context.IsValid = Value >= length;
			if (!context.IsValid)
				context.Message = $"Value is not shorter than or equal to {Value} characters";
		}
	}

	internal class MaxLengthKeywordJsonConverter : JsonConverter<MaxLengthKeyword>
	{
		public override MaxLengthKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException("Expected number");

			var number = reader.GetUInt32();

			return new MaxLengthKeyword(number);
		}
		public override void Write(Utf8JsonWriter writer, MaxLengthKeyword value, JsonSerializerOptions options)
		{
			writer.WriteNumber(MaxLengthKeyword.Name, value.Value);
		}
	}
}