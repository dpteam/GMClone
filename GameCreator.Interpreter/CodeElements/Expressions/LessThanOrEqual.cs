﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameCreator.Interpreter
{
    class LessThanOrEqual : Expr
    {
        Expr expr1, expr2;
        public LessThanOrEqual(Expr e1, Expr e2) { expr1 = e1; expr2 = e2; }
        public override Value Eval()
        {
            Value v1 = expr1.Eval(), v2 = expr2.Eval();
            if (v1.IsReal && v2.IsReal)
            {
                return v1.Real <= v2.Real ? Value.One : Value.Zero;
            }
            else if (v1.IsString && v2.IsString)
            {
                return String.CompareOrdinal(v1.String, v2.String) <= 0 ? Value.One : Value.Zero;
            }
            else throw new ProgramError("Cannot compare arguments.");
        }
    }
}
