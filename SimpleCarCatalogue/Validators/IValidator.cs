namespace SimpleCarCatalogue.Validators
{
    public interface IValidator<T>
    {
        string[] ValidationMessages { get; }
        bool Validate(T itemToValidate);
    }
}
