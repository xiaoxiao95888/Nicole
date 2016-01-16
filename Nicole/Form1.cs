using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Plastic.helper;
using Plastic.Library.Models;
using Plastic.Service;

namespace Plastic
{
    public partial class Form1 : Form
    {
        private PlasticDataContext _dbContext;
        private List<Product> _products;
        private List<PriceType> _priceTypes;
        private static List<string> UrlList
        {
            get
            {
                return new List<string>
                {
                    "http://www.oilchem.net/",
                    "http://www.315.com.cn/",
                    "http://www.icis-china.com/chemease/information/default.aspx",
                    "http://price.sci99.com/",
                    "http://chem.chem365.net/Web/",
                    "http://www.chinaccm.com/",
                    "http://www.ccf.com.cn/",
                    "http://www.oilmsg.com/index.aspx",
                    "http://www.sinofi.com/",
                    "http://www.chinapu.com/",
                    "http://market.puworld.com/",
                    "http://www.zhaosuliao.com/"

                };
            }
        }
        public Form1()
        {
            InitializeComponent();
            LoadPage();
        }

        private void LoadPage()
        {
            _dbContext= new PlasticDataContext();
            //webBrowser1.Navigate(UrlList[0]);
            //webBrowser2.Navigate(UrlList[1]);
            webBrowser3.Navigate(UrlList[2]);
            webBrowser4.Navigate(UrlList[3]);
            //webBrowser5.Navigate(UrlList[4]);
            //webBrowser6.Navigate(UrlList[5]);
            //webBrowser7.Navigate(UrlList[6]);
            //webBrowser8.Navigate(UrlList[7]);
            //webBrowser9.Navigate(UrlList[8]);
            //webBrowser10.Navigate(UrlList[9]);
            //webBrowser11.Navigate(UrlList[10]);
            webBrowser12.Navigate(UrlList[11]);

            //webBrowser1.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser2.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser3.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser4.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser5.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser6.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser7.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser8.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser9.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser10.DocumentCompleted += webBrowser_DocumentCompleted;
            //webBrowser11.DocumentCompleted += webBrowser_DocumentCompleted;
            tbxstartdate.Text = DateTime.Now.ToShortDateString();
            textBox1.Text = DateTime.Now.ToShortDateString();
            //_dbContext.Sites.Add(new Site
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "安迅思化工",
            //    Domain = "www.icis-china.com"
            //});
            //_dbContext.Sites.Add(new Site
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "卓创咨询化工网",
            //    Domain = "chem.chem99.com"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "采集"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "宏观焦点"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "塑市快讯"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "期市动态"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "汇市动态"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "股市动态"
            //});
            //_dbContext.ArticleTypes.Add(new ArticleType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "独家点评"
            //});
            //_dbContext.SaveChanges();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var types = DbContext.Prices.GroupBy(n => new { n.ProductName, n.ProductStandardStr }).ToList();
            //var products = DbContext.Products.ToArray();
            //foreach (var item in types)
            //{
            //    var product = products.FirstOrDefault(n => n.Name == item.Key.ProductName);
            //    if (product != null)
            //    {
            //        var s = new ProductStandard { Id = Guid.NewGuid(), Name = item.Key.ProductStandardStr };
            //        item.ToList().ForEach(n =>
            //        {
            //            //n.pr
            //        });
            //        product.ProductStandards.Add(s);
            //    }
            //}
            //DbContext.SaveChanges();
        }

        private List<string> _webBrowser4TargetUrl;
        /// <summary>
        /// 卓创咨询化工网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            _products = _dbContext.Products.ToList();
            _priceTypes = _dbContext.PriceTypes.ToList();
            var satrtdate = DateTime.Parse(tbxstartdate.Text).ToShortDateString();
            webBrowser4.DocumentCompleted += webBrowser4_DocumentCompleted;
            _webBrowser4TargetUrl = new List<string>();
            //获取ProductId列表
            const string productIdurl = "http://price.sci99.com/plastic.aspx?pagename=plastic";
            const string encode = "utf-8";
            var condition = new[]
                    {
                        new Condition
                        {
                            Name = "title",
                            Start = "onclick='footprint(this)' id='",
                            End = "'>",
                        }
                    };
            const string loop = "gray-bottom-border";
            //获取产品ID
            //var productIds = CollectionHelper.ProcessorValue(condition, productIdurl, encode, loop).SelectMany(n => n.Value).Select(n => n.Value).ToArray();

