namespace DoctrineShips.Validation
{
    using System.Collections.Generic;

    public interface IValidationResult
    {
        bool IsValid { get; }
        IDictionary<string, string> Errors { get; }
        void AddError(string key, string errorMessage);
        void Merge(IValidationResult validationResult);
        void Clear();
    }
}