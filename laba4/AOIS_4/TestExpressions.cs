using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_4
{
    internal class TestExpressions
    {
        static string[] testExpressions =
        {
            "((x1+x2)*x3*(x4+x2))",
            "!(((!x1)+(!x2))*(!((!x2)*(!x3))))",
            "(p*(q+r))",
            "((P==Q)==R)",
            "(!(P->(P->(Q==Q))))",
            "((P->Q)+(P->(Q*P)))",
            "((P+(Q*(!R)))->(R==Q))",
            "(((!P)->(Q*R))==((!(!(P+Q)))->S))",
            "((P->Q)->((P->(Q->R))->(P->R)))",
            "(!((S->((!R)+(P*Q)))==(P*(!(Q->R)))))",
            "((((P->R)*(Q->S))*((!P)+(!S)))->((!P)+(!Q)))"
        };

        public static string[] GetTestExpressions()
        {
            return testExpressions;
        }
    }
}