            var productIds = new[] { "1079", "1080", "1081", "114", "123", "931", "932", "573", "574", "115", "116", "169", "170", "172", "173" };

            var templatePriceUrl = string.Format(
                "http://price.sci99.com/popWin/priceList.aspx?1=1&ddlProduct={0}&ddlModel=-1&ddlFactory=-1&ddlMarket=-1&ddlArea=-1&ddlPriceType={1}&t=plastic&startDate={2}&endDate={3}&isD=True&PageIndex={4}",
                "{0}", "{1}", satrtdate, DateTime.Now.ToShortDateString(), "{2}");

            var pricetypeIds = new[] { "24", "25", "26" };

            foreach (var productId in productIds)
            {
                foreach (var typeid in pricetypeIds)
                {
                    //首先获取需要采集的目标页码
                    var priceListUrl = string.Format(templatePriceUrl, productId, typeid, 1);
                    var pagecondition = new[]
                    {
                        new Condition
                        {
                            Name = "page",
                            Start = "PageIndex=",
                            End = "\">",
                        }
                    };
                    const string pageloop = "pagination";
                    int x;
                    var maxpaginationList =
                        CollectionHelper.ProcessorValue(pagecondition, priceListUrl, encode, pageloop)
                            .SelectMany(n => n.Value)
                            .Select(n => n.Value).Where(n => Int32.TryParse(n, out x)).Select(Int32.Parse).ToArray();
                    var maxpagination = 0;
                    if (maxpaginationList.Any())
                    {
                        maxpagination = maxpaginationList.Max();
                    }

                    for (var i = 1; i <= maxpagination; i++)
                    {
                        var targetUrl = string.Format(templatePriceUrl, productId, typeid, i);
                        _webBrowser4TargetUrl.Add(targetUrl);
                        //_webBrowser4Loading = true;
                        ////目标URL
                        //var targetUrl = string.Format(templatePriceUrl, productId, typeid, i);

                        //webBrowser4.Navigate(targetUrl);
                        //while (_webBrowser4Loading)
                        //{
                        //    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        //}
                    }
                }
            }

            foreach (var url in _webBrowser4TargetUrl)
            {
                _webBrowser4Loading = true;
                webBrowser4.Navigate(url);
                while (_webBrowser4Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
            //删除重复
            var duplicate =
                _dbContext.Database.SqlQuery<Price>(
                    " select *  from Prices A where (select count(*) from Prices B where  A.PriceDate=B.PriceDate  and A.Type_Id=B.Type_Id and A.ProductStandard_Id=B.ProductStandard_Id  and A.Unit=B.Unit and A.SalesArea=B.SalesArea and A.ProductPrice=B.ProductPrice) >1 ")
                    .ToList();
            var ids = duplicate.GroupBy(
                n =>
                    new
                    {
                        n.PriceDate,
                        n.Type,
                        n.ProductStandard,
                        n.ProductPrice,
                        n.SalesArea,
                        n.Unit
                    }).Select(n => n.FirstOrDefault()).Select(n => n.Id);
            var willdeleteId = duplicate.Where(n => ids.All(i => i != n.Id)).Select(n => "'" + n.Id + "'").ToArray();
            var strqty = string.Format("delete from Prices where Id in ({0})", string.Join(",", willdeleteId));
            _dbContext.Database.ExecuteSqlCommand(strqty);

        }
        bool _webBrowser4Loading = true;
        bool _webBrowser3Loading = true;
        private string Pricetype(string typeId)
        {
            var str = string.Empty;
            switch (typeId)
            {
                case "24":
                    str = "出厂价";
                    break;
                case "25":
                    str = "市场价";
                    break;
                case "26":
                    str = "国际价";
                    break;
            }
            return str;
        }
        private decimal? GetDecimal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return Convert.ToDecimal(str);
        }
        void webBrowser4_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = (WebBrowser)sender;
            var urlParam = wb.Url.PathAndQuery.Split('&');
            var typeId = string.Empty;
            foreach (var val in urlParam.Select(s => s.Split('=')).Where(val => val[0] == "ddlPriceType"))
            {
                typeId = val[1];
            }

