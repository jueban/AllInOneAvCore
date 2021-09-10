using Models;
using Newtonsoft.Json;
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
    None = 0,
    Category = 1,
    Actress = 2,
    Director = 3,
    Publisher = 4,
    Company = 5,
    Series = 6,
    Prefix = 7
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
    public string LocalPic { get; set; }
    [JsonIgnore]
    public List<CommonModel> InfoObj
    {
        get
        {
            return !string.IsNullOrEmpty(this.Infos) ? System.Text.Json.JsonSerializer.Deserialize<List<CommonModel>>(this.Infos) : new List<CommonModel>();
        }
    }

    //public bool Equals(AvModel model)
    //{
    //    if (ReferenceEquals(null, model)) return false;

    //    if (ReferenceEquals(this, model)) return true;

    //    if (!(this.AvId + this.Name).Equals(model.AvId + model.Name)) return false;

    //    return true;
    //}

    //public override bool Equals(object obj)
    //{
    //    if (ReferenceEquals(null, obj)) return false;

    //    if (ReferenceEquals(this, obj)) return true;

    //    if (this.GetType() != obj.GetType()) return false;

    //    return Equals((AvModel)obj);
    //}

    //public override int GetHashCode()
    //{
    //    return (this.AvId + this.Name).GetHashCode();
    //}

    //public static bool operator ==(AvModel left, AvModel right)
    //{
    //    return Equals(left, right);
    //}

    //public static bool operator !=(AvModel left, AvModel right)
    //{
    //    return !Equals(left, right);
    //}

    public override string ToString()
    {
        return $"∑¨∫≈:{this.AvId}, √˚≥∆:{this.Name}, –≈œ¢:{this.Infos}, Õ¯÷∑:{this.Url}, Õº∆¨µÿ÷∑:{this.PicUrl}";
    }
}

public enum WebScanUrlSite
{
    None = 0,
    JavLibrary = 1,
    JavBus = 2,
}