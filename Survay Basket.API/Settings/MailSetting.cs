namespace Survay_Basket.API.Settings;

public class MailSetting
{
    public const string SectionName = "MailSettings";
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host {  get; set; } = string.Empty;
    public string DisplayName {  get; set; } = string.Empty;
    public int port { get; set; }
}