            if (wb.Document != null && wb.Document.Url == e.Url)
            {
                var site = _dbContext.Sites.FirstOrDefault(n => n.Domain == "chem.chem99.com");
                if (site != null)
                {
                    var html = webBrowser4.DocumentText;
                    var trcondition = new[]
                        {
                            new Condition
                            {
                                Name = "ProductName",
                                Start = "_lbClass\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "ProductStandard",
                                Start = "_linkmanLabel\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "SalesAreaC",
                                Start = "_lbFactoryNamec\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "SalesArea",
                                Start = "_lbFactoryName\">",
                                End = "</span>",
                            },
                             new Condition
                            {
                                Name = "SalesAreaM",
                                Start = "_lbMarketName\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "ProductPrice",
                                Start = "_lbMPrice\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "Unit",
                                Start = "_Label7\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "PriceTerm",
                                Start = "_Label2\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "PriceDate",
                                Start = "_lbDateTime\">",
                                End = "</span>",
                            },
                        };
                    const string trloop = "tr";

                    var values = CollectionHelper.ProcessorValue(trcondition, string.Empty, string.Empty, trloop, html).Select(n => new
                    {
                        Id = Guid.NewGuid(),
                        PriceType = Pricetype(typeId),
                        PriceDate =
                            Convert.ToDateTime(
                                n.Value.Where(v => v.ConditionName == "PriceDate")
                                    .Select(v => v.Value)
                                    .FirstOrDefault()),
                        ProductName =
                            n.Value.Where(v => v.ConditionName == "ProductName")
                                .Select(v => v.Value)
                                .FirstOrDefault(),
                        PriceTerm =
                            n.Value.Where(v => v.ConditionName == "PriceTerm")
                                .Select(v => v.Value)
                                .FirstOrDefault(),
                        ProductStandardStr =
                            n.Value.Where(v => v.ConditionName == "ProductStandard")
                                .Select(v => v.Value)
                                .FirstOrDefault(),
                        ProductPrice =
                            GetDecimal(
                                n.Value.Where(v => v.ConditionName == "ProductPrice")
                                    .Select(v => v.Value)
                                    .FirstOrDefault()),
                        Unit =
                            n.Value.Where(v => v.ConditionName == "Unit").Select(v => v.Value).FirstOrDefault(),
                        SalesArea =
                            n.Value.Where(v => v.ConditionName == "SalesArea")
                                .Select(v => v.Value)
                                .FirstOrDefault() ?? n.Value.Where(v => v.ConditionName == "SalesAreaC")
                                    .Select(v => v.Value)
                                    .FirstOrDefault() ?? n.Value.Where(v => v.ConditionName == "SalesAreaM")
                                        .Select(v => v.Value)
                                        .FirstOrDefault(),
                        CreatedTime = DateTime.Now,
                        Url = wb.Url.ToString(),
                        Site = site
                    }).ToArray();


                    foreach (var insertvalue in values)
                    {
                        if (string.IsNullOrEmpty(insertvalue.ProductName) == false &&
                            string.IsNullOrEmpty(insertvalue.PriceType) == false)
                        {
                            var product = _products.FirstOrDefault(n => n.Name == insertvalue.ProductName) ?? new Product
                            {
                                Id = Guid.NewGuid(),
                                Name = insertvalue.ProductName,
                                ProductStandards = new Collection<ProductStandard>()
                            };
                            var productStandard =
                                product.ProductStandards.FirstOrDefault(n => n.Name == insertvalue.ProductStandardStr);
                            if (productStandard == null)
                            {
                                productStandard = new ProductStandard
                                {
                                    Id = Guid.NewGuid(),
                                    Name = insertvalue.ProductStandardStr
                                };
                                product.ProductStandards.Add(productStandard);
                            }
                            var priceType = _priceTypes.FirstOrDefault(n => n.Name == insertvalue.PriceType) ??
                                            new PriceType { Id = Guid.NewGuid(), Name = insertvalue.PriceType };
                            _dbContext.Prices.Add(new Price
                            {
                                Id = Guid.NewGuid(),
                                CreatedTime = DateTime.Now,
                                IsDeleted = false,
                                PriceDate = insertvalue.PriceDate,
                                PriceTerm = insertvalue.PriceTerm,
                                ProductPrice = insertvalue.ProductPrice,
                                ProductStandard = productStandard,
                                SalesArea = insertvalue.SalesArea,
                                Site = insertvalue.Site,
                                Type = priceType,
                                Url = insertvalue.Url,
                                Unit = insertvalue.Unit
                            });
                        }


                    }
                    _dbContext.SaveChanges();
                    _products = _dbContext.Products.ToList();
                    _priceTypes = _dbContext.PriceTypes.ToList();
                    _webBrowser4Loading = false;
                }
            }
        }

