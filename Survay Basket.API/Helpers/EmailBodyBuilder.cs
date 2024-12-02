namespace Survay_Basket.API.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templeteValus)
    {
        var templetePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        var streamReader = new StreamReader(templetePath);
        var body = streamReader.ReadToEnd();
        streamReader.Close();

        foreach(var item in templeteValus) 
            body = body.Replace(item.Key, item.Value);

        return body;
    }
}
