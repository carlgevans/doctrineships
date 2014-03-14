namespace DoctrineShips.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationResult : IValidationResult
    {
        private IDictionary<string, string> errors;

        public ValidationResult()
        {
            this.errors = new Dictionary<string, string>();
        }

        public bool IsValid
        {
            get
            { 
                if (this.errors.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IDictionary<string, string> Errors
        {
            get
            {
                return this.errors;
            }
        }

        public void AddError(string key, string errorMessage)
        {
            this.errors.Add(key, errorMessage);
        }

        public void Clear()
        {
            this.errors.Clear();
        }

        public void Merge(IValidationResult validationResult)
        {
            this.errors = this.errors.Concat(validationResult.Errors).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
        }
    }
}