using System.Linq.Expressions;
using System.Reflection;

namespace WarcraftLogsAnalyzer.Query;

/// <summary>
/// Creates a string from a linq expression in the warcraftlogs language. 
/// Example:
/// FilterExpression.Where(x => (x.AbilityId == 10 || x.AbilityId == 20) && x.Type == Type.cast).Where(x => x.AbilityId == 10);
/// Creates a string:
/// ((ability.id = 10 OR ability.id = 20) AND (type = "cast")) AND (ability.id = 10)
/// </summary>

public class FilterExpression
{
  private FilterExpression() { }
  private FilterExpression(string expression) { Expression = expression; }
  private string Expression { get; set; } = string.Empty;

  public override string ToString() { return Expression; }
  public static implicit operator string(FilterExpression expression) { return expression.Expression; }

  private static readonly Dictionary<ExpressionType, string> ExpressionTypeToString = new()
  {
    { ExpressionType.And, "AND" },
    { ExpressionType.AndAlso, "AND" },
    { ExpressionType.Or, "OR" },
    { ExpressionType.OrElse, "OR" },
    { ExpressionType.Not, "NOT" },
    { ExpressionType.Equal, "=" },
    { ExpressionType.NotEqual, "!=" },
    { ExpressionType.GreaterThan, ">" },
    { ExpressionType.GreaterThanOrEqual, ">=" },
    { ExpressionType.LessThan, "<" },
    { ExpressionType.LessThanOrEqual, "<=" },
    { ExpressionType.Add, "+"},
    { ExpressionType.Subtract, "-"},
    { ExpressionType.Multiply, "*" },
    { ExpressionType.Divide, "/" },
    { ExpressionType.Modulo, "%" }
  };

  public static FilterExpression AnyOf<T>(Expression<Func<FilterParameters, T>> expr, IEnumerable<T> ids)
  {
    if (ids == null || !ids.Any()) return new FilterExpression();

    IEnumerable<Expression> equalExpressions = ids.Select(id =>
    {
      var value = System.Linq.Expressions.Expression.Constant(id, typeof(T));
      return System.Linq.Expressions.Expression.Equal(expr.Body, value) as Expression;
    });

    Expression<Func<FilterParameters, bool>>? combinedExpression = null;

    foreach (var equalExpression in equalExpressions)
    {
      if (combinedExpression == null)
      {
        combinedExpression = System.Linq.Expressions.Expression.Lambda<Func<FilterParameters, bool>>(equalExpression, expr.Parameters);
      }
      else
      {
        var orElseExpression = System.Linq.Expressions.Expression.OrElse(combinedExpression.Body, equalExpression);
        combinedExpression = System.Linq.Expressions.Expression.Lambda<Func<FilterParameters, bool>>(orElseExpression, expr.Parameters);
      }
    }

    return Where(combinedExpression!);
  }

  public static FilterExpression Where(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var expression = filterExpression.Body;
    return new FilterExpression(ExpressionToString(expression));
  }

  public FilterExpression AndWhere(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var rightExpt = Where(filterExpression);
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.And);

    Expression = $"({Expression}) {operatorString} ({rightExpt})";
    return this;
  }

  public FilterExpression OrWhere(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var rightExpt = Where(filterExpression);
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.Or);

    Expression = $"({Expression}) {operatorString} ({rightExpt})";
    return this;
  }

  private static string ExpressionToString(Expression node)
  {
    return node switch
    {
      BinaryExpression e => ConstructExpressionString(e),
      MemberExpression e => ConstructExpressionString(e),
      ConstantExpression e => ConstructExpressionString(e),
      UnaryExpression e => ConstructExpressionString(e),
      _ => throw new NotSupportedException($"Not support {node} type")
    };
  }

  private static string ConstructExpressionString(BinaryExpression binaryExpression)
  {
    var operatorString = ExpressionTypeToString.GetValueOrDefault(binaryExpression.NodeType);
    if (operatorString == null) throw new NotSupportedException($"Not support node type {binaryExpression.NodeType}");

    string leftExpression = ExpressionToString(binaryExpression.Left);
    string rightExpression = string.Empty;

    if (binaryExpression.Left is UnaryExpression exL && IsEnumConvert(exL) && binaryExpression.Right is ConstantExpression exR && exR.Value != null)
    {
      rightExpression = $"'{Enum.GetName(exL.Operand.Type, exR.Value)}'";
    }
    else
    {
      rightExpression = ExpressionToString(binaryExpression.Right);
    }

    CheckPriorityOperation(binaryExpression, ref leftExpression, ref rightExpression);

    return $"{leftExpression} {operatorString} {rightExpression}";
  }
  private static bool IsEnumConvert(UnaryExpression expression)
  {
    return expression.NodeType == ExpressionType.Convert && expression.Operand.Type.IsEnum;
  }

  private static void CheckPriorityOperation(BinaryExpression binaryExpression, ref string leftExpression, ref string rightExpression)
  {
    if (binaryExpression.Left is BinaryExpression leftBinaryExpression &&
        binaryExpression.NodeType < leftBinaryExpression.NodeType)
    {
      leftExpression = $"({leftExpression})";
    }

    if (binaryExpression.Right is BinaryExpression rightBinaryExpression &&
        binaryExpression.NodeType < rightBinaryExpression.NodeType)
    {
      rightExpression = $"({rightExpression})";
    }
  }

  private static string ConstructExpressionString(MemberExpression memberExpression)
  {
    var member = memberExpression.Member;
    var attribute = member.GetCustomAttribute<FilterParameterNameAttribute>();

    if (attribute == null) return nameof(member);
    return attribute.Name;
  }

  private static string ConstructExpressionString(ConstantExpression constantExpression)
  {
    if (constantExpression.Type == typeof(string))
    {
      return $"'{constantExpression.Value}'";
    }
    if (constantExpression.Type.IsEnum)
    {
      return $"'{constantExpression.Value}'";
    }
    return constantExpression.Value?.ToString() ?? throw new Exception("No value");
  }

  private static string ConstructExpressionString(UnaryExpression unaryExpression)
  {
    if (unaryExpression.NodeType == ExpressionType.Convert)
    {
      return ExpressionToString(unaryExpression.Operand);
    }
    var operatorString = ExpressionTypeToString.GetValueOrDefault(unaryExpression.NodeType);
    if (operatorString == null) throw new NotSupportedException($"Not support node type {unaryExpression.NodeType}");

    return $"{operatorString} ({ExpressionToString(unaryExpression.Operand)})";
  }
}