        private List<string> _companyNewsurls;
        private List<string> _outsideDishNewUrls;
        private List<string> _marketNewUrls;
        private List<string> _aroundTheMarketNewUrls;
        private List<string> _analysisNewUrls;
        /// <summary>
        /// icis截取新闻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var satrtdate = DateTime.Parse(textBox1.Text).ToShortDateString();
            //企业动态
            const string companyNewsurl = "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=12&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=1&SectionName=%c6%f3%d2%b5%b6%af%cc%ac&ViewSectionName=%c6%f3%d2%b5%b6%af%cc%ac&BigSectionName=&page=0";
            //外盘动态
            const string outsideDishNewUrl = "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=12&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%b9%fa%bc%ca%ca%d0%b3%a1%2c%bd%f8%bf%da%ca%d0%b3%a1%2c%b3%f6%bf%da%ca%d0%b3%a1&ViewSectionName=%cd%e2%c5%cc%b6%af%cc%ac&BigSectionName=&page=0";
            //市场传闻
            const string marketDishNewUrl = "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=13&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%ca%d0%b3%a1%b4%ab%ce%c5&ViewSectionName=%ca%d0%b3%a1%b4%ab%ce%c5&BigSectionName=&page=0";
            //各地市场
            const string aroundTheMarketNewUrl = "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=13&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%bb%aa%c4%cf%ca%d0%b3%a1%2c%bb%aa%b6%ab%ca%d0%b3%a1%2c%bb%aa%b1%b1%ca%d0%b3%a1%2c%bb%aa%d6%d0%ca%d0%b3%a1%2c%ce%f7%b1%b1%ca%d0%b3%a1%2c%ce%f7%c4%cf%ca%d0%b3%a1%2c%b6%ab%b1%b1%ca%d0%b3%a1&ViewSectionName=%b8%f7%b5%d8%ca%d0%b3%a1&BigSectionName=&page=0";
            //评论分析
            const string analysisNewUrl =
                "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=11&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=1&SectionName=%b7%d6%ce%f6%c6%c0%c2%db&ViewSectionName=%b7%d6%ce%f6%c6%c0%c2%db&BigSectionName=&page=0";
            const string encode = "gb2312";
            //企业动态
            _companyNewsurls = new List<string>();
            //外盘动态
            _outsideDishNewUrls = new List<string>();
            //市场传闻
            _marketNewUrls = new List<string>();
            //各地市场
            _aroundTheMarketNewUrls = new List<string>();
            //评论分析
            _analysisNewUrls = new List<string>();
            var condition = new[]
                    {
                        new Condition
                        {
                            Name = "pagesize",
                            Start = "page=",
                            End = "\">尾页</a>",
                        }
                    };
            var pagecondition = new[]
                    {
                        new Condition
                        {
                            Name = "pagesize",
                            Start = "<a id=\"ContentPlaceHolder1_gvNews_Title_",
                            End = "\" target=\"_blank\">",
                        }
                    };
            //var companyNewsStrpagesize = CollectionHelper.ProcessorValue(condition, companyNewsurl, encode, string.Empty).SelectMany(n => n.Value).Select(n => n.Value).ToArray().FirstOrDefault();
            //var outsideDishNewpagesize = CollectionHelper.ProcessorValue(condition, outsideDishNewUrl, encode, string.Empty).SelectMany(n => n.Value).Select(n => n.Value).ToArray().FirstOrDefault();
            //var marketNewpagesize = CollectionHelper.ProcessorValue(condition, marketDishNewUrl, encode, string.Empty).SelectMany(n => n.Value).Select(n => n.Value).ToArray().FirstOrDefault();
            //var aroundTheMarketNewpagesize = CollectionHelper.ProcessorValue(condition, aroundTheMarketNewUrl, encode, string.Empty).SelectMany(n => n.Value).Select(n => n.Value).ToArray().FirstOrDefault();
            //var analysisNewpagesize = CollectionHelper.ProcessorValue(condition, analysisNewUrl, encode, string.Empty).SelectMany(n => n.Value).Select(n => n.Value).ToArray().FirstOrDefault();
            var companyNewsStrpagesize = "0";
            var outsideDishNewpagesize = "0";
            var marketNewpagesize = "0";
            var aroundTheMarketNewpagesize = "0";
            var analysisNewpagesize = "0";
            if (companyNewsStrpagesize != null)
            {
                var pagesize = Int32.Parse(companyNewsStrpagesize);
                for (var i = 0; i <= pagesize; i++)
                {
                    var pageurl = string.Format(
                        "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=12&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=1&SectionName=%c6%f3%d2%b5%b6%af%cc%ac&ViewSectionName=%c6%f3%d2%b5%b6%af%cc%ac&BigSectionName=&page={0}",
                        i);
                    _companyNewsurls.AddRange(CollectionHelper.ProcessorValue(pagecondition, pageurl, encode, "<td>")
                        .SelectMany(n => n.Value)
                        .Select(n => n.Value)
                        .ToArray()
                        .Select(n => "http://www.icis-china.com/chemease" + n.Split('"')[2].Replace("..", ""))
                        .ToList());
                }
            }
            if (outsideDishNewpagesize != null)
            {
                var pagesize = Int32.Parse(outsideDishNewpagesize);
                for (var i = 0; i <= pagesize; i++)
                {
                    var pageurl = string.Format(
                        "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=12&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%b9%fa%bc%ca%ca%d0%b3%a1%2c%bd%f8%bf%da%ca%d0%b3%a1%2c%b3%f6%bf%da%ca%d0%b3%a1&ViewSectionName=%cd%e2%c5%cc%b6%af%cc%ac&BigSectionName=&page={0}",
                        i);
                    _outsideDishNewUrls.AddRange(CollectionHelper.ProcessorValue(pagecondition, pageurl, encode, "<td>")
                        .SelectMany(n => n.Value)
                        .Select(n => n.Value)
                        .ToArray()
                        .Select(n => "http://www.icis-china.com/chemease" + n.Split('"')[2].Replace("..", ""))
                        .ToList());
                }
            }
            if (marketNewpagesize != null)
            {
                var pagesize = Int32.Parse(marketNewpagesize);
                for (var i = 0; i <= pagesize; i++)
                {
                    var pageurl = string.Format(
                        "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=13&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%ca%d0%b3%a1%b4%ab%ce%c5&ViewSectionName=%ca%d0%b3%a1%b4%ab%ce%c5&BigSectionName=&page={0}",
                        i);
                    _marketNewUrls.AddRange(CollectionHelper.ProcessorValue(pagecondition, pageurl, encode, "<td>")
                        .SelectMany(n => n.Value)
                        .Select(n => n.Value)
                        .ToArray()
                        .Select(n => "http://www.icis-china.com/chemease" + n.Split('"')[2].Replace("..", ""))
                        .ToList());
                }
            }
            if (aroundTheMarketNewpagesize != null)
            {
                var pagesize = Int32.Parse(aroundTheMarketNewpagesize);
                for (var i = 0; i <= pagesize; i++)
                {
                    var pageurl = string.Format(
                        "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=13&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=2&SectionName=%bb%aa%c4%cf%ca%d0%b3%a1%2c%bb%aa%b6%ab%ca%d0%b3%a1%2c%bb%aa%b1%b1%ca%d0%b3%a1%2c%bb%aa%d6%d0%ca%d0%b3%a1%2c%ce%f7%b1%b1%ca%d0%b3%a1%2c%ce%f7%c4%cf%ca%d0%b3%a1%2c%b6%ab%b1%b1%ca%d0%b3%a1&ViewSectionName=%b8%f7%b5%d8%ca%d0%b3%a1&BigSectionName=&page={0}",
                        i);
                    _aroundTheMarketNewUrls.AddRange(
                        CollectionHelper.ProcessorValue(pagecondition, pageurl, encode, "<td>")
                            .SelectMany(n => n.Value)
                            .Select(n => n.Value)
                            .ToArray()
                            .Select(n => "http://www.icis-china.com/chemease" + n.Split('"')[2].Replace("..", ""))
                            .ToList());
                }
            }
            if (analysisNewpagesize != null)
            {
                var pagesize = Int32.Parse(analysisNewpagesize);
                for (var i = 0; i <= pagesize; i++)
                {
                    var pageurl = string.Format(
                        "http://www.icis-china.com/chemease/Information/ShowMoreNews.aspx?MoreType=11&ProductType=1&ProductName=%cb%dc%c1%cf%cd%a8%d3%c3%c1%cf%2cPP-PE&SectionGrade=1&SectionName=%b7%d6%ce%f6%c6%c0%c2%db&ViewSectionName=%b7%d6%ce%f6%c6%c0%c2%db&BigSectionName=&page={0}",
                        i);
                    _analysisNewUrls.AddRange(
                        CollectionHelper.ProcessorValue(pagecondition, pageurl, encode, "<td>")
                            .SelectMany(n => n.Value)
                            .Select(n => n.Value)
                            .ToArray()
                            .Select(n => "http://www.icis-china.com/chemease" + n.Split('"')[2].Replace("..", ""))
                            .ToList());
                }
            }
            webBrowser3.DocumentCompleted += webBrowser3_DocumentCompleted;
            foreach (var url in _analysisNewUrls)
            {
                _webBrowser3Loading = true;
                _plate = new Guid("172CDE38-54BF-AA09-137C-431FCA63659B");
                webBrowser3.Navigate(url);
                while (_webBrowser3Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
            foreach (var url in _companyNewsurls)
            {
                _webBrowser3Loading = true;
                _plate = new Guid("172CDE38-54BF-AA09-137C-431FCA63659B");
                webBrowser3.Navigate(url);
                while (_webBrowser3Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
            foreach (var url in _outsideDishNewUrls)
            {
                _webBrowser3Loading = true;
                _plate = new Guid("172CDE38-54BF-AA09-137C-431FCA63659B");
                webBrowser3.Navigate(url);
                while (_webBrowser3Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
            foreach (var url in _marketNewUrls)
            {
                _webBrowser3Loading = true;
                _plate = new Guid("172CDE38-54BF-AA09-137C-431FCA63659B");
                webBrowser3.Navigate(url);
                while (_webBrowser3Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
            foreach (var url in _aroundTheMarketNewUrls)
            {
                _webBrowser3Loading = true;
                _plate = new Guid("172CDE38-54BF-AA09-137C-431FCA63659B");
                webBrowser3.Navigate(url);
                while (_webBrowser3Loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
            }
        }

        private Guid _plate;
        void webBrowser3_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = (WebBrowser)sender;
            if (wb.Document != null && wb.Document.Url == e.Url)
            {
                var site = _dbContext.Sites.FirstOrDefault(n => n.Domain == "www.icis-china.com");
                var articleType = _dbContext.ArticleTypes.FirstOrDefault(n => n.Name == "采集");
                if (site != null && articleType != null)
                {
                    var html = webBrowser3.DocumentText;
                    var condition = new[]
                        {
                            new Condition
                            {
                                Name = "Title",
                                Start = "<span id=\"ContentPlaceHolder1_SiteMapPath_CTitle\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "CreateTime",
                                Start = "<span id=\"ContentPlaceHolder1_l_Pubdate\">",
                                End = "</span>",
                            },
                            new Condition
                            {
                                Name = "Details",
                                Start = "<span id=\"ContentPlaceHolder1_l_Content\">",
                                End = "<div id=\"ContentPlaceHolder1_DivSource",
                            }
                        };

                    var values =
                        CollectionHelper.ProcessorValue(condition, string.Empty, string.Empty, string.Empty, html)
                            .ToArray()
                            .Select(n => new Article
                            {
                                Id = Guid.NewGuid(),
                                Title = n.Value.Where(v => v.ConditionName == "Title")
                                    .Select(v => v.Value)
                                    .FirstOrDefault(),
                                CollectDate = DateTime.Parse(n.Value.Where(v => v.ConditionName == "CreateTime")
                                    .Select(v => v.Value)
                                    .FirstOrDefault()),
                                Description = n.Value.Where(v => v.ConditionName == "Title")
                                    .Select(v => v.Value)
                                    .FirstOrDefault(),
                                Body = n.Value.Where(v => v.ConditionName == "Details")
                                    .Select(v => ClearHtml(v.Value.Replace("安迅思", "本站").Replace("icis中国", string.Empty).Replace("icis", string.Empty)))
                                    .FirstOrDefault(),
                                Site = site,
                                ArticleType = articleType,
                                FromUrl = e.Url.ToString(),
                                CreatedTime = DateTime.Now,
                                ViewCount = 0
                            }).ToArray();


                    foreach (var item in values)
                    {
                        if (
                            !_dbContext.Articles.Any(
                                n => n.Title == item.Title
                                    && n.CollectDate == item.CollectDate
                                    && n.ArticleType.Id == item.ArticleType.Id
                                    && n.Body == item.Body
                                    && n.FromUrl == item.FromUrl
                                    ))
                        {
                            _dbContext.Articles.Add(item);
                        }

                    }
                    _dbContext.SaveChanges();
                    _webBrowser3Loading = false;
                }
            }
        }
        /// <summary>  
        /// 清除文本中Html的标签  
        /// </summary>  
        /// <param name="content"></param>  
        /// <returns></returns>  
        protected string ClearHtml(string content)
        {
            content = Zxj_ReplaceHtml("&#[^>]*;", "", content);
            content = Zxj_ReplaceHtml("</?marquee[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?object[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?param[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?embed[^>]*>", "", content);
            //Content = Zxj_ReplaceHtml("</?table[^>]*>", "", Content);
            content = Zxj_ReplaceHtml(" ", "", content);
            //Content = Zxj_ReplaceHtml("</?tr[^>]*>", "", Content);
            //Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            //Content = Zxj_ReplaceHtml("</?p[^>]*>", "", Content);
            content = Zxj_ReplaceHtml("</?a[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?img[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?tbody[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?li[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?span[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?div[^>]*>", "", content);
            //Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            //Content = Zxj_ReplaceHtml("</?td[^>]*>", "", Content);
            content = Zxj_ReplaceHtml("</?script[^>]*>", "", content);
            content = Zxj_ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", content);
            content = Zxj_ReplaceHtml("on(mouse|exit|error|click|key)", "", content);
            content = Zxj_ReplaceHtml("<\\?xml[^>]*>", "", content);
            content = Zxj_ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?font[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?b[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?u[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?i[^>]*>", "", content);
            content = Zxj_ReplaceHtml("</?strong[^>]*>", "", content);
            var clearHtml = content;
            return clearHtml;
        }
        /// <summary>  
        /// 清除文本中的Html标签  
        /// </summary>  
        /// <param name="patrn">要替换的标签正则表达式</param>  
        /// <param name="strRep">替换为的内容</param>  
        /// <param name="content">要替换的内容</param>  
        /// <returns></returns>  
        private string Zxj_ReplaceHtml(string patrn, string strRep, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = string.Empty;
            }
            var rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
            var strTxt = rgEx.Replace(content, strRep);
            return strTxt;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var site = _dbContext.Sites.FirstOrDefault(n => n.Domain == "www.zhaosuliao.com");

            if (site != null)
            {
                var html = webBrowser12.DocumentText;
                const string loop = "<div class=\"content_block\">";

                var condition = new[]
                {
                    new Condition
                    {
                        Name = "Factory",
                        Start = "<p class=\"p1\"><span>",
                        End = "</span>",
                    },
                    new Condition
                    {
                        Name = "ProductStandard",
                        Start = "<span class=\"sp2\">",
                        End = "</span>",
                    },
                    new Condition
                    {
                        Name = "Price",
                        Start = "<p class=\"p2 \"><span class=\"sp1\">",
                        End = "</span>",
                    }
                };
                const string encode = "utf-8";
                var dateCondition = new[]
                {
                    new Condition
                    {
                        Name = "PriceDate",
                        Start = "<p>最后更新：",
                        End = "</p>",
                    }
                };
                var pricedate =
                    CollectionHelper.ProcessorValue(dateCondition, string.Empty, encode, string.Empty, html)
                        .ToArray()
                        .Select(n => n.Value.Where(v => v.ConditionName == "PriceDate")
                            .Select(v => v.Value)
                            .FirstOrDefault())
                        .FirstOrDefault();
                var date = Convert.ToDateTime(DateTime.Now.Year + "年" + pricedate);
                var values =
                    CollectionHelper.ProcessorValue(condition, string.Empty, encode, loop, html)
                        .ToArray()
                        .Select(n => new PetrochemicalPrice
                        {
                            Id = Guid.NewGuid(),
                            Price = GetDecimal(
                                n.Value.Where(v => v.ConditionName == "Price")
                                    .Select(v => v.Value)
                                    .FirstOrDefault()),
                            Product = n.Value.Where(v => v.ConditionName == "ProductStandard")
                                .Select(v => v.Value.Split(' ')[0])
                                .FirstOrDefault(),
                            ProductStandard = n.Value.Where(v => v.ConditionName == "ProductStandard")
                                .Select(v => v.Value.Split(' ')[1])
                                .FirstOrDefault(),
                            Site = site,
                            Factory = n.Value.Where(v => v.ConditionName == "Factory")
                                .Select(v => v.Value)
                                .FirstOrDefault(),
                            CreatedTime = date,
                            Url = webBrowser12.Url.ToString()
                            //Description = n.Value.Where(v => v.ConditionName == "Title")
                            //    .Select(v => v.Value)
                            //    .FirstOrDefault(),

                        }).ToArray();


                foreach (var item in values)
                {
                    if (
                        !_dbContext.PetrochemicalPrices.Any(
                            n => n.Site.Id == item.Site.Id
                                 && n.Factory == item.Factory
                                 && n.Price == item.Price
                                 && n.Product == item.Product
                                 && n.ProductStandard == item.ProductStandard
                                 && n.SalesArea == item.SalesArea
                            ))
                    {
                        _dbContext.PetrochemicalPrices.Add(item);
                    }

                }
                _dbContext.SaveChanges();
                MessageBox.Show("完成");
            }
        }
    }
}
