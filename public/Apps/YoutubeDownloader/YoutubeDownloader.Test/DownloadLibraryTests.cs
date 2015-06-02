using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using YoutubeDownloader.Libraries.YoutubeExtractor;
using YoutubeDownloader;
using YoutubeDownloader.ViewModels;

namespace YoutubeDownloader.Test
{
    [TestClass]
    public class DownloadLibraryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string url = "https://r5---sn-cx5h-itqs.googlevideo.com/videoplayback?mm=31&key=yt5&clen=2043407&ms=au&mt=1433076710&pl=21&mv=m&gcr=in&sver=3&fexp=9405973,9407116,9407662,9407718,9408142,9408207,9408420,9408589,9408710,9412503,9412773,9413503,9415304,9415443,952612&gir=yes&sparams=clen,dur,gcr,gir,id,initcwndbps,ip,ipbits,itag,keepalive,lmt,mime,mm,ms,mv,pl,requiressl,source,upn,expire&upn=6XURi05BGHo&mime=audio/webm&expire=1433098414&ipbits=0&initcwndbps=878750&requiressl=yes&lmt=1432853362146001&itag=249&keepalive=yes&ip=49.207.150.124&dur=319.041&source=youtube&id=o-AMwlAVLz57HpVRNNIhSZ11JCZLOW4q36msUAsZ6L8FGv&signature=D37337E3F878CD4CEA0D8CDE2B57880185CF944E49.B8D31154166D40AC1A0D189EB1B90B0032F81294D94F&ratebypass=yes";
            var response = YoutubeExtractor.HttpHelper.GetHeadeResponse(url);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string url = "https://www.youtube.com/watch?v=KuttNmIDB1w";
            var response = DownloadInfoPageViewModel.GetVideoIdFromYoutubeUrl(url);
        }

        [TestMethod]
        public void RunTestPages()
        {
            //DownloadInfoPage page = new DownloadInfoPage();
        }
    }
}
