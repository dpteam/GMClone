﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameCreator.Interpreter
{
    public class Stmt : Node
    {
        protected FlowType ProgramFlow;
        // Nothing should call run(), which is called internally be Stmt.
        // This is the function you override. The statement changes program flow with the variable ProgramFlow.
        protected virtual void run()
        {
        }
        // This is the user function to execute a statement. It should not be called by statements themselves.
        public FlowType Exec()
        {
            ProgramFlow = FlowType.None;
            run();
            return ProgramFlow;
        }
        // This function is called by non-loop statements with embedded statements. The calling statement must
        // return if Exec(s) != FlowType.None.
        protected FlowType Exec(Stmt s)
        {
            FlowType t = s.Exec();
            ProgramFlow |= t;
            return t;
        }
        // This function is called by loop statements to execute embedded statements. You can catch
        // program flow statements, to keep them from falling through, like this:
        /* switch (Exec(s, FlowType.Break|FlowType.Continue))
         * {
         * case FlowType.Break:
         *     goto End;
         * case FlowType.Continue:
         *     goto Test;
         * default:
         *     return;
         * }
         */
        protected FlowType Exec(Stmt s, FlowType Catch)
        {
            FlowType t = s.Exec();
            ProgramFlow |= t & ~Catch;
            return t;
        }

        public static Stmt Null = new Stmt();
    }
}
