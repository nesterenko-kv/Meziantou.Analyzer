﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Meziantou.Analyzer.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new StringComparisonAnalyzer();
        }

        //No diagnostics expected to show up
        [TestMethod]
        public void StringComparisonIsMissing_ShouldNotReportDiagnosticForEmptyString()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void StringComparisonIsMissing_ShouldReportDiagnostic()
        {
            var test = @"
class TypeName
{
    public void Test()
    {
        var a = ""test"";
        System.String.Equals(a, ""v"");
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "MA0001",
                Message = "StringComparison is missing",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                    // Test0.cs is the name of the file created by VerifyCSharpDiagnostic
                    new DiagnosticResultLocation("Test0.cs", line: 7, column: 9)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

    }
}