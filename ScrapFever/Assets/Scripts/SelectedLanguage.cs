using System;

public static class SelectedLanguage
{
    public static event Action languageChanged;
    public static LanguageType value
    {
        get
        {
            return _value;
        }
        set
        {
            languageChanged?.Invoke();
            _value = value;
        }
    }

    private static LanguageType _value = LanguageType.EN;
}
