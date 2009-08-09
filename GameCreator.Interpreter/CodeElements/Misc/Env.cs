﻿using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;

namespace GameCreator.Interpreter
{
	// Get accessor delegate for variables
    public delegate void SetAccessor(int i1, int i2, Value val);
    // Set accessor delegate for variables
    public delegate Value GetAccessor(int i1, int i2);
    public class Env
    {
        static long ids = 100000;
        public static Dictionary<long, Env> Instances = new Dictionary<long, Env>();
        public static Env Current = null;
        public static Env Other = null;
        static Dictionary<string, Variable> globals = new Dictionary<string, Variable>();
        static List<string> globalvars = new List<string>();
        static List<string> localvars;
        public static List<string> Builtin = new List<string>();
        static Stack<List<string>> varstack = new Stack<List<string>>();
        static Stack<Dictionary<string, Variable>> localstack = new Stack<Dictionary<string, Variable>>();
        static Dictionary<string, BaseFunction> functions = new Dictionary<string, BaseFunction>();
        static Dictionary<string, Variable> locals;// = new Dictionary<string, Variable>();
        Dictionary<string, Variable> instancevars = new Dictionary<string, Variable>();
        static Stack<Value[]> argstack = new Stack<Value[]>();
        static Value[] args = new Value[0];
        public static string Title = "game";
		public static Dictionary<string, BaseFunction> Functions{ get { return functions; } }
		public delegate void ErrorMessage(string msg);
		public static event ErrorMessage Error;
        public const long self = -1, other = -2, all = -3, noone = -4, global = -5;
        // Holds the value of the last returned value. This mimics GM's behavior. If a script does not
        // return a value, it automatically returns the value returned from the last call
        // (Env.Returned is not changed). Upon entry of a script, Env.Return is set to Value(0.d).
        // In other words, a script containing "func0()" is the same as "return func0()", but an empty
        // script returns 0. Also, when a string is executed, i.e. with execute_string(),
        // it has the same behavior as a script (Env.Return -> 0, if return statement is encountered,
        // returns control from string, not script).
        public static Value Returned;
        // private; use CreateInstance() or CreatePrivateInstance().
        Env()
        {
            NewReadOnly("object_index");
            NewReadOnly("id");
        }
		public static void DefineFunctionsFromType(Type t)
		{
			// Build the list of functions
            foreach (System.Reflection.MethodInfo mi in t.GetMethods())
            {
                if (!mi.IsStatic) continue;
                GMLFunctionAttribute[] attrs = (GMLFunctionAttribute[])mi.GetCustomAttributes(typeof(GMLFunctionAttribute), false);
                if (attrs.Length != 1) continue;
				GMLFunctionAttribute fn = attrs[0];
                string name = string.IsNullOrEmpty(fn.Name) ? mi.Name : fn.Name;
                DefineFunction(name, fn.Argc, (FunctionDelegate)System.Delegate.CreateDelegate(typeof(FunctionDelegate), mi));
            }
		}
        static Env()
        {

            // builtins
            DefineVar("current_time", current_time);
            DefineVar("argument", get_argument, set_argument);
            DefineVar("argument0", get_argument0, set_argument0);
            DefineVar("argument1", get_argument1, set_argument1);
            DefineVar("argument2", get_argument2, set_argument2);
            DefineVar("argument3", get_argument3, set_argument3);
            DefineVar("argument4", get_argument4, set_argument4);
            DefineVar("argument5", get_argument5, set_argument5);
            DefineVar("argument6", get_argument6, set_argument6);
            DefineVar("argument7", get_argument7, set_argument7);
            DefineVar("argument8", get_argument8, set_argument8);
            DefineVar("argument9", get_argument9, set_argument9);
            DefineVar("argument10", get_argument10, set_argument10);
            DefineVar("argument11", get_argument11, set_argument11);
            DefineVar("argument12", get_argument12, set_argument12);
            DefineVar("argument13", get_argument13, set_argument13);
            DefineVar("argument14", get_argument14, set_argument14);
            DefineVar("argument15", get_argument15, set_argument15);
            Builtin.Add("object_index");
            Builtin.Add("id");
        }
        #region Argument access methods
        static Value get_argument(int i1, int i2)
        {
            if (i2 >= 16 || i2 < 0) return Value.Zero;
            return args[i2];
        }
        static void set_argument(int i1, int i2, Value v)
        {
            if (i2 >= 16 || i2 < 0) return;
            args[i2] = v;
        }
        static Value get_argument0(int i1, int i2)
        {
            return args[0];
        }
        static void set_argument0(int i1, int i2, Value v)
        {
            args[0] = v;
        }
        static Value get_argument1(int i1, int i2)
        {
            return args[1];
        }
        static void set_argument1(int i1, int i2, Value v)
        {
            args[1] = v;
        }
        static Value get_argument2(int i1, int i2)
        {
            return args[2];
        }
        static void set_argument2(int i1, int i2, Value v)
        {
            args[2] = v;
        }
        static Value get_argument3(int i1, int i2)
        {
            return args[3];
        }
        static void set_argument3(int i1, int i2, Value v)
        {
            args[3] = v;
        }
        static Value get_argument4(int i1, int i2)
        {
            return args[4];
        }
        static void set_argument4(int i1, int i2, Value v)
        {
            args[4] = v;
        }
        static Value get_argument5(int i1, int i2)
        {
            return args[5];
        }
        static void set_argument5(int i1, int i2, Value v)
        {
            args[5] = v;
        }
        static Value get_argument6(int i1, int i2)
        {
            return args[6];
        }
        static void set_argument6(int i1, int i2, Value v)
        {
            args[6] = v;
        }
        static Value get_argument7(int i1, int i2)
        {
            return args[7];
        }
        static void set_argument7(int i1, int i2, Value v)
        {
            args[7] = v;
        }
        static Value get_argument8(int i1, int i2)
        {
            return args[8];
        }
        static void set_argument8(int i1, int i2, Value v)
        {
            args[8] = v;
        }
        static Value get_argument9(int i1, int i2)
        {
            return args[9];
        }
        static void set_argument9(int i1, int i2, Value v)
        {
            args[9] = v;
        }
        static Value get_argument10(int i1, int i2)
        {
            return args[10];
        }
        static void set_argument10(int i1, int i2, Value v)
        {
            args[10] = v;
        }
        static Value get_argument11(int i1, int i2)
        {
            return args[11];
        }
        static void set_argument11(int i1, int i2, Value v)
        {
            args[11] = v;
        }
        static Value get_argument12(int i1, int i2)
        {
            return args[12];
        }
        static void set_argument12(int i1, int i2, Value v)
        {
            args[12] = v;
        }
        static Value get_argument13(int i1, int i2)
        {
            return args[13];
        }
        static void set_argument13(int i1, int i2, Value v)
        {
            args[13] = v;
        }
        static Value get_argument14(int i1, int i2)
        {
            return args[14];
        }
        static void set_argument14(int i1, int i2, Value v)
        {
            args[14] = v;
        }
        static Value get_argument15(int i1, int i2)
        {
            return args[15];
        }
        static void set_argument15(int i1, int i2, Value v)
        {
            args[15] = v;
        }
        #endregion
		#region Builtin Variables
		/*
         * Built-in variables
         */
        public static Value current_time(int i1, int i2)
        {
            return new Value((double)Environment.TickCount);
        }
		#endregion
        static void DefineVar(string n, GetAccessor f)
        {
            Builtin.Add(n);
            globals.Add(n, new Variable(f));
            globalvars.Add(n);
        }
        static void DefineVar(string n, GetAccessor g, SetAccessor s)
        {
            Builtin.Add(n);
            globals.Add(n, new Variable(g, s));
            globalvars.Add(n);
        }
        public static void DefineFunction(string n, int argc, FunctionDelegate f)
        {
            if (functions.ContainsKey(n)) return;
            functions.Add(n, new Function(n, argc, f));
        }
        public static bool FunctionExists(string n)
        {
            return functions.ContainsKey(n);
        }
        public static Value ExecuteFunction(string n, params Value[] args)
        {
            Returned = functions[n].Execute(args);
            return Returned;
        }
        public static BaseFunction GetFunction(string n)
        {
            return functions[n];
        }
        public static void Enter()
        {
            varstack.Push(localvars);
            localstack.Push(locals);
            argstack.Push(args);
            localvars = new List<string>();
            locals = new Dictionary<string, Variable>();
            args = new Value[] {Value.Zero, Value.Zero, Value.Zero, Value.Zero,
                                Value.Zero, Value.Zero, Value.Zero, Value.Zero,
                                Value.Zero, Value.Zero, Value.Zero, Value.Zero,
                                Value.Zero, Value.Zero, Value.Zero, Value.Zero};
            Returned = Value.Zero;
        }
        public static void Leave()
        {
            localvars = varstack.Pop();
            locals = localstack.Pop();
            args = argstack.Pop();
        }
        public static void SetArguments(Value[] args)
        {
            for (int i = 0; i < 16 && i < args.Length; i++)
                Env.args[i] = args[i];
        }
        public static void DefineScript(string n, string c)
        {
            if (functions.ContainsKey(n)) return;
            functions.Add(n, new Script(n, c));
        }
		/*
        public static void RunProgram(System.IO.Stream s)
        {
            try
            {
                try
                {
                    Env t = Env.Current;
                    Env.Current = Env.CreatePrivateInstance(); // The current instance executing the code
                    ImportScripts(s);
                    if (!FunctionExists("main")) throw new ProgramError("Entry point not found.");
                    CompileScripts();
                    Enter();
                    try
                    {
                        functions["main"].Execute();
                    }
                    catch (System.OutOfMemoryException)
                    {
                        throw;
                    }
                    finally
                    {
                        Leave();
                        Env.Current = t;
                    }
                }
                catch (ProgramError p)
                {
                    System.Windows.Forms.MessageBox.Show(p.Message, Env.Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.OutOfMemoryException)
            {
                System.Windows.Forms.MessageBox.Show("Unexpected error occurred when running the game.");
                System.Environment.Exit(1);
            }

        }
        */
        public static void ImportScripts(string fname)
        {
            ImportScripts(System.IO.File.Open(fname, System.IO.FileMode.Open));
        }
        public static void ImportScripts(System.IO.Stream s)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(s);
            string scr = null;
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Trim().StartsWith("#define "))
                {
                    if (!string.IsNullOrEmpty(scr))
                    {
                        DefineScript(scr, sb.ToString());
                    }
                    scr = line.Substring(line.IndexOf("#define ") + 8).Trim();
                    sb.Remove(0, sb.Length);
                }
                else if (!string.IsNullOrEmpty(scr))
                {
                    sb.AppendLine(line);
                }
            }
            if (!string.IsNullOrEmpty(scr))
            {
                DefineScript(scr, sb.ToString());
            }
        }
        public static void CompileScripts()
        {
            foreach (BaseFunction f in functions.Values)
            {
                if (f.GetType() == typeof(Script))
                {
                    ((Script)f).Compile();
                }
            }
        }
        private void NewReadOnly(string p)
        {
            instancevars.Add(p, new Variable());
            instancevars[p].IsReadOnly = true;
        }
        void assign_id()
        {
            long id = ++ids;
            Instances.Add(id, this);
            instancevars["id"].Value = new Value((double)id);
        }
        public static Env CreateInstance()
        {
            Env e = new Env();
            e.assign_id();
            return e;
        }
        // Used for: Room scripts, etc.
        public static Env CreatePrivateInstance()
        {
            return new Env();
        }
        public void Exec(string s)
        {
            Env t = Current;
            Current = this;
            Env.Enter();
            Parser.Execute(s);
            Env.Leave();
            Current = t;
        }
        // returns whether the name exists as a variable, in the scope of the current instance.
        // The interpreter checks array bounds with Variable.CheckIndex().
        public static bool VariableExists(string name)
        {
            return (locals.ContainsKey(name) || globalvars.Contains(name) || Current.instancevars.ContainsKey(name));
        }
        // returns whether the name exists as a variable, in the scope of the given instance.
        // example: x = VariableExists(Env.self, "t");
        public static bool VariableExists(long id, string name)
        {
            switch (id)
            {
                case self:
                    return (Current != null && Current.instancevars.ContainsKey(name));
                case other:
                    return (Other != null && Other.instancevars.ContainsKey(name));
                case all:
                    foreach (Env e in Instances.Values)
                    {
                        if (e.instancevars.ContainsKey(name)) return true;
                    }
                    return false;
                case noone:
                    return false;
                case global:
                    return globals.ContainsKey(name);
                default:
                    return (Instances.ContainsKey(id) && Instances[id].instancevars.ContainsKey(name));
            }
        }
        public static void SetVar(string name, int i1, int i2, Value val)
        {
            if (localvars.Contains(name))
            {
                if (locals.ContainsKey(name))
                {
                    if (locals[name].IsReadOnly) throw new ProgramError("Cannot assign to the variable");
                    locals[name][i1, i2] = val;
                }
                else
                    locals.Add(name, new Variable(i1, i2, val));
            }
            else if (globalvars.Contains(name))
            {
                if (globals[name].IsReadOnly) throw new ProgramError("Cannot assign to the variable");
                globals[name][i1, i2] = val;
            }
            else if (Current != null)
            {
                if (Current.instancevars.ContainsKey(name))
                {
                    if (Current.instancevars[name].IsReadOnly) throw new ProgramError("Cannot assign to the variable");
                    Current.instancevars[name][i1, i2] = val;
                }
                else
                    Current.instancevars.Add(name, new Variable(i1, i2, val));
            }
            
        }
        public static void SetVar(string name, int index, Value val)
        {
            SetVar(name, 0, index, val);
        }
        public static void SetVar(string name, Value val)
        {
            SetVar(name, 0, 0, val);
        }
        public void SetLocalVar(string name, Value val)
        {
            if (instancevars.ContainsKey(name))
                instancevars[name].Value = val;
            else
                instancevars.Add(name, new Variable(val));
        }
        public void SetLocalVar(string name, int index, Value val)
        {
            if (instancevars.ContainsKey(name))
                instancevars[name][index] = val;
            else
                instancevars.Add(name, new Variable(index, val));
        }
        public void SetLocalVar(string name, int i1, int i2, Value val)
        {
            if (instancevars.ContainsKey(name))
                instancevars[name][i1, i2] = val;
            else
                instancevars.Add(name, new Variable(i1, i2, val));
        }
        public Value GetLocalVar(string name)
        {
            return instancevars[name].Value;
        }
        public Value GetLocalVar(string name, int index)
        {
            return instancevars[name][index];
        }
        public Value GetLocalVar(string name, int i1, int i2)
        {
            return instancevars[name][i1, i2];
        }
        public static void SetVar(long instance, string name, int i1, int i2, Value val)
        {
            Dictionary<string, Variable> vars;
            switch (instance)
            {
                case self:
                    vars = Current.instancevars;
                    break;
                case other:
                    if (Other == null) throw new ProgramError("Cannot assign to the variable");
                    vars = Other.instancevars;
                    break;
                case all:
                    foreach (long l in Instances.Keys)
                    {
                        SetVar(l, name, i1, i2, val);
                    }
                    return;
                case noone:
                    throw new ProgramError("Cannot assign to the variable");
                case global:
                    vars = globals;
                    break;
                default:
                    if (Instances.ContainsKey(instance))
                        vars = Instances[instance].instancevars;
                    else if (instance < global)
                        throw new ProgramError("Cannot assign to the variable");
                    else
                    {
                        foreach (Env e in Instances.Values)
                        {
                            if ((long)e.instancevars["object_index"].Value.Real == instance)
                            {
                                //SetVar((long)e.instancevars["id"].Value.Real, name, i1, i2, val);
                                vars = e.instancevars;
                                if (vars.ContainsKey(name))
                                    vars[name][i1, i2] = val;
                                else
                                    vars.Add(name, new Variable(i1, i2, val));
                            }
                        }
                        return;
                    }
                    break;
            }
            if (vars.ContainsKey(name))
                vars[name][i1, i2] = val;
            else
                vars.Add(name, new Variable(i1, i2, val));
        }
        public static void SetVar(long instance, string name, int index, Value val)
        {
            SetVar(instance, name, 0, index, val);
        }
        public static void SetVar(long instance, string name, Value val)
        {
            SetVar(instance, name, 0, 0, val);
        }
        public static Value GetVar(string name, int i1, int i2)
        {
            if (localvars.Contains(name))
                return locals[name][i1, i2];
            if (globalvars.Contains(name))
                return globals[name][i1, i2];
            return Current.instancevars[name][i1, i2];
        }
        public static Value GetVar(string name, int index)
        {
            return GetVar(name, 0, index);
        }
        public static Value GetVar(string name)
        {
            return GetVar(name, 0, 0);
        }
        public static Value GetVar(long instance, string name, int i1, int i2)
        {
            switch (instance)
            {
                case self:
                    return Current.instancevars[name][i1, i2];
                case other:
                    return Other.instancevars[name][i1, i2];
                case all:
                    foreach (Env e in Instances.Values)
                    {
                        if (e.instancevars.ContainsKey(name))
                            return e.instancevars[name][i1, i2];
                    }
                    return Value.Zero;
                case noone:
                    return Value.Zero;
                case global:
                    return globals[name][i1, i2];
                default:
                    return Instances[instance].instancevars[name][i1, i2];
            }
        }
        public static Value GetVar(long instance, string name, int index)
        {
            return GetVar(instance, name, 0, index);
        }
        public static Value GetVar(long instance, string name)
        {
            return GetVar(instance, name, 0, 0);
        }
        public static Variable Variable(string name)
        {
            if (localvars.Contains(name))
                return locals[name];
            if (globalvars.Contains(name))
                return globals[name];
            return Current.instancevars[name];
        }
        public static Variable Variable(long instance, string name)
        {
            switch (instance)
            {
                case self:
                    return Current.instancevars[name];
                case other:
                    return Other.instancevars[name];
                case all:
                    foreach (Env e in Instances.Values)
                    {
                        if (e.instancevars.ContainsKey(name))
                            return e.instancevars[name];
                    }
                    return null;
                case noone:
                    return null;
                case global:
                    return globals[name];
                default:
                    return Instances[instance].instancevars[name];
            }
        }
        public static void GlobalVars(string[] v)
        {
            foreach (string s in v)
            {
                if (!globalvars.Contains(s))
                {
                    globalvars.Add(s);
                    globals.Add(s, new Variable());
                }
            }
        }
        public static void LocalVars(string[] v)
        {
            foreach (string s in v)
            {
                if (!localvars.Contains(s))
                    localvars.Add(s);
            }
        }
    }
}
