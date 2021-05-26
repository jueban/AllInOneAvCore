using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class EverythingService
    {
        public static string Extensions = "ext:3g2;3gp;3gp2;3gpp;amr;amv;asf;avi;bdmv;bik;d2v;divx;drc;dsa;dsm;dss;dsv;evo;f4v;flc;fli;flic;flv;hdmov;ifo;ivf;m1v;m2p;m2t;m2ts;m2v;m4b;m4p;m4v;mkv;mp2v;mp4;mp4v;mpe;mpeg;mpg;mpls;mpv2;mpv4;mov;mts;ogm;ogv;pss;pva;qt;ram;ratdvd;rm;rmm;rmvb;roq;rpm;smil;smk;swf;tp;tpr;ts;vob;vp6;webm;wm;wmp;wmv";

        public async static Task<EverythingResult> EverythingSearch(string content)
        {
            var retModel = new EverythingResult();
            string resContent = "";

            using (HttpClient client = new())
            {
                resContent = await client.GetStringAsync("http://localhost:8086/" + @"?s=&o=0&j=1&p=c&path_column=1&size_column=1&j=1&q=!c:\ " + Extensions + " " + content);
            }

            if (!string.IsNullOrEmpty(resContent))
            {
                retModel = JsonConvert.DeserializeObject<EverythingResult>(resContent);

                if (retModel != null && retModel.results != null && retModel.results.Count > 0)
                {
                    retModel.results = retModel.results.OrderByDescending(x => double.Parse(x.size)).ToList();

                    foreach (var r in retModel.results)
                    {
                        r.sizeStr = FileUtility.GetAutoSizeString(double.Parse(r.size), 1);
                        r.location = "本地";
                    }

                    return retModel;
                }
            }

            return retModel;
        }

        public async static Task<EverythingResult> SearchBothLocalAnd115(string content)
        {
            var retModel = new EverythingResult();

            retModel = await EverythingSearch(content);

            if (retModel == null || retModel.results == null || retModel.results.Count <= 0)
            {
                retModel = new EverythingResult
                {
                    results = new List<EverythingFileResult>()
                };

                List<OneOneFiveFileItemModel> oneOneFiveFiles = await OneOneFiveService.Get115SearchFileResult(content, OneOneFiveFolder.AV, true);

                if (oneOneFiveFiles != null && oneOneFiveFiles.Any())
                {
                    retModel.totalResults = oneOneFiveFiles.Count + "";

                    foreach (var file in oneOneFiveFiles)
                    {
                        EverythingFileResult temp = new()
                        {
                            size = file.s + "",
                            sizeStr = FileUtility.GetAutoSizeString(double.Parse(file.s + ""), 1),
                            location = "115网盘",
                            name = file.n,
                            path = file.pc,
                            type = file.@class
                        };

                        retModel.results.Add(temp);
                    }
                }
            }

            return retModel;
        }
    }
}
