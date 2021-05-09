using System;

public class WebScanUrlModel
{
    public string Id { get; set; }
    public string URL { get; set; }
    public string Name { get; set; }
    public bool IsDownload { get; set; }
    public WebScanUrlSite ScanUrlSite { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public enum WebScanUrlSite
{
    JavLibrary = 1,
    JavBus = 2,
}