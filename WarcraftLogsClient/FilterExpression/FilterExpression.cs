using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace WarcraftLogs;

/// <summary>
/// Creates a string from a linq expression in the WarcraftLogs language. 
/// Example:
/// FilterExpression.Where(x => (x.AbilityId == 10 || x.AbilityId == 20) && x.Type == Type.cast).AndWhere(x => x.AbilityId == 10);
/// Creates a string:
/// ((ability.id = 10 OR ability.id = 20) AND (type = 'cast')) AND (ability.id = 10)
/// </summary>
public class FilterExpression
{
  private FilterExpression() { }
  private FilterExpression(FilterExpression expression) => Builder.Append(expression.ToString());
  private StringBuilder Builder { get; set; } = new StringBuilder();
  public override string ToString() { return Builder.ToString(); }
  public static implicit operator string(FilterExpression expression) { return expression.ToString(); }

  private static readonly Dictionary<ExpressionType, string> ExpressionTypeToString = new()
  {
    { ExpressionType.And, " AND " },
    { ExpressionType.AndAlso, " AND " },
    { ExpressionType.Or, " OR " },
    { ExpressionType.OrElse, " OR " },
    { ExpressionType.Not, " NOT " },
    { ExpressionType.Equal, " = " },
    { ExpressionType.NotEqual, " != " },
    { ExpressionType.GreaterThan, " > " },
    { ExpressionType.GreaterThanOrEqual, " >= " },
    { ExpressionType.LessThan, " < " },
    { ExpressionType.LessThanOrEqual, " <= " },
    { ExpressionType.Add, " + "},
    { ExpressionType.Subtract, " - "},
    { ExpressionType.Multiply, " * " },
    { ExpressionType.Divide, " / " },
    { ExpressionType.Modulo, " % " }
  };

  #region Public methods
  
  public static FilterExpression Where(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var newExpression = new FilterExpression();
    return newExpression.AppendExpression(filterExpression.Body);
  }
  public FilterExpression AndWhere(FilterExpression filterExpression)
  {
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.And)!;
    return JoinExpression(filterExpression, operatorString);
  }
  
  public FilterExpression AndWhere(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.And)!;

    return JoinExpression(filterExpression, operatorString);
  }
  public FilterExpression OrWhere(FilterExpression filterExpression)
  {
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.Or)!;
    
    return JoinExpression(filterExpression, operatorString);
  }
  
  public FilterExpression OrWhere(Expression<Func<FilterParameters, bool>> filterExpression)
  {
    var operatorString = ExpressionTypeToString.GetValueOrDefault(ExpressionType.Or)!;
    
    return JoinExpression(filterExpression, operatorString);
  }

  #endregion
  
  #region Private methods
  
  private FilterExpression AppendExpression(Expression node)
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
  
  #region Binary Expression

  private FilterExpression ConstructExpressionString(BinaryExpression binaryExpression)
  {
    // Build left expression
    if (IsLeftExpressionNeedsParenthesis(binaryExpression))
    {
      Builder.Append('(');
      AppendExpression(binaryExpression.Left);
      Builder.Append(')');
    }
    else
    {
      AppendExpression(binaryExpression.Left);
    }
    
    // Build operator
    var operatorString = ExpressionTypeToString.GetValueOrDefault(binaryExpression.NodeType) ??
                         throw new NotSupportedException($"Not support node type {binaryExpression.NodeType}");
    Builder.Append(operatorString);
    
    // Build right expression
    if (binaryExpression.Left is UnaryExpression exL && IsEnumConvert(exL) && binaryExpression.Right is ConstantExpression { Value: not null } exR)
    {
      Builder.Append($"'{Enum.GetName(exL.Operand.Type, exR.Value)}'");
    }
    else if (IsRightExpressionNeedsParenthesis(binaryExpression))
    {
      Builder.Append('(');
      AppendExpression(binaryExpression.Right);
      Builder.Append(')');
    }
    else
    {
      AppendExpression(binaryExpression.Right);
    }

    return this;
  }

  private static bool IsLeftExpressionNeedsParenthesis(BinaryExpression expression)
  {
    return expression.Left is BinaryExpression leftBinaryExpression &&
           expression.NodeType < leftBinaryExpression.NodeType;
  }
  
  private static bool IsRightExpressionNeedsParenthesis(BinaryExpression expression)
  {
    return expression.Right is BinaryExpression rightBinaryExpression &&
           expression.NodeType < rightBinaryExpression.NodeType;
  }
  
  #endregion
  
  #region Unary Expression
  
  private FilterExpression ConstructExpressionString(UnaryExpression unaryExpression)
  {
    if (unaryExpression.NodeType == ExpressionType.Convert)
    {
      AppendExpression(unaryExpression.Operand);
      return this;
    }
    var operatorString = ExpressionTypeToString.GetValueOrDefault(unaryExpression.NodeType);
    if (operatorString == null) throw new NotSupportedException($"Not support node type {unaryExpression.NodeType}");

    Builder.Append($"{operatorString}");
    AppendExpression(unaryExpression.Operand);

    return this;
  }
  
  private static bool IsEnumConvert(UnaryExpression expression)
  {
    return expression.NodeType is ExpressionType.Convert && expression.Operand.Type.IsEnum;
  }

  #endregion
  
  #region Member Expression

  private FilterExpression ConstructExpressionString(MemberExpression memberExpression)
  {
    var member = memberExpression.Member;
    var attribute = member.GetCustomAttribute<FilterParameterNameAttribute>();

    if (attribute == null)
    {
      var objectMember = Expression.Convert(memberExpression, typeof(object));
      var getterLambda = Expression.Lambda<Func<object>>(objectMember);
      var getter = getterLambda.Compile();
      var value = getter();
      Builder.Append(value);
    }
    else
    {
      Builder.Append(attribute.Name);
    }

    return this;
  }

  #endregion
  
  #region Constant Expression
  private FilterExpression ConstructExpressionString(ConstantExpression constantExpression)
  {
    if (constantExpression.Type == typeof(string) || constantExpression.Type.IsEnum)
    {
      Builder.Append($"'{constantExpression.Value}'");
    }
    else
    {
      Builder.Append(constantExpression.Value);
    }

    return this;
  }
  
  #endregion
  
  #region Common

  private FilterExpression JoinExpression(Expression<Func<FilterParameters, bool>> filterExpression, string separator)
  {
    var rightExpression = Where(filterExpression);
    Builder.Insert(0, '(');
    Builder.Append($"){separator}({rightExpression.ToString()})"); // ?? )
    return this;
  }

  private FilterExpression JoinExpression(FilterExpression filterExpression, string separator)
  {
    Builder.Insert(0, '(');
    Builder.Append($"){separator}({filterExpression.ToString()})"); // ?? )
    return this;
  }
  #endregion
  
  


  #endregion
}