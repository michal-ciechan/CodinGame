using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace CodersStrikeBack
{
    public static class Logger
    {
        public static Action<String> WriteAction = 
            s => Console.Error.WriteLine(s);

        public static void Write<T>(string prefix, Expression<Func<T>> expr)
        {
            var name = GetName(expr);
            var value = expr.Compile()();

            WriteAction($"{prefix}: {name} = {value}");
        }

        public static void Write<T>(Expression<Func<T>> expr)
        {
            Write("", expr);
        }

        public static void Write(string msg)
        {
            WriteAction(msg);
        }

        private static string GetName<T>(Expression<Func<T>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body != null)
                return PropertyNameFromMemberExpr(body);

            throw new NotImplementedException();
        }

        private static string PropertyNameFromMemberExpr(MemberExpression expr)
        {
            return expr.Member.Name;
        }
    }
}