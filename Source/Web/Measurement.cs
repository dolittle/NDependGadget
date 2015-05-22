using System;
using Bifrost.Read;

namespace Web
{
    public class Measurement : IReadModel, IEquatable<Measurement>
    {
        public DateTime Time { get; set; }

        public int NumberOfRules { get; set; }
        public int NumberOfRulesViolated { get; set; }
        public int NumberOfRulesViolations { get; set; }
        public int NumberOfCriticalRules { get; set; }
        public int NumberOfCriticalRulesViolated { get; set; }
        public int NumberOfCriticalRulesViolations { get; set; }
        public int NumberOfLinesOfCode { get; set; }
        public int NumberOfLinesOfCodeJustMyCode { get; set; }
        public int NumberOfLinesOfCodeNotMyCode { get; set; }
        public int NumberOfSourceFiles { get; set; }
        public int NumberOfILInstructions { get; set; }
        public int NumberOfILInstructionsNotMyCode { get; set; }
        public int NumberOfLinesOfComments { get; set; }
        public int NumberOfAssemblies { get; set; }
        public int NumberOfNamespaces { get; set; }
        public int NumberOfTypes { get; set; }
        public int NumberOfPublicTypes { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAbstractClasses { get; set; }
        public int NumberOfInterfaces { get; set; }
        public int NumberOfStructures { get; set; }
        public int NumberOfMethods { get; set; }
        public int NumberOfAbstractMethods { get; set; }
        public int NumberOfConcreteMethods { get; set; }
        public int NumberOfFields { get; set; }
        public int MaxNumberOfLinesOfCodeForMethodsJustMyCode { get; set; }
        public int AverageNumberOfLinesOfCodeForMethods { get; set; }
        public int AverageNumberOfLinesOfCodeForMethodsWithAtLeast3LinesOfCode { get; set; }
        public int MaxNumberOfLinesOfCodeForTypesJustMyCode { get; set; }
        public int AverageNumberOfLinesOfCodeForTypes { get; set; }
        public int MaxCyclomaticComplexityForTypes { get; set; }
        public int AverageCyclomaticComplexityForTypes { get; set; }
        public int MaxCyclomaticComplexityForMethods { get; set; }
        public int AverageCyclomaticComplexityForMethods { get; set; }
        public int MaxILCyclomaticComplexityForMethods { get; set; }
        public int AverageILCyclomaticComplexityForMethods { get; set; }
        public int MaxILNestingDepthForMethods { get; set; }
        public int AverageILNestingDepthForMethods { get; set; }
        public int MaxNumberOfMethodsForTypes { get; set; }
        public int AverageNumberOfMethodsForTypes { get; set; }
        public int MaxNumberOfMethodsForInterfaces { get; set; }
        public int AverageNumberOfMethodsForInterfaces { get; set; }
        public int NumberOfThirdPartyAssembliesUsed { get; set; }
        public int NumberOfThirdPartyNamespacesUsed { get; set; }
        public int NumberOfThirdPartyTypesUsed { get; set; }
        public int NumberOfThirdPartyMethodsUsed { get; set; }
        public int NumberOfThirdPartyFieldsUsed { get; set; }


        public bool Equals(Measurement other)
        {
            return other.Time == Time;
        }
    }
}
