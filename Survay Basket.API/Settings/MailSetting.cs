namespace Survay_Basket.API.Settings;

public class MailSetting
{
    public const string SectionName = "MailSettings";
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host {  get; set; } = string.Empty;
    public string DisplayName {  get; set; } = string.Empty;
    public int Port { get; set; }
    public override string ToString()
        => $"{Mail}, {Password}, {Host}, {DisplayName}, {Port}";
}
