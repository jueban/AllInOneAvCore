using Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

public class WebScanUrlModel
{
    public int Id { get; set; }
    public string AvId { get; set; }
    public string URL { get; set; }
    public string Name { get; set; }
    public bool IsDownload { get; set; }
    public WebScanUrlSite ScanUrlSite { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class CommonModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public CommonModelType Type { get; set; }
}

public enum CommonModelType
{
    Category = 1,
    Actress = 2,
    Director = 3,
    Publisher = 4,
    Company = 5
}

public class AvModel
{
    public int Id { get; set; }
    public string AvId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string PicUrl { get; set; }
    public string Infos { get; set; }
    public string FileNameWithoutExtension { get; set; }
    public int AvLength { get; set; }
    public WebScanUrlSite Site { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public List<CommonModel> InfoObj
    {
        get
        {
            return !string.IsNullOrEmpty(this.Infos) ? JsonSerializer.Deserialize<List<CommonModel>>(this.Infos) : new List<CommonModel>();
        }
    }

    public override string ToString()
    {
        return $"∑¨∫≈:{this.AvId}, √˚≥∆:{this.Name}, –≈œ¢:{this.Infos}, Õ¯÷∑:{this.Url}, Õº∆¨µÿ÷∑:{this.PicUrl}";
    }
}

public enum WebScanUrlSite
{
    JavLibrary = 1,
    JavBus = 2,
}