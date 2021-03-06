﻿using System.Text.Json;

namespace Json.Path.QueryExpressions
{
	internal interface IQueryExpressionOperator
	{
		int OrderOfOperation { get; }

		QueryExpressionType GetOutputType(QueryExpressionNode left, QueryExpressionNode right);
		JsonElement Evaluate(QueryExpressionNode left, QueryExpressionNode right, JsonElement element);
		string ToString(QueryExpressionNode left, QueryExpressionNode right);
	}
}