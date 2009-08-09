﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameCreator.Interpreter
{
    class Switch : Stmt
    {
        Expr expr;
        Stmt[] stmts;
        public Switch(Expr x, Stmt[] y)
        {
            expr = x;
            stmts = y;
        }
        protected override void run()
        {
            bool met = false;
            Value v1 = expr.Eval(), v2;
            foreach (Stmt s in stmts)
            {
                if (s.GetType() == typeof(Default))
                    met = true;
                else if (s.GetType() == typeof(Case))
                {
                    v2 = ((Case)s).Eval();
                    if ((v1.IsReal && v2.IsReal && v1.Real == v2.Real) ||
                        (v1.IsString && v2.IsString && v1.String == v2.String))
                        met = true;
                }
                else if (met)
                {
                    if (Exec(s, FlowType.Break) != FlowType.None) return;
                }
            }
        }
    }
}
