using App.Modules.Base.Substrate.Contracts.Models;

public class UniversalDisplayItemDisplayAction : IUniversalDisplayItemDisplayAction
{
        public string Label { get; set; } = string.Empty;
        public string ActionKey { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}
