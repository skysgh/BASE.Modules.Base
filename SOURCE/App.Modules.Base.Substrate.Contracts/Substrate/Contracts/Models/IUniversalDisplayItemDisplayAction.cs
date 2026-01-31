namespace App.Modules.Base.Substrate.Contracts.Models
{
    public interface IUniversalDisplayItemDisplayAction
    {
        public string Label { get; set; } = string.Empty;
        public string ActionKey { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}
