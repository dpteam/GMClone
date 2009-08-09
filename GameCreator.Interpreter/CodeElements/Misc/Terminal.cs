﻿namespace GameCreator.Interpreter
{
    public enum Terminal
    {
        None,
        Eof,
        Unknown,
        Identifier,
        Real,
        StringLiteral,
        Break,
        Continue,
        If,
        Then,
        Else,
        While,
        Do,
        Until,
        For,
        BitwiseComplement,
        Not,
        Inequality,
        Mod,
        BitwiseXor,
        LogicalXor,
        XorAssignment,
        BitwiseAnd,
        LogicalAnd,
        AndAssignment,
        Multiply,
        MultiplyAssignment,
        OpeningParenthesis,
        ClosingParenthesis,
        Minus,
        SubtractionAssignment,
        Plus,
        AdditionAssignment,
        Assignment,
        Equality,
        OpeningCurlyBrace,
        ClosingCurlyBrace,
        OpeningSquareBracket,
        ClosingSquareBracket,
        BitwiseOr,
        LogicalOr,
        OrAssignment,
        Colon,
        Semicolon,
        LessThan,
        LessThanOrEqual,
        ShiftLeft,
        ShiftRight,
        GreaterThan,
        GreaterThanOrEqual,
        Comma,
        Dot,
        Divide,
        DivideAssignment,
        Div,
        Var,
        Globalvar,
        Repeat,
        Switch,
        Case,
        Default,
        Exit,
        With,
        Return,
    }
}