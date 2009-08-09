﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameCreator.Interpreter
{
    class Repeat : Stmt
    {
        public Expr expr;
        public Stmt stmt;
        public Repeat(Expr e, Stmt s)
        {
            expr = e;
            stmt = s;
        }
        protected override void run()
        {
            Value v = expr.Eval();
            if (!v.IsReal) throw new ProgramError("Repeat count must be a number");
            int times = (int)Math.Round(v.Real);
            while (times > 0)
            {
                if ((Exec(stmt, FlowType.Continue | FlowType.Break) & ~FlowType.Continue) != FlowType.None) return;
                times--;
            }
        }
        public override string ToString()
        {
            return string.Format("repeat {0} {1}", expr, stmt);
        }
    }
}
