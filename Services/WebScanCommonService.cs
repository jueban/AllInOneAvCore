using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class WebScanCommonService
    {
        //保存扫描URL模型
        public async static Task<int> SaveWebScanUrlModel(WebScanUrlModel entity)
        {
            int ret = 0;

            switch (entity.ScanUrlSite)
            {
                case WebScanUrlSite.JavLibrary:
                    ret = await new JavLibraryDAL().InsertWebScanUrlModel(entity);
                    break;

                case WebScanUrlSite.JavBus:
                    break;
            }

            return ret;
        }
    }
